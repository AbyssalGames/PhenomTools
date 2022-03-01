using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

namespace PhenomTools
{
    public enum CardinalDirection
    {
        North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
    }

    public static class PhenomUtils
    {
        #region Misc
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

        #region Coroutines
        public static IEnumerator DelayActionByTime(float time, Action callback, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            IEnumerator routine;

            if (!Application.isPlaying && Application.isEditor)
            {
                routine = DelayActionByTimeEditorCoroutine(time, callback);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
            }
            else 
            {
                if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    routine = DelayActionByTimeUnscaledCoroutine(time, callback);
                else
                    routine = DelayActionByTimeNormalCoroutine(time, callback);

                CoroutineHolder.StartCoroutine(routine);
            }
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
        private static IEnumerator DelayActionByTimeEditorCoroutine(float time, Action callback)
        {
            yield return new EditorWaitForSeconds(time);
            callback();
        }

        public static IEnumerator DelayActionByFrames(int frames, Action callback, bool fixedUpdate = false)
        {
            IEnumerator routine;

            if (!Application.isPlaying && Application.isEditor)
            {
                routine = DelayActionByFramesCoroutine(frames, callback);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
            }
            else
            {
                routine = DelayActionByFramesCoroutine(frames, callback, fixedUpdate);
                CoroutineHolder.StartCoroutine(routine);
            }
            return routine;
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

        public static IEnumerator RepeatActionByTime(float timeBetween, Action onRepeat, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            IEnumerator routine;

            if (!Application.isPlaying && Application.isEditor)
            {
                routine = RepeatActionByTimeEditorCoroutine(timeBetween, onRepeat);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
            }
            else
            {
                if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    routine = RepeatActionByTimeUnscaledCoroutine(timeBetween, onRepeat);
                else
                    routine = RepeatActionByTimeCoroutine(timeBetween, onRepeat);

                CoroutineHolder.StartCoroutine(routine);
            }

            return routine;
        }
        public static IEnumerator RepeatActionByTime(float timeBetween, int timesToRepeat, Action onRepeat, Action onComplete, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            IEnumerator routine;

            if (!Application.isPlaying && Application.isEditor)
            {
                routine = RepeatActionByTimeEditorCoroutine(timeBetween, timesToRepeat, onRepeat, onComplete);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
            }
            else
            {
                if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    routine = RepeatActionByTimeUnscaledCoroutine(timeBetween, timesToRepeat, onRepeat, onComplete);
                else
                    routine = RepeatActionByTimeCoroutine(timeBetween, timesToRepeat, onRepeat, onComplete);

                CoroutineHolder.StartCoroutine(routine);
            }

            return routine;
        }
        private static IEnumerator RepeatActionByTimeCoroutine(float timeBetween, Action onRepeat)
        {
            WaitForSeconds waitDuration = new WaitForSeconds(timeBetween);
            while (true)
            {
                onRepeat();
                yield return waitDuration;
            }
        }
        private static IEnumerator RepeatActionByTimeCoroutine(float timeBetween, int timesToRepeat, Action onRepeat, Action onComplete)
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
        private static IEnumerator RepeatActionByTimeUnscaledCoroutine(float timeBetween, Action onRepeat)
        {
            WaitForSecondsRealtime waitDuration = new WaitForSecondsRealtime(timeBetween);
            while (true)
            {
                onRepeat();
                yield return waitDuration;
            }
        }
        private static IEnumerator RepeatActionByTimeUnscaledCoroutine(float timeBetween, int timesToRepeat, Action onRepeat, Action onComplete)
        {
            WaitForSecondsRealtime waitDuration = new WaitForSecondsRealtime(timeBetween);
            while (timesToRepeat > 0)
            {
                onRepeat();
                timesToRepeat--;
                yield return waitDuration;
            }

            onComplete();
        }
        private static IEnumerator RepeatActionByTimeEditorCoroutine(float timeBetween, Action onRepeat)
        {
            EditorWaitForSeconds waitDuration = new EditorWaitForSeconds(timeBetween);
            while (true)
            {
                onRepeat();
                yield return waitDuration;
            }
        }
        private static IEnumerator RepeatActionByTimeEditorCoroutine(float timeBetween, int timesToRepeat, Action onRepeat, Action onComplete)
        {
            EditorWaitForSeconds waitDuration = new EditorWaitForSeconds(timeBetween);
            while (timesToRepeat > 0)
            {
                onRepeat();
                timesToRepeat--;
                yield return waitDuration;
            }

            onComplete();
        }

        public static IEnumerator RepeatActionByFrames(int framesBetween, Action onRepeat, bool fixedUpdate = false)
        {
            IEnumerator routine;

            if (!Application.isPlaying && Application.isEditor)
            {
                routine = RepeatActionByFramesCoroutine(framesBetween, onRepeat);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
            }
            else
            {
                routine = RepeatActionByFramesCoroutine(framesBetween, onRepeat, fixedUpdate);
                CoroutineHolder.StartCoroutine(routine);
            }
            return routine;
        }

        public static IEnumerator RepeatActionByFrames(int framesBetween, int timesToRepeat, Action onRepeat, Action onComplete, bool fixedUpdate = false)
        {
            IEnumerator routine;

            if (!Application.isPlaying && Application.isEditor)
            {
                routine = RepeatActionByFramesCoroutine(framesBetween, timesToRepeat, onRepeat, onComplete);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
            }
            else
            {
                routine = RepeatActionByFramesCoroutine(framesBetween, timesToRepeat, onRepeat, onComplete, fixedUpdate);
                CoroutineHolder.StartCoroutine(routine);
            }
            return routine;
        }
        private static IEnumerator RepeatActionByFramesCoroutine(int framesBetween, Action onRepeat, bool fixedUpdate = false)
        {
            while (true)
            {
                int count = 0;

                while (count < framesBetween)
                {
                    if (fixedUpdate)
                        yield return new WaitForFixedUpdate();
                    else
                        yield return null;

                    count++;
                }

                onRepeat();
            }
        }
        private static IEnumerator RepeatActionByFramesCoroutine(int framesBetween, int timesToRepeat, Action onRepeat, Action onComplete, bool fixedUpdate = false)
        {
            while (timesToRepeat > 0)
            {
                int count = 0;

                while (count < framesBetween)
                {
                    if (fixedUpdate)
                        yield return new WaitForFixedUpdate();
                    else
                        yield return null;

                    count++;
                }

                onRepeat();
                timesToRepeat--;
            }

            onComplete();
        }

        /// <summary>
        /// Can only be used to stop coroutines that are managed by the CoroutineHolder or were started with a PhenomTools extension/utility method.
        /// </summary>
        public static void Stop(this IEnumerator enumerator)
        {
            CoroutineHolder.StopCoroutine(enumerator);
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