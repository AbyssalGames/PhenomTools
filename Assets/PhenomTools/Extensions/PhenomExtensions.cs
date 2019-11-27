using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using Random = System.Random;

public static partial class PhenomExtensions
{
    private static readonly Random rng = new Random();
    private static readonly TweenParams tweenParams_Sine = new TweenParams().SetEase(Ease.InOutSine);

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

    public static Tweener Roll(this int i, DOGetter<int> getter, DOSetter<int> setter, int endValue, float duration)
    {
        DOTween.Kill(i);
        return DOTween.To(getter, setter, endValue, duration).SetAs(tweenParams_Sine);
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
    public static bool ToBool(this float value)
    {
        return value >= 0;
    }

    public static float Loop(this float i, float min, float max)
    {
        float n = i % max;
        if (n < min)
            n = min + i;

        return n;
    }

    public static Tweener Roll(this float i, DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
    {
        DOTween.Kill(i);
        return DOTween.To(getter, setter, endValue, duration).SetAs(tweenParams_Sine);
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

    #region String
    public static string ToFileFormat(this string s)
    {
        return s.Replace(' ', '_').ToLower();
    }
    #endregion

    #region Vector
    public static Vector3Int RoundToInt(this Vector3 vector)
    {
        return new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }
    public static Vector3Int ToVector3Int(this Vector2Int vector)
    {
        return new Vector3Int(vector.x, vector.y, 0);
    }
    public static Vector2Int ToVector2Int(this Vector3Int vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }
    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y);
    }
    public static Vector3 ToVector3(this Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y);
    }
    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3Int_Serializable[] ToSerializable(this Vector3Int[] array)
    {
        Vector3Int_Serializable[] newArray = new Vector3Int_Serializable[array.Length];

        for (int i = 0; i < array.Length; i++)
            newArray[i] = new Vector3Int_Serializable(array[i]);

        return newArray;
    }
    public static Vector2Int_Serializable[] ToSerializable(this Vector2Int[] array)
    {
        Vector2Int_Serializable[] newArray = new Vector2Int_Serializable[array.Length];

        for (int i = 0; i < array.Length; i++)
            newArray[i] = new Vector2Int_Serializable(array[i]);

        return newArray;
    }
    #endregion

    #region GameObject
    public static Transform EmptyInstantiate(this GameObject obj, Vector3 position, Quaternion rotation)
    {
        Transform t = obj.transform;
        t.position = position;
        t.rotation = rotation;
        return t;
    }
    #endregion

    #region Transform
    public static void SetParentAndReset(this Transform transform, Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
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
}
