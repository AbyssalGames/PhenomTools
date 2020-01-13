using System;
using System.Collections;
using UnityEngine;

namespace PhenomTools
{
    [Serializable]
    public class Stopwatch : TimeKeeperBase
    {
        public Stopwatch(float duration = 0, bool useSeconds = true, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            Begin(duration, useSeconds, updateMode);
        }

        public override void Begin(float duration = 0, bool useSeconds = true, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            currentTime = 0;
            keeperCoroutine = KeeperCoroutine();

            CallOnUpdate();

            base.Begin(duration, useSeconds, updateMode);
        }

        protected override IEnumerator KeeperCoroutine()
        {
            while (isRunning && (duration == 0 || currentTime < duration))
            {
                if (useSeconds)
                {
                    if (updateMode == AnimatorUpdateMode.Normal || updateMode == AnimatorUpdateMode.AnimatePhysics)
                        yield return new WaitForSeconds(1);
                    else if (updateMode == AnimatorUpdateMode.UnscaledTime)
                        yield return new WaitForSecondsRealtime(1);

                    currentTime = (int)currentTime + 1;

                    CallOnUpdate();
                }
                else
                {
                    if (updateMode == AnimatorUpdateMode.UnscaledTime)
                    {
                        currentTime = Time.realtimeSinceStartup - startTime;
                    }
                    else
                    {
                        if (updateMode == AnimatorUpdateMode.AnimatePhysics)
                            yield return new WaitForFixedUpdate();
                        else
                            yield return new WaitForEndOfFrame();

                        currentTime += Time.deltaTime;
                    }

                    CallOnUpdate();
                }
            }

            keeperCoroutine = null;
            Stop();
        }

        public override void Reset()
        {
            currentTime = 0;

            base.Reset();
        }
    }
}
