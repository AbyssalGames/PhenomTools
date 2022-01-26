using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PhenomTools
{
    [Serializable]
    public class TimeKeeper
    {
        public float duration = 30f;

        /// <summary>
        /// Optimize to 1 per second vs frames per second
        /// </summary>
        public bool useSeconds = true;
        public bool removeListenersOnFinished;
        public AnimatorUpdateMode updateMode;

        public UnityEventFloat onUpdate = new UnityEventFloat();
        public UnityEvent onComplete = new UnityEvent();

        public bool isRunning { get; protected set; }
        public float currentTime { get; protected set; }
        public float startTime { get; protected set; }

        public IEnumerator keeperCoroutine;

        public static WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        public static WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();
        public static WaitForSeconds waitSecond = new WaitForSeconds(1f);
        public static WaitForSecondsRealtime waitRealSecond = new WaitForSecondsRealtime(1f);

        public virtual void Initialize(float duration, bool useSeconds = true, bool removeListenersOnFinished = false, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            this.useSeconds = useSeconds;
            this.duration = duration;
            this.updateMode = updateMode;
        }

        public virtual void Begin()
        { 
            startTime = Time.realtimeSinceStartup;
            isRunning = true;

            TimerManager.RegisterNewTimer(this);
        }

        protected virtual IEnumerator KeeperCoroutine()
        {
            yield break;
        }

        protected virtual void CallOnUpdate()
        {
            onUpdate?.Invoke(currentTime);
        }

        public virtual void Stop()
        {
            if (!isRunning)
                return;

            onComplete?.Invoke();
            Finished();
        }

        /// <summary>
        /// Stop the timer without notifying listeners of onComplete
        /// </summary>
        public virtual void Kill()
        {
            if (!isRunning)
                return;

            Finished();
        }

        protected virtual void CallOnComplete()
        {
            onComplete?.Invoke();
        }

        public virtual void DoReset()
        {
            //TimerManager.UpdateActiveTimersList();
        }

        protected virtual void Finished()
        {
            isRunning = false;
            TimerManager.RemoveTimer(this);

            if (keeperCoroutine != null)
            {
                CoroutineHolder.StopCoroutine(keeperCoroutine);
                keeperCoroutine = null;
            }

            if (removeListenersOnFinished)
            {
                onUpdate?.RemoveAllListeners();
                onComplete?.RemoveAllListeners();
            }
        }
    }
}
