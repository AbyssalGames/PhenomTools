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
        public Timer(float duration, bool useSeconds = true, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            Begin(duration, useSeconds, updateMode);
        }

        public override void Begin(float duration, bool useSeconds = true, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            currentTime = duration;
            keeperCoroutine = KeeperCoroutine();

            CallOnUpdate();

            base.Begin(duration, useSeconds, updateMode);
        }

        protected override IEnumerator KeeperCoroutine()
        {
            while (isRunning && currentTime > 0)
            {
                if (useSeconds)
                {
                    if(updateMode == AnimatorUpdateMode.Normal || updateMode == AnimatorUpdateMode.AnimatePhysics)
                        yield return new WaitForSeconds(1);
                    else if(updateMode == AnimatorUpdateMode.UnscaledTime)
                        yield return new WaitForSecondsRealtime(1);

                    currentTime = (int)currentTime - 1;

                    CallOnUpdate();
                }
                else
                {
                    if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    {
                        currentTime = startTime + duration - Time.realtimeSinceStartup;
                    }
                    else
                    {
                        if (updateMode == AnimatorUpdateMode.AnimatePhysics)
                            yield return new WaitForFixedUpdate();
                        else
                            yield return new WaitForEndOfFrame();

                        currentTime -= Time.deltaTime;
                    }

                    CallOnUpdate();
                }
            }

            currentTime = 0f;

            keeperCoroutine = null;
            Stop();
        }

        public override void Reset()
        {
            currentTime = duration;

            base.Reset();
        }
    }
}
