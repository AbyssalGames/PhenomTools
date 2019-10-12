using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = System.Random;

public static class PhenomExtensions
{
    private static Random rng = new System.Random();

    private static CoroutineHolder _coroutineHolder;
    public static CoroutineHolder coroutineHolder
    {
        get
        {
            if (_coroutineHolder == null)
            {
                _coroutineHolder = new GameObject("CoroutineHolder").AddComponent<CoroutineHolder>();
                UnityEngine.Object.DontDestroyOnLoad(_coroutineHolder.gameObject);
            }

            return _coroutineHolder;
        }
    }

    public static MonoBehaviour SetAsSingleton(this MonoBehaviour type, MonoBehaviour instance)
    {
        if (instance == null)
        {
            instance = type;
            UnityEngine.Object.DontDestroyOnLoad(instance);
            return instance;
        }
        else
            UnityEngine.Object.DestroyImmediate(type.gameObject);

        return instance;
    }

    #region Integer
    public static int Remap(this int oldValue, int oldMin, int oldMax, int newMin, int newMax)
    {
        float floatValue = oldValue;
        return Mathf.RoundToInt(floatValue.Remap(oldMin, oldMax, newMin, newMax));
    }


    public static float ToFloat(this int value)
    {
        return value;
    }

    public static bool ToBoolBinary(this int i)
    {
        if (i == 1)
            return true;
        else
            return false;
    }

    public static bool ToBoolEvenOdd(this int i)
    {
        if (i % 2 == 0)
            return true;
        else
            return false;
    }

    public static int Loop(this int i, int m)
    {
        int n = i % m;
        if (n < 0)
            n = m + i;

        return n;
    }

    public static void RollToValue(ref int i, int o, float t)
    {
        int newValue = i;
        coroutineHolder.StartCoroutine(DoIntRoll(i, o, t, (int outValue) => { newValue = outValue; }));
        i = newValue;
    }

    private static IEnumerator DoIntRoll(int i, int o, float t, Action<int> callback)
    {
        float time = 0f;
        int newValue;

        while (time < 1f)
        {
            time += Time.deltaTime / t;

            newValue = Mathf.RoundToInt(Mathf.Lerp(i, o, time));
            callback?.Invoke(newValue);

            yield return null;
        }
    }
    #endregion
    #region Float
    public static float Remap(this float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        return ((oldValue - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
    }
    public static float RemapInverse(this float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        return -(-oldValue).Remap(-oldMax, -oldMin, -newMin, -newMax);
    }
    public static float RemapNormalized(this float oldValue, float oldMin, float oldMax)
    {
        return (oldValue - oldMin) * 1 / (oldMax - oldMin);
    }
    public static float RemapInverseNormalized(this float oldValue, float oldMin, float oldMax)
    {
        return -(-oldValue).Remap(-oldMax, -oldMin, 0, -1);
    }

    public static float Loop(this float i, float min, float max)
    {
        float n = i % max;
        if (n < min)
            n = min + i;

        return n;
    }
    #endregion
    #region Boolean
    public static int ToIntBinary(this bool i)
    {
        if (i == true)
            return 1;
        else
            return 0;
    }
    public static int ToIntDirectional(this bool i)
    {
        if (i == true)
            return 1;
        else
            return -1;
    }
    #endregion
    #region Collections
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Vector3 Sum(this IList<Vector3> list)
    {
        Vector3 sum = Vector3.zero;

        foreach (Vector3 vector in list)
            sum += vector;

        return sum;
    }

    public static Quaternion Product(this IList<Quaternion> list)
    {
        Quaternion product = Quaternion.identity;

        foreach (Quaternion q in list)
            product *= q;

        return product;
    }

    public static void Reset<T>(this List<T> list)
    {
        list.Clear();
        list.TrimExcess();
    }
    #endregion
    #region UI
    public static void SetInteractable(this Button button, bool on)
    {
        button.interactable = on;
    }
    public static void SetInteractable(this Toggle toggle, bool on)
    {
        toggle.interactable = on;
    }
    public static void SetIsOn(this Toggle toggle, bool on)
    {
        toggle.isOn = on;
    }

    #region Canvas Group
    public static void SetInteractable(this CanvasGroup group, bool enabled)
    {
        group.interactable = enabled;
        group.blocksRaycasts = enabled;
    }

    public static void SetVisibility(this CanvasGroup group, bool enabled)
    {
        if (enabled)
            group.gameObject.SetActive(true);

        group.alpha = enabled ? 1f : 0f;
        group.interactable = enabled;
        group.blocksRaycasts = enabled;
    }

    public static void Fade(this CanvasGroup canvasGroup, bool enabled, float duration = .5f, bool animateScale = false, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null)
    {
        canvasGroup.gameObject.SetActive(true);

        if (!enabled)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        coroutineHolder.RegisterCoroutine(canvasGroup, FadeCanvasGroupOverTime(canvasGroup, enabled, duration, animateScale, destroyOnFadeOut, disableOnFadeOut, completeCallback));
    }
    public static void Fade(this CanvasGroup canvasGroup, bool enabled, UnityEngine.Object customKey, float duration = .5f, bool animateScale = false, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null)
    {
        canvasGroup.gameObject.SetActive(true);

        if (!enabled)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (customKey == null)
            coroutineHolder.StartCoroutine(FadeCanvasGroupOverTime(canvasGroup, enabled, duration, animateScale, destroyOnFadeOut, disableOnFadeOut, completeCallback));
        else
            coroutineHolder.RegisterCoroutine(customKey, FadeCanvasGroupOverTime(canvasGroup, enabled, duration, animateScale, destroyOnFadeOut, disableOnFadeOut, completeCallback));
    }
    public static IEnumerator FadeOverTime(this CanvasGroup canvasGroup, bool enabled, float duration = .5f, bool animateScale = false, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null)
    {
        yield return coroutineHolder.StartCoroutine(FadeCanvasGroupOverTime(canvasGroup, enabled, duration, animateScale, destroyOnFadeOut, disableOnFadeOut, completeCallback));
    }
    private static IEnumerator FadeCanvasGroupOverTime(CanvasGroup canvasGroup, bool enabled, float duration = .5f, bool animateScale = false, bool disableOnFadeOut = false, bool destroyOnFadeOut = false, Action completeCallback = null)
    {
        if (enabled)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        if (animateScale)
            canvasGroup.transform.localScale = Vector3.one * (enabled ? 1.05f : 1f);

        float time = 0f;
        while (time < 1f)
        {
            if (canvasGroup == null)
                yield break;

            time += Time.deltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, enabled ? 1f : 0f, time);

            if (animateScale)
                canvasGroup.transform.localScale = Vector3.one * Mathf.Lerp(canvasGroup.transform.localScale.x, animateScale ? 1f : 1.05f, time);

            yield return null;
        }

        if (!enabled && disableOnFadeOut && canvasGroup != null && canvasGroup.gameObject != null)
            canvasGroup.gameObject.SetActive(false);

        if (!enabled && destroyOnFadeOut)
            UnityEngine.Object.Destroy(canvasGroup.gameObject);

        completeCallback?.Invoke();
    }


    #endregion
    #region Text
    public static string ToCreditsDisplay(this int valueIn, bool isCurrencyDisplay = false)
    {
        return isCurrencyDisplay ? valueIn.ToCurrency() : valueIn.ToString();
    }

    public static string ToCurrency(this int valueToConvert, bool removeZeroCents = false, bool hideDollarSign = false)
    {
        double tempDouble = valueToConvert * 0.01d;
        string tempString;
        if (hideDollarSign)
            tempString = string.Format("{0:N}", tempDouble); // Does not have Currency Symbol
        else
            tempString = string.Format("{0:C}", tempDouble); // Has Currency Symbol

        if (removeZeroCents && tempString.Substring(tempString.Length - 2, 2) == "00")
            tempString = tempString.Substring(0, tempString.Length - 3);

        return tempString;
    }

    public static void Roll(this TMP_Text text, int valueIn, int valueOut, float duration, bool isCurrency = false)
    {
        coroutineHolder.StartCoroutine(RollToValueOverTime(text, valueIn, valueOut, duration, isCurrency));
    }
    public static IEnumerator RollOverTime(this TMP_Text text, int valueIn, int valueOut, float duration, bool isCurrency = false)
    {
        yield return coroutineHolder.StartCoroutine(RollToValueOverTime(text, valueIn, valueOut, duration, isCurrency));
    }
    private static IEnumerator RollToValueOverTime(TMP_Text text, int valueIn, int valueOut, float duration, bool isCurrency = false)
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime / duration;
            int newValue = Mathf.FloorToInt(Mathf.Lerp(valueIn, valueOut, time));
            text.text = newValue.ToCreditsDisplay(isCurrency);
            yield return null;
        }
    }
    #endregion
    #endregion
    #region Video Player
    public static void Fade(this VideoPlayer videoPlayer, bool on, float duration, ref IEnumerator coroutine, bool destroyOnFadeOut = false, bool disableOnFadeOut = false)
    {
        videoPlayer.SetActive(true);

        if (coroutine != null)
            coroutineHolder.StopCoroutine(coroutine);

        coroutine = FadeVideoOverTime(videoPlayer, on, duration, destroyOnFadeOut, disableOnFadeOut);
        coroutineHolder.StartCoroutine(coroutine);
    }

    public static void Fade(this VideoPlayer videoPlayer, bool on, float duration, bool destroyOnFadeOut = false, bool disableOnFadeOut = false)
    {
        videoPlayer.gameObject.SetActive(true);
        coroutineHolder.StartCoroutine(FadeVideoOverTime(videoPlayer, on, duration, destroyOnFadeOut, disableOnFadeOut));
    }

    public static IEnumerator FadeVideoOverTime(VideoPlayer videoPlayer, bool on, float duration, bool destroyOnFadeOut = false, bool disableOnFadeOut = false)
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime / duration;
            videoPlayer.targetCameraAlpha = Mathf.Lerp(videoPlayer.targetCameraAlpha, on ? 1f : 0f, time);
            yield return null;
        }

        if (!on && disableOnFadeOut)
            videoPlayer.SetActive(false);
    }

    public static void SetActive(this VideoPlayer videoPlayer, bool enabled)
    {
        videoPlayer.enabled = enabled;
    }
    #endregion
    #region Transform
    public static void SetParentAndReset(this Transform transform, Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    public static void Grow(this RectTransform rectTransform, Vector2 additionalScale, float duration = .5f)
    {
        coroutineHolder.RegisterCoroutine(rectTransform, GrowRectransformOverTime(rectTransform, additionalScale, duration));
    }
    private static IEnumerator GrowRectransformOverTime(RectTransform rectTransform, Vector2 additionalScale, float duration)
    {
        Vector2 newScale = rectTransform.sizeDelta + additionalScale;

        float time = 0f;
        while (time < 1f)
        {
            if (rectTransform == null)
                yield break;

            time += Time.deltaTime / duration;

            rectTransform.sizeDelta = new Vector2(Mathf.Lerp(rectTransform.sizeDelta.x, newScale.x, time), Mathf.Lerp(rectTransform.sizeDelta.y, newScale.y, time));

            yield return null;
        }
    }
    #endregion

    public static Transform EmptyInstantiate(this GameObject obj, Vector3 position, Quaternion rotation)
    {
        Transform t = obj.transform;
        t.position = position;
        t.rotation = rotation;
        return t;
    }

    public static void PrepareFillImage (this Image image, int fillOrigin)
    {
        image.fillAmount = 0;
        image.fillOrigin = fillOrigin;
    }

    public static void PrepareFillImage(this Image image, bool clockwise)
    {
        image.fillAmount = 0;
        image.fillClockwise = clockwise;
    }

    public static Vector3Int ToTilePosition(this Vector3 vector)
    {
        return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), 0);
    }
    public static Vector3Int ToTilePosition(this Vector2 vector)
    {
        return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), 0);
    }
    public static Vector2Int ToTilePosition2D(this Vector3 vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }
    public static Vector2Int ToTilePosition2D(this Vector2 vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }
    public static Vector2Int ToTilePosition2D(this Vector3Int vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }
    public static Vector2Int ToChunk(this Vector2Int vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x / 16f), Mathf.FloorToInt(vector.y / 16f));
    }
    public static Vector2Int ToChunk(this Vector3 vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x / 16f), Mathf.FloorToInt(vector.y / 16f));
    }
    public static Vector3Int ToTilePosition(this Vector2Int vector)
    {
        return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), 0);
    }
    public static Vector3Int RoundToInt(this Vector3 vector)
    {
        return new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }
    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y);
    }
    public static Vector3 ToVector3(this Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y);
    }
    public static Vector3Int_Serializable[] ToSerializable(this Vector3Int[] array)
    {
        Vector3Int_Serializable[] newArray = new Vector3Int_Serializable[array.Length];

        for (int i = 0; i < array.Length; i++)
            newArray[i] = new Vector3Int_Serializable(array[i]);

        return newArray;
    }
    public static Vector3Int_Serializable[] ToSerializable(this Vector2Int[] array)
    {
        Vector3Int_Serializable[] newArray = new Vector3Int_Serializable[array.Length];

        for (int i = 0; i < array.Length; i++)
            newArray[i] = new Vector3Int_Serializable(array[i]);

        return newArray;
    }


    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static bool ToBool(this float value)
    {
        return value >= 0;
    }

    public static string ToFileFormat(this string s)
    {
        return s.Replace(' ', '_').ToLower();
    }


}
