using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PhenomTools
{
    [Serializable]
    public class TimeKeeperBase
    {
        public float duration = 30f;
        public bool useSeconds = true; // Optimize to 1 per second vs frames per second
        public AnimatorUpdateMode updateMode;

        [Serializable]
        public class UnityEventFloat : UnityEvent<float> { }
        public UnityEventFloat onUpdate;
        public UnityEvent onComplete;

        public bool isRunning { get; protected set; }
        public float currentTime { get; protected set; }
        public float startTime { get; protected set; }

        public IEnumerator keeperCoroutine;

        public virtual void Begin() => Begin(duration, useSeconds, updateMode);
        public virtual void Begin(float duration, bool useSeconds = true, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
        {
            if (isRunning)
                Stop();

            this.useSeconds = useSeconds;
            this.duration = duration;
            this.updateMode = updateMode;

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

        public virtual void Reset()
        {
            TimerManager.UpdateActiveTimersList();
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

            onUpdate?.RemoveAllListeners();
            onComplete?.RemoveAllListeners();
        }
    }
}
