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
        private bool isInit;

        private bool hasBeenVisible;
        private bool overlapsCache;
        private bool containsCache;

        public void BeginVisibilityChecks(RectTransform otherRect)
        {
            rect = transform as RectTransform;
            this.otherRect = otherRect;
            isInit = true;
        }

        protected virtual void Update()
        {
            if (!isInit)
                return;

            CheckVisibility();
        }

        private void CheckVisibility()
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