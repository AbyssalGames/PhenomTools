using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            return i ? 1 : 0;
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
            if (i)
                return 1;

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
                    newString = string.Concat(newString, "0");
                }

                newString = string.Concat(newString, i.ToString());

                return newString;
            }

            if (iLength <= length) return i.ToString();
            //string s = (i / Mathf.Pow(10, iLength)).ToString("F" + length.ToString());
            return (i / Mathf.Pow(10, iLength)).ToString("F" + length).Remove(0, 2); // remove the 0 and decimal point
        }

        public static string ToBigNumberString(this byte num) => ToBigNumberString((ulong)num);
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

        public static string ToNonCamelCase(this string text)
        {
            return Regex.Replace(
                Regex.Replace(
                    text,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );

            //return Regex.Replace(text, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1");
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

        #region GameObject
        public static Transform EmptyInstantiate(this GameObject obj, Vector3 position, Quaternion rotation)
        {
            Transform t = obj.transform;
            t.SetPositionAndRotation(position, rotation);
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
            transform.localScale = Vector3.one;
        }

        public static void RotateYTowards(this Transform transform, Vector3 target, float speed)
        {
            Vector3 direction = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        }

        public static Transform[] GetChildren(this Transform transform)
        {
            Transform[] array = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
                array[i] = transform.GetChild(i);

            return array;
        }

        public static Transform[] GetChildrenRecursive(this Transform parent)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform t in parent)
            {
                children.Add(t);

                Transform[] second = t.GetChildrenRecursive();

                if (second.Length > 0)
                    children.AddRange(second);
            }

            return children.ToArray();
        }

        public static Transform FindDeepChild(this Transform parent, string childName)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(parent);

            while (queue.Count > 0)
            {
                Transform c = queue.Dequeue();
                if (c.name == childName)
                    return c;
                foreach (Transform t in c)
                    queue.Enqueue(t);
            }
            return null;
        }
        #endregion

        #region Rigidbody
        public static void Cannonball(this Rigidbody body, Vector3 targetPoint, float initialAngle, bool inheritVelocity = false, float additionalGravity = 0)
        {
            if (!inheritVelocity)
                body.velocity = Vector3.zero;

            float gravity = -Physics.gravity.y - additionalGravity;
            //Debug.Log(gravity);
            // Selected angle in radians
            float angle = initialAngle * Mathf.Deg2Rad;

            // Positions of this object and the target on the same plane
            Vector3 planarTarget = new Vector3(targetPoint.x, 0, targetPoint.z);
            Vector3 planarPostion = new Vector3(body.position.x, 0, body.position.z);

            // Planar distance between objects
            float distance = Vector3.Distance(planarTarget, planarPostion);
            // Distance along the y axis between objects
            float yOffset = body.position.y - targetPoint.y;

            float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

            Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

            // Rotate our velocity to match the direction between the two objects
            float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
            Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
            //Debug.Log(finalVelocity + ", " + finalVelocity.magnitude);
            // Fire!
            //rigid.velocity = finalVelocity;
            // Alternative way:
            body.AddForce(finalVelocity, ForceMode.VelocityChange);
        }
        #endregion

        #region Animation
        public static bool HasParameter(this Animator anim, string parameterName)
        {
            return anim.parameters.Any(param => param.name == parameterName);
        }
        #endregion

        #region Collections
        public static void AddUnique<T>(this IList<T> list, T element)
        {
            if (!list.Contains(element))
                list.Add(element);
        }
        public static void AddUnique<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }

        public static bool AnyOut<T>(this IEnumerable<T> list, Func<T, bool> predicate, out T obj)
        {
            if (list.Any(predicate))
            {
                obj = list.First(predicate);
                return true;
            }

            obj = default;
            return false;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static Vector3 Sum(this IList<Vector3> list)
        {
            return list.Aggregate(Vector3.zero, (current, vector) => current + vector);
        }

        public static Quaternion Product(this IList<Quaternion> list)
        {
            return list.Aggregate(Quaternion.identity, (current, q) => current * q);
        }

        public static void Reset<T>(this List<T> list)
        {
            list.Clear();
            list.TrimExcess();
        }

        #endregion

        #region Coroutines
        /// <summary>
        /// Starts this Coroutine owned by the CoroutineHolder
        /// </summary>
        public static Coroutine Start(this IEnumerator enumerator)
        {
            return CoroutineHolder.StartCoroutine(enumerator);
        }

        /// <summary>
        /// Can only be used to stop coroutines that are managed by the CoroutineHolder or were started with a PhenomTools extension/utility method.
        /// </summary>
        public static void Stop(this IEnumerator enumerator)
        {
            CoroutineHolder.StopCoroutine(enumerator);
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
        public static byte[] ToBytes(this bool[] bools)
        {
            return ToBytes(new BitArray(bools));
        }
        public static byte[] ToBytes(this BitArray bits)
        {
            byte[] bytes = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(bytes, 0);

            return bytes;
        }

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