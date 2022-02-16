using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace PhenomTools
{
    public enum CardinalDirection
    {
        North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
    }


    public static class PhenomUtils
    {
        public static string GetCurrentPlatformString()
        {
            return GetPlatformString(Application.platform);
        }
        public static string GetPlatformString(RuntimePlatform platform)
        {
            switch (platform)
            {
                default:
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
            }
        }

        /// <summary>Gets a string from a text file at the specified path.</summary>
        /// <param name="path">The path to the file.</param>
        public static string GetStringFromFile (string path)
        {
            if (!File.Exists(path))
                throw new System.Exception("File does not exist at specified path");

            StreamReader reader = new StreamReader(path);
            string text = reader.ReadToEnd();
            reader.Close();

            return text;
        }

        public static int GetDirectionOfRelativeObject(Vector3 baseObjectPos, Vector3 baseObjectAxis, Vector3 otherObjectPos)
        {
            Vector3 toOther = otherObjectPos - baseObjectPos;

            if (Vector3.Dot(baseObjectAxis, toOther) > 0)
                return -1;
            else
                return 1;
        }

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

        public static IEnumerator DelayActionByTime(float time, Action callback, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            IEnumerator routine;

            if(updateMode == AnimatorUpdateMode.UnscaledTime)
                routine = DelayActionByTimeUnscaledCoroutine(time, callback);
            else
                routine = DelayActionByTimeNormalCoroutine(time, callback);

            CoroutineHolder.StartCoroutine(routine);
            return routine;
        }
        private static IEnumerator DelayActionByTimeNormalCoroutine(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback();
        }
        private static IEnumerator DelayActionByTimeUnscaledCoroutine(float time, Action callback)
        {
            yield return new WaitForSecondsRealtime(time);
            callback();
        }

        public static void DelayActionByFrames(int frames, Action callback, bool fixedUpdate = false)
        {
            CoroutineHolder.StartCoroutine(DelayActionByFramesCoroutine(frames, callback, fixedUpdate));
        }
        private static IEnumerator DelayActionByFramesCoroutine(int frames, Action callback, bool fixedUpdate = false)
        {
            int count = 0;

            while (count < frames)
            {
                if (fixedUpdate)
                    yield return new WaitForFixedUpdate();
                else
                    yield return null;

                count++;
            }

            callback();
        }

        public static IEnumerator RepeatAction(float timeBetween, Action onRepeat)
        {
            WaitForSeconds waitDuration = new WaitForSeconds(timeBetween);
            while (true)
            {
                onRepeat();
                yield return waitDuration;
            }
        }

        public static IEnumerator RepeatAction(float timeBetween, int timesToRepeat, Action onRepeat, Action onComplete)
        {
            WaitForSeconds waitDuration = new WaitForSeconds(timeBetween);
            while (timesToRepeat > 0)
            {
                onRepeat();
                timesToRepeat--;
                yield return waitDuration;
            }

            onComplete();
        }
    }
}