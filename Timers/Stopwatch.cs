using System;
using System.Collections;
using UnityEngine;

namespace PhenomTools
{
    [Serializable]
    public class Stopwatch : TimeKeeper
    {
        public Stopwatch(float duration = 0, bool useSeconds = true, bool removeListenersOnFinished = false, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            Initialize(duration, useSeconds, removeListenersOnFinished, updateMode);
        }

        public override void Begin()
        {
            if (isRunning)
                Stop();

            currentTime = 0;
            keeperCoroutine = KeeperCoroutine();

            CallOnUpdate();
            base.Begin();
        }

        protected override IEnumerator KeeperCoroutine()
        {
            while (isRunning && (duration == 0 || currentTime < duration))
            {
                if (useSeconds)
                {
                    if (updateMode == AnimatorUpdateMode.Normal || updateMode == AnimatorUpdateMode.AnimatePhysics)
                    {
                        yield return waitSecond;
                    }
                    else if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    {
                        yield return waitRealSecond;
                    }

                    currentTime += 1;

                    if (duration > 0)
                        currentTime = Mathf.Clamp(currentTime, 0, duration);

                    CallOnUpdate();
                }
                else
                {
                    if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    {
                        currentTime = Time.realtimeSinceStartup - startTime;
                    }
                    else if (updateMode == AnimatorUpdateMode.AnimatePhysics)
                    {
                        yield return waitFixed;
                        currentTime += Time.fixedDeltaTime;
                    }
                    else
                    { 
                        yield return waitFrame;
                        currentTime += Time.deltaTime;
                    }

                    if (duration > 0)
                        currentTime = Mathf.Clamp(currentTime, 0, duration);

                    CallOnUpdate();
                }
            }

            keeperCoroutine = null;
            Stop();
        }

        public override void DoReset()
        {
            currentTime = 0;

            base.DoReset();
        }
    }
}
