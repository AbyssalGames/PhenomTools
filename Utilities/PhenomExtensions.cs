using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = System.Random;

namespace PhenomTools.Utility
{
    public static partial class PhenomExtensions
    {
        private static readonly Random rng = new Random();

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
        
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength); 
        }

        /// <summary>
        /// Outputs numbers with commas, and uses abbreviation for up to 3 sig figs above 9,999 (ex 909, 9,009, 11.1k, 1.22m, etc)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TryFormatAsVeryLargeNumber(this byte num) => TryFormatAsVeryLargeNumber((ulong)num);
        public static string TryFormatAsVeryLargeNumber(this int num) => TryFormatAsVeryLargeNumber((ulong)num);
        public static string TryFormatAsVeryLargeNumber(this uint num) => TryFormatAsVeryLargeNumber((ulong)num);
        public static string TryFormatAsVeryLargeNumber(this ulong num)
        {
          // Ensure number has max 3 significant digits (no rounding up can happen)
          ulong i = (ulong)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
          ulong originalValue = num;
          num = num / i * i;

          if (num >= 1_000_000_000_000)
            return $"{num / 1_000_000_000_000D:0.##}T";
          if (num >= 1_000_000_000)
            return $"{num / 1_000_000_000D:0.##}B";
          if(num >= 1_000_000)
            return $"{num / 1_000_000D:0.##}M";
          if (num >= 10_000)
            return $"{num / 1_000D:0.##}K";

          return originalValue.ToString("#,0");
        }
        /// <summary>
        /// Outputs smaller amount of chars, suitible for smaller UI
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TryFormatIntegerMaxThreeDigits(this byte num) => TryFormatIntegerMaxThreeDigits((ulong)num);
        public static string TryFormatIntegerMaxThreeDigits(this int num) => TryFormatIntegerMaxThreeDigits((ulong)num);
        public static string TryFormatIntegerMaxThreeDigits(this uint num) => TryFormatIntegerMaxThreeDigits((ulong)num);
        public static string TryFormatIntegerMaxThreeDigits(ulong num)
        {
          if (num < 1_000)
            return num.ToString();
          if (num < 1_000_000)
            return $"{num / 1_000}K";
          if (num < 1_000_000_000)
            return $"{num / 1_000_000}M";
          if (num < 1_000_000_000_000)
            return $"{num / 1_000_000_000}B";
          if (num < 1_000_000_000_000_000)
            return $"{num / 1_000_000_000_000}T";

          return num.ToString();
        }

        public static string ToReadableString(this TimeSpan span)
        {
            return string.Join(", ", span.GetReadableStringElements()
                .Where(str => !string.IsNullOrWhiteSpace(str)));
        }

        private static IEnumerable<string> GetReadableStringElements(this TimeSpan span)
        {
            yield return GetDaysString((int)Math.Floor(span.TotalDays));
            yield return GetHoursString(span.Hours);
            yield return GetMinutesString(span.Minutes);
            yield return GetSecondsString(span.Seconds);
        }

        private static string GetDaysString(int days)
        {
            if (days == 0)
                return string.Empty;

            if (days == 1)
                return "1 day";

            return string.Format("{0:0} days", days);
        }

        private static string GetHoursString(int hours)
        {
            if (hours == 0)
                return string.Empty;

            if (hours == 1)
                return "1 hour";

            return string.Format("{0:0} hours", hours);
        }

        private static string GetMinutesString(int minutes)
        {
            if (minutes == 0)
                return string.Empty;

            if (minutes == 1)
                return "1 minute";

            return string.Format("{0:0} minutes", minutes);
        }

        private static string GetSecondsString(int seconds)
        {
            if (seconds == 0)
                return string.Empty;

            if (seconds == 1)
                return "1 second";

            return string.Format("{0:0} seconds", seconds);
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
        #endregion

        #region GameObject
        public static Transform EmptyInstantiate(this GameObject obj, Vector3 position, Quaternion rotation)
        {
            Transform t = obj.transform;
            t.SetPositionAndRotation(position, rotation);
            return t;
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
                body.linearVelocity = Vector3.zero;

            float gravity = -Physics.gravity.y - additionalGravity;
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
        public static bool AddUnique<T>(this IList<T> list, T element)
        {
          if (list.Contains(element)) 
            return false;
          
          list.Add(element);
          return true;
        }

        public static bool AnyOut<T>(this IEnumerable<T> list, Func<T, bool> predicate, out T obj)
        {
          IEnumerable<T> enumerable = list as T[] ?? list.ToArray();
          if (enumerable.Any(predicate))
            {
                obj = enumerable.First(predicate);
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