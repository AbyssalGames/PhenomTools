using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

namespace PhenomTools
{
    public static partial class PhenomUtils
    {
        public static IEnumerator DelayActionByTime(float time, Action callback, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            IEnumerator routine = null;

            if (!Application.isPlaying && Application.isEditor)
            {
#if UNITY_EDITOR
                routine = DelayActionByTimeEditorCoroutine(time, callback);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
#endif
            }
            else
            {
                if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    routine = DelayActionByTimeUnscaledCoroutine(time, callback);
                else
                    routine = DelayActionByTimeNormalCoroutine(time, callback);

                routine.Start();
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
#if UNITY_EDITOR
        private static IEnumerator DelayActionByTimeEditorCoroutine(float time, Action callback)
        {
            yield return new EditorWaitForSeconds(time);
            callback();
        }
#endif

        public static IEnumerator DelayActionByFrames(int frames, Action callback, bool fixedUpdate = false)
        {
            IEnumerator routine = null;

            if (!Application.isPlaying && Application.isEditor)
            {
#if UNITY_EDITOR
                routine = DelayActionByFramesCoroutine(frames, callback);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
#endif
            }
            else
            {
                routine = DelayActionByFramesCoroutine(frames, callback, fixedUpdate);
                routine.Start();
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

        public static IEnumerator RepeatActionByTime(float timeBetween, Action onRepeat, float? initialDelay = null, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            IEnumerator routine = null;

            if (!Application.isPlaying && Application.isEditor)
            {
#if UNITY_EDITOR
                routine = RepeatActionByTimeEditorCoroutine(timeBetween, onRepeat, initialDelay);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
#endif
            }
            else
            {
                if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    routine = RepeatActionByTimeUnscaledCoroutine(timeBetween, onRepeat, initialDelay);
                else
                    routine = RepeatActionByTimeCoroutine(timeBetween, onRepeat, initialDelay);

                routine.Start();
            }

            return routine;
        }
        public static IEnumerator RepeatActionByTime(float timeBetween, int timesToRepeat, Action onRepeat, Action onComplete, float? initialDelay = null, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            IEnumerator routine = null;

            if (!Application.isPlaying && Application.isEditor)
            {
#if UNITY_EDITOR
                routine = RepeatActionByTimeEditorCoroutine(timeBetween, timesToRepeat, onRepeat, onComplete, initialDelay);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
#endif
            }
            else
            {
                if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    routine = RepeatActionByTimeUnscaledCoroutine(timeBetween, timesToRepeat, onRepeat, onComplete, initialDelay);
                else
                    routine = RepeatActionByTimeCoroutine(timeBetween, timesToRepeat, onRepeat, onComplete, initialDelay);

                routine.Start();
            }

            return routine;
        }
        private static IEnumerator RepeatActionByTimeCoroutine(float timeBetween, Action onRepeat, float? initialDelay)
        {
            if (initialDelay != null)
                yield return new WaitForSeconds(initialDelay.Value);

            WaitForSeconds waitDuration = new WaitForSeconds(timeBetween);
            while (true)
            {
                onRepeat();
                yield return waitDuration;
            }
        }
        private static IEnumerator RepeatActionByTimeCoroutine(float timeBetween, int timesToRepeat, Action onRepeat, Action onComplete, float? initialDelay)
        {
            if (initialDelay != null)
                yield return new WaitForSeconds(initialDelay.Value);

            WaitForSeconds waitDuration = new WaitForSeconds(timeBetween);
            while (timesToRepeat > 0)
            {
                onRepeat();
                timesToRepeat--;
                yield return waitDuration;
            }

            onComplete();
        }
        private static IEnumerator RepeatActionByTimeUnscaledCoroutine(float timeBetween, Action onRepeat, float? initialDelay)
        {
            if (initialDelay != null)
                yield return new WaitForSecondsRealtime(initialDelay.Value);

            WaitForSecondsRealtime waitDuration = new WaitForSecondsRealtime(timeBetween);
            while (true)
            {
                onRepeat();
                yield return waitDuration;
            }
        }
        private static IEnumerator RepeatActionByTimeUnscaledCoroutine(float timeBetween, int timesToRepeat, Action onRepeat, Action onComplete, float? initialDelay)
        {
            if (initialDelay != null)
                yield return new WaitForSecondsRealtime(initialDelay.Value);

            WaitForSecondsRealtime waitDuration = new WaitForSecondsRealtime(timeBetween);
            while (timesToRepeat > 0)
            {
                onRepeat();
                timesToRepeat--;
                yield return waitDuration;
            }

            onComplete();
        }
#if UNITY_EDITOR
        private static IEnumerator RepeatActionByTimeEditorCoroutine(float timeBetween, Action onRepeat, float? initialDelay)
        {
            if (initialDelay != null)
                yield return new EditorWaitForSeconds(initialDelay.Value);

            EditorWaitForSeconds waitDuration = new EditorWaitForSeconds(timeBetween);
            while (true)
            {
                onRepeat();
                yield return waitDuration;
            }
        }
        private static IEnumerator RepeatActionByTimeEditorCoroutine(float timeBetween, int timesToRepeat, Action onRepeat, Action onComplete, float? initialDelay)
        {
            if (initialDelay != null)
                yield return new EditorWaitForSeconds(initialDelay.Value);

            EditorWaitForSeconds waitDuration = new EditorWaitForSeconds(timeBetween);
            while (timesToRepeat > 0)
            {
                onRepeat();
                timesToRepeat--;
                yield return waitDuration;
            }

            onComplete();
        }
#endif

        public static IEnumerator RepeatActionByFrames(int framesBetween, Action onRepeat, bool fixedUpdate = false, int? initialDelay = null)
        {
            IEnumerator routine = null;

            if (!Application.isPlaying && Application.isEditor)
            {
#if UNITY_EDITOR
                routine = RepeatActionByFramesCoroutine(framesBetween, onRepeat, false, initialDelay);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
#endif
            }
            else
            {
                routine = RepeatActionByFramesCoroutine(framesBetween, onRepeat, fixedUpdate, initialDelay);
                routine.Start();
            }
            return routine;
        }

        public static IEnumerator RepeatActionByFrames(int framesBetween, int timesToRepeat, Action onRepeat, Action onComplete, bool fixedUpdate = false, int? initialDelay = null)
        {
            IEnumerator routine = null;

            if (!Application.isPlaying && Application.isEditor)
            {
#if UNITY_EDITOR
                routine = RepeatActionByFramesCoroutine(framesBetween, timesToRepeat, onRepeat, onComplete, false, initialDelay);
                EditorCoroutineUtility.StartCoroutineOwnerless(routine);
#endif
            }
            else
            {
                routine = RepeatActionByFramesCoroutine(framesBetween, timesToRepeat, onRepeat, onComplete, fixedUpdate, initialDelay);
                routine.Start();
            }
            return routine;
        }
        private static IEnumerator RepeatActionByFramesCoroutine(int framesBetween, Action onRepeat, bool fixedUpdate, int? initialDelay)
        {
            if (initialDelay != null)
                yield return DelayActionByFramesCoroutine(initialDelay.Value, onRepeat, fixedUpdate);
            else
                onRepeat();

            while (true)
                yield return DelayActionByFramesCoroutine(framesBetween, onRepeat, fixedUpdate);
        }
        private static IEnumerator RepeatActionByFramesCoroutine(int framesBetween, int timesToRepeat, Action onRepeat, Action onComplete, bool fixedUpdate, int? initialDelay)
        {
            if (initialDelay != null)
                yield return DelayActionByFramesCoroutine(initialDelay.Value, onRepeat, fixedUpdate);
            else
                onRepeat();

            while (timesToRepeat > 1)
            {
                yield return DelayActionByFramesCoroutine(framesBetween, onRepeat, fixedUpdate);
                timesToRepeat--;
            }

            onComplete();
        }
    }
}
