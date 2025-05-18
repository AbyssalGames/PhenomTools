using System;
using System.IO;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Properties;
using UnityEngine.Networking;

namespace PhenomTools.Utility
{
  public enum CardinalDirection
  {
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
  }

  public static partial class PhenomUtils
  {
    #region Misc

    public static long GetUtcTimestamp()
    {
      return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public static long GetUtcTimestamp(DateTime dateTime)
    {
      return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }

    public static TimeSpan GetTimeSince(long utcTimestamp)
    {
      return DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(utcTimestamp);
    }

    public static string GetCurrentPlatformString()
    {
      return GetPlatformString(Application.platform);
    }

    public static string GetPlatformString(RuntimePlatform platform)
    {
      return platform switch
      {
        RuntimePlatform.Android => "Android",
        RuntimePlatform.IPhonePlayer => "iOS",
        RuntimePlatform.WebGLPlayer => "WebGL",
        _ => platform.ToString()
      };
    }

    /// <summary>Gets a string from a text file at the specified path.</summary>
    /// <param name="path">The path to the file.</param>
    public static string GetStringFromFile(string path)
    {
      if (!File.Exists(path))
        throw new Exception("File does not exist at specified path");

      StreamReader reader = new StreamReader(path);
      string text = reader.ReadToEnd();
      reader.Close();

      return text;
    }

    public static int GetDirectionOfRelativeObject(Vector3 baseObjectPos, Vector3 baseObjectAxis, Vector3 otherObjectPos)
    {
      return Vector3.Dot(baseObjectAxis, otherObjectPos - baseObjectPos) > 0 ? -1 : 1;
    }

    public static async UniTask<Texture> GetTextureFromURL(string url)
    {
      if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        throw new InvalidPathException("Invalid URL: " + url);

      UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
      await www.SendWebRequest();
      return www.result == UnityWebRequest.Result.Success ? DownloadHandlerTexture.GetContent(www) : null;
    }

    public static void GetVideoFromURL(string url, Action<Texture> onSuccess, Action<string> onError)
    {

    }

    #endregion

    #region Random

    public static int GetWeightedNumber(int[] finalValues, float[] weights)
    {
      float[] weightIndex = new float[finalValues.Length];

      int finalNumber = finalValues[0];
      float randomNumber = UnityEngine.Random.Range(0, weights.Sum());
      float counter = 0;

      for (int i = 0; i < finalValues.Length; i++)
      {
        counter += weights[i];
        weightIndex[i] = counter;
      }

      for (int i = 0; i < finalValues.Length; i++)
      {
        if (randomNumber < weightIndex[i])
        {
          finalNumber = finalValues[i];
          break;
        }
      }

      return finalNumber;
    }

    public static T GetWeightedValue<T>(T[] finalValues, float[] weights)
    {
      float[] weightIndex = new float[finalValues.Length];

      T finalValue = finalValues[0];
      float randomNumber = UnityEngine.Random.Range(0, weights.Sum());
      float counter = 0;

      for (int i = 0; i < finalValues.Length; i++)
      {
        counter += weights[i];
        weightIndex[i] = counter;
      }

      for (int i = 0; i < finalValues.Length; i++)
      {
        if (randomNumber < weightIndex[i])
        {
          finalValue = finalValues[i];
          break;
        }
      }

      return finalValue;
    }

    public static string GetRandomString(int length, UnityEngine.UI.InputField.ContentType contentType = UnityEngine.UI.InputField.ContentType.Alphanumeric)
    {
      string newString = "";
      const string availableChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

      for (int i = 0; i < length; i++)
      {
        newString = string.Concat(newString, availableChars[UnityEngine.Random.Range(0, availableChars.Length)]);
      }

      return newString;
    }

    #endregion

    #region Mobile

    public static float GetKeyboardHeightRelative(float canvasHeight, bool includeInput)
    {
      return (GetKeyboardHeight(includeInput) / Display.main.systemHeight) * canvasHeight;
    }

    public static float GetKeyboardHeight(bool includeInput)
    {
#if UNITY_EDITOR
      return 300f;
#elif UNITY_ANDROID
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
                var view = unityPlayer.Call<AndroidJavaObject>("getView");

                var result = 0;

                if (view != null)
                {
                    using (var rect = new AndroidJavaObject("android.graphics.Rect"))
                    {
                        view.Call("getWindowVisibleDisplayFrame", rect);
                        result = Display.main.systemHeight - rect.Call<int>("height");
                    }

                    if (includeInput)
                    {
                        var dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
                        var decorView = dialog?.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");

                        if (decorView != null)
                        {
                            var decorHeight = decorView.Call<int>("getHeight");
                            result += decorHeight;
                        }
                        else
                        {
                            decorView = dialog?.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("b");

                            if (decorView != null)
                            {
                                var decorHeight = decorView.Call<int>("getHeight");
                                result += decorHeight;
                            }
                            else
                            {
                                result += 60;
                            }
                        }
                    }
                }

                return result;
            }
#else
            var height = Mathf.RoundToInt(TouchScreenKeyboard.area.height);
            return height >= Display.main.systemHeight ? 0 : height;
#endif
    }

    #endregion
  }
}