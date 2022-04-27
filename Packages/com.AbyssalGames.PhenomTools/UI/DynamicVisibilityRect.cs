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
        public event Action onFirstBecameVisible;
        public event Action onBecameVisible;
        public event Action onBecameHidden;
        public event Action onBecameFullyVisible;
        public event Action onBecamePartlyHidden;
        public bool isVisible { get; private set; }
        public bool isFullyVisible { get; private set; }

        private RectTransform rect;
        private RectTransform otherRect;

        private bool hasBeenVisible;
        private bool overlapsCache;
        private bool containsCache;

        private IEnumerator visibilityCheckRoutine;
        private Action checkEvent;
        private UnityEvent checkUnityEvent;

        public virtual void BeginVisibilityChecks(RectTransform otherRect, int framesBetweenChecks = 0)
        {
            rect = transform as RectTransform;
            this.otherRect = otherRect;

            visibilityCheckRoutine = PhenomUtils.RepeatActionByFrames(framesBetweenChecks, CheckVisibility);
        }

        public virtual void BeginVisibilityChecks(RectTransform otherRect, float timeBetweenChecks)
        {
            rect = transform as RectTransform;
            this.otherRect = otherRect;

            visibilityCheckRoutine = PhenomUtils.RepeatActionByTime(timeBetweenChecks, CheckVisibility);
        }

        public virtual void BeginVisibilityChecks(RectTransform otherRect, Action checkEvent)
        {
            rect = transform as RectTransform;
            this.otherRect = otherRect;
            this.checkEvent = checkEvent;

            checkEvent += CheckVisibility;
        }

        public virtual void BeginVisibilityChecks(RectTransform otherRect, UnityEvent checkUnityEvent)
        {
            rect = transform as RectTransform;
            this.otherRect = otherRect;
            this.checkUnityEvent = checkUnityEvent;

            checkUnityEvent.AddListener(CheckVisibility);
        }

        public virtual void EndVisibilityChecks()
        {
            if (visibilityCheckRoutine != null)
                visibilityCheckRoutine.Stop();

            if (checkEvent != null)
                checkEvent -= CheckVisibility;

            if (checkUnityEvent != null)
                checkUnityEvent.RemoveListener(CheckVisibility);
        }

        protected virtual void OnDisable()
        {
            EndVisibilityChecks();
        }

        public virtual void CheckVisibility()
        {
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