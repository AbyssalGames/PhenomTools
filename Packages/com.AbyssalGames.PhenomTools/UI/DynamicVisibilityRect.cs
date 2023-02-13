using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using UnityEngine.UI;
using UnityEngine.Events;

namespace PhenomTools
{
    [RequireComponent(typeof(RectTransform))]
    public class DynamicVisibilityRect : MonoBehaviour
    {
        [Flags]
        public enum VisibilityCheckType
        {
            None = 0, Frames = 1 << 0, Time = 1 << 1, Event = 1 << 2, UnityEvent = 1 << 3
        }
        
        public Action onFirstBecameVisible;
        public Action onBecameVisible;
        public Action onBecameHidden;
        public Action onBecameFullyVisible;
        public Action onBecamePartlyHidden;
        public bool isVisible { get; private set; }
        public bool isFullyVisible { get; private set; }
        public VisibilityCheckType visibilityCheckType;// { get; private set; }

        private RectTransform rect;
        private RectTransform otherRect;

        private bool hasBeenVisible;
        private bool overlapsCache;
        private bool containsCache;

        private List<IEnumerator> visibilityCheckRoutines = new List<IEnumerator>();

        private int framesBetweenChecks;
        private float timeBetweenChecks;
        private Action checkEvent;
        private UnityEvent checkUnityEvent;

        public virtual void BeginVisibilityChecks(RectTransform otherRect, int framesBetweenChecks = 0)
        {
            rect = transform as RectTransform;
            this.otherRect = otherRect;
            this.framesBetweenChecks = framesBetweenChecks;
            visibilityCheckType |= VisibilityCheckType.Frames;

            visibilityCheckRoutines.Add(PhenomUtils.RepeatActionByFrames(framesBetweenChecks, CheckVisibility));
            CheckVisibility();
        }

        public virtual void BeginVisibilityChecks(RectTransform otherRect, float timeBetweenChecks)
        {
            rect = transform as RectTransform;
            this.otherRect = otherRect;
            this.timeBetweenChecks = timeBetweenChecks;
            visibilityCheckType |= VisibilityCheckType.Time;

            visibilityCheckRoutines.Add(PhenomUtils.RepeatActionByTime(timeBetweenChecks, CheckVisibility));
            CheckVisibility();
        }

        public virtual void BeginVisibilityChecks(RectTransform otherRect, Action checkEvent)
        {
            rect = transform as RectTransform;
            this.otherRect = otherRect;
            this.checkEvent = checkEvent;
            visibilityCheckType |= VisibilityCheckType.Event;

            checkEvent += CheckVisibility;
            CheckVisibility();
        }

        public virtual void BeginVisibilityChecks(RectTransform otherRect, UnityEvent checkUnityEvent)
        {
            rect = transform as RectTransform;
            this.otherRect = otherRect;
            this.checkUnityEvent = checkUnityEvent;
            visibilityCheckType |= VisibilityCheckType.UnityEvent;

            checkUnityEvent.AddListener(CheckVisibility);
            CheckVisibility();
        }

        public virtual void PauseVisibilityChecks()
        {
            foreach (IEnumerator visibilityCheckRoutine in visibilityCheckRoutines)
                visibilityCheckRoutine?.Stop();
            
            visibilityCheckRoutines.Clear();
            
            if (checkEvent != null)
                checkEvent -= CheckVisibility;
            
            checkUnityEvent?.RemoveListener(CheckVisibility);
        }

        public virtual void EndVisibilityChecks()
        {
            PauseVisibilityChecks();
            
            framesBetweenChecks = 0;
            timeBetweenChecks = 0;

            visibilityCheckType = VisibilityCheckType.None;
        }

        protected virtual void OnEnable()
        {
            if (visibilityCheckType == VisibilityCheckType.None)
            {
                Debug.LogError("Hmm");
                return;
            }
            
            if(visibilityCheckType.HasFlag(VisibilityCheckType.Frames))
                BeginVisibilityChecks(otherRect, framesBetweenChecks);
            if(visibilityCheckType.HasFlag(VisibilityCheckType.Time))
                BeginVisibilityChecks(otherRect, timeBetweenChecks);
            if(visibilityCheckType.HasFlag(VisibilityCheckType.Event))
                BeginVisibilityChecks(otherRect, checkEvent);
            if(visibilityCheckType.HasFlag(VisibilityCheckType.UnityEvent))
                BeginVisibilityChecks(otherRect, checkUnityEvent);
        }

        protected virtual void OnDisable()
        {
            PauseVisibilityChecks();
        }

        public virtual void CheckVisibility()
        {
            if (otherRect == null)
                return;
            
            isVisible = rect.Overlaps(otherRect);

            if (isVisible != overlapsCache)
            {
                if (isVisible)
                    BecameVisible();
                else
                    BecameHidden();

                overlapsCache = isVisible;
            }

            isFullyVisible = otherRect.Contains(rect);

            if (isFullyVisible != containsCache)
            {
                if (isFullyVisible)
                    BecameFullyVisible();
                else
                    BecamePartiallyHidden();

                containsCache = isFullyVisible;
            }
        }

        protected virtual void BecameVisible()
        {
            if (!hasBeenVisible)
            {
                FirstBecameVisible();
                hasBeenVisible = true;
            }

            onBecameVisible?.Invoke();
        }
        protected virtual void FirstBecameVisible()
        {
            onFirstBecameVisible?.Invoke();
        }
        protected virtual void BecameHidden()
        {
            onBecameHidden?.Invoke();
        }
        protected virtual void BecameFullyVisible()
        {
            onBecameFullyVisible?.Invoke();
        }
        protected virtual void BecamePartiallyHidden()
        {
            onBecamePartlyHidden?.Invoke();
        }
    }
}