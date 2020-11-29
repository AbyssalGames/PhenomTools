using System;
using System.Collections;
using UnityEngine;

namespace PhenomTools
{
    /// <summary>
    /// Counts down from given duration. Calls onComplete when it reaches 0.
    /// </summary>
    [Serializable]
    public class Timer : TimeKeeperBase
    {
        public Timer(float duration, bool useSeconds = true, bool removeListenersOnFinished = false, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            Initialize(duration, useSeconds, removeListenersOnFinished, updateMode);
        }

        public override void Begin()
        {
            if (isRunning)
                Stop();

            currentTime = duration;
            keeperCoroutine = KeeperCoroutine();

            CallOnUpdate();

            base.Begin();
        }

        protected override IEnumerator KeeperCoroutine()
        {
            while (isRunning && currentTime > 0)
            {
                if (useSeconds)
                {
                    if(updateMode == AnimatorUpdateMode.Normal || updateMode == AnimatorUpdateMode.AnimatePhysics)
                        yield return waitSecond;
                    else if(updateMode == AnimatorUpdateMode.UnscaledTime)
                        yield return waitRealSecond;

                    currentTime = Mathf.Clamp((int)currentTime - 1, 0, duration);

                    CallOnUpdate();
                }
                else
                {
                    if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    {
                        currentTime = Mathf.Clamp(startTime + duration - Time.realtimeSinceStartup, 0, duration);
                    }
                    else if (updateMode == AnimatorUpdateMode.AnimatePhysics)
                    {
                        yield return waitFixed;
                        currentTime = Mathf.Clamp(currentTime - Time.fixedDeltaTime, 0, duration);
                    }
                    else
                    { 
                        yield return waitFrame;
                        currentTime = Mathf.Clamp(currentTime - Time.deltaTime, 0, duration);
                    }

                    CallOnUpdate();
                }
            }

            keeperCoroutine = null;
            Stop();
        }

        public override void DoReset()
        {
            currentTime = duration;

            base.DoReset();
        }
    }
}
