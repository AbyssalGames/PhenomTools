using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using Random = System.Random;

namespace PhenomTools
{
    public static partial class PhenomExtensions
    {
        private static readonly Random rng = new Random();

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

        #region Boolean

        public static int ToIntBinary(this bool i)
        {
            return i == true ? 1 : 0;
        }

        public static int[] ToIntBinary(this bool[] boolArray)
        {
            int[] intArray = new int[boolArray.Length];

            for (int i = 0; i < boolArray.Length; i++)
            {
                intArray[i] = boolArray[i].ToIntBinary();
            }

            return intArray;
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

        public static string ToFixedLengthString(this int i, int length)
        {
            int iLength = i.ToString().Length;

            if (iLength < length)
            {
                string newString = "";
                for (int n = 0; n < length - iLength; n++)
                {
                    string.Concat(newString, "0");
                }

                string.Concat(newString, i.ToString());

                return newString;
            }
            else if (iLength > length)
            {
                string s = (i / Mathf.Pow(10, iLength)).ToString("F" + length.ToString());
                return s.Remove(0, 2); // remove the 0 and decimal point
            }
            else
            {
                return i.ToString();
            }
        }

        public static string ToBigNumberString(this int num) => ToBigNumberString((ulong)num);
        public static string ToBigNumberString(this uint num) => ToBigNumberString((ulong)num);
        public static string ToBigNumberString(this ulong num)
        {
            // Ensure number has max 3 significant digits (no rounding up can happen)
            ulong i = (ulong)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;

            if (num >= 1000000000000)
                return (num / 1000000000000D).ToString("0.##") + "T";
            else if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + "B";
            else if(num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "M";
            else if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "K";

            return num.ToString("#,0");
        }

        //public static string ToBigNumberString(this ulong i)
        //{
        //    char[] suffixes = new char[] { 'K', 'M', 'B', 'T', 'Q' };

        //    string fullString = i.ToString();
        //    int length = fullString.Length;

        //    if(length > 4)
        //    {
        //        if(length < 7)
        //            return fullString.Substring(0, )
        //    }
        //    else
        //    {
        //        return i.ToString("N"/*, System.Globalization.CultureInfo.InvariantCulture*/);
        //    }
        //}

        //public static string ToFixedLengthString(this int i, int length)
        //{
        //    int currentCount = i.ToString().Length;
        //    string text = "";

        //    if (currentCount < length)
        //    {
        //        int dif = length - currentCount;


        //        for (int x = 0; x < dif; x++)
        //        {
        //            text = string.Concat(text, "0");
        //        }

        //        text = string.Concat(text, i.ToString());
        //    }

        //    return text;
        //}

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

        public static Vector3 ToVector3(this Vector3Int vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
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

        public static Vector3Int[] ToUnserializable(this Vector3Int_Serializable[] array)
        {
            Vector3Int[] newArray = new Vector3Int[array.Length];

            for (int i = 0; i < array.Length; i++)
                newArray[i] = array[i].ToUnserializable();

            return newArray;
        }

        public static Vector2Int[] ToUnserializable(this Vector2Int_Serializable[] array)
        {
            Vector2Int[] newArray = new Vector2Int[array.Length];

            for (int i = 0; i < array.Length; i++)
                newArray[i] = array[i].ToUnserializable();

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

        public static void DestroyDelayed(this MonoBehaviour _, UnityEngine.Object obj, float time)
        {
            obj.DestroyDelayed(time);
        }

        public static void DestroyDelayed(this UnityEngine.Object obj, float time)
        {
            PhenomUtils.DelayActionByTime(time, () => UnityEngine.Object.Destroy(obj));
        }

        public static bool ContainsLayer(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static int ToLayer(this LayerMask layerMask)
        {
            return (int)Mathf.Log(layerMask.value, 2);
        }
        #endregion

        #region Transform
        public static void SetParentAndReset(this Transform transform, Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }

        public static Transform[] GetChildren(this Transform transform)
        {
            Transform[] array = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
                array[i] = transform.GetChild(i);

            return array;
        }

        public static void RotateYTowards(this Transform transform, Vector3 target, float speed)
        {
            Vector3 direction = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        }
        #endregion

        #region Animation
        public static bool HasParameter(this Animator anim, string parameterName)
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == parameterName) 
                    return true;
            }
            return false;
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

        public static void Fade(this VideoPlayer videoPlayer, bool on, float duration, ref IEnumerator coroutine,
            bool destroyOnFadeOut = false, bool disableOnFadeOut = false)
        {
            videoPlayer.SetActive(true);

            if (coroutine != null)
                CoroutineHolder.StopCoroutine(coroutine);

            coroutine = FadeVideoOverTime(videoPlayer, on, duration, destroyOnFadeOut, disableOnFadeOut);
            CoroutineHolder.StartCoroutine(coroutine);
        }

        public static void Fade(this VideoPlayer videoPlayer, bool on, float duration, bool destroyOnFadeOut = false,
            bool disableOnFadeOut = false)
        {
            videoPlayer.gameObject.SetActive(true);
            CoroutineHolder.StartCoroutine(FadeVideoOverTime(videoPlayer, on, duration, destroyOnFadeOut,
                disableOnFadeOut));
        }

        public static IEnumerator FadeVideoOverTime(VideoPlayer videoPlayer, bool on, float duration,
            bool destroyOnFadeOut = false, bool disableOnFadeOut = false)
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

        #region Misc
        public static int ToIndex(this DeviceOrientation orientation, bool includeFaceUp = false, bool includeFaceDown = false)
        {
            switch (orientation)
            {
                default:
                case DeviceOrientation.Unknown:
                case DeviceOrientation.Portrait:
                    return 0;
                case DeviceOrientation.LandscapeLeft:
                    return 1;
                case DeviceOrientation.PortraitUpsideDown:
                    return 2;
                case DeviceOrientation.LandscapeRight:
                    return 3;
                case DeviceOrientation.FaceUp:
                    return includeFaceUp ? 4 : 0;
                case DeviceOrientation.FaceDown:
                    return includeFaceDown ? 5 : 0;
            }
        }

        public static Vector2 ToVector(this CardinalDirection cardinalDirection, bool normalized = false)
        {
            switch (cardinalDirection)
            {
                default:
                case CardinalDirection.North:
                    return Vector2.up;
                case CardinalDirection.NorthEast:
                    return normalized ? Vector2.one.normalized : Vector2.one;
                case CardinalDirection.East:
                    return Vector2.right;
                case CardinalDirection.SouthEast:
                    return normalized ? new Vector2(1, -1).normalized : new Vector2(1, -1);
                case CardinalDirection.South:
                    return Vector2.down;
                case CardinalDirection.SouthWest:
                    return normalized ? -Vector2.one.normalized : -Vector2.one;
                case CardinalDirection.West:
                    return Vector2.left;
                case CardinalDirection.NorthWest:
                    return normalized ? new Vector2(-1, 1).normalized : new Vector2(-1, 1);
            }
        }

        public static int ToQuadrant(this CardinalDirection cardinalDirection)
        {
            switch (cardinalDirection)
            {
                default:
                case CardinalDirection.North:
                case CardinalDirection.NorthEast:
                    return 0;
                case CardinalDirection.East:
                case CardinalDirection.SouthEast:
                    return 1;
                case CardinalDirection.South:
                case CardinalDirection.SouthWest:
                    return 2;
                case CardinalDirection.West:
                case CardinalDirection.NorthWest:
                    return 3;
            }
        }
        #endregion
    }
}