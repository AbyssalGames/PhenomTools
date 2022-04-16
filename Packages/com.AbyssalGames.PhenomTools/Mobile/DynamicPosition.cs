using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using DG.Tweening;

namespace PhenomTools
{
    public class DynamicPosition : DynamicBase
    {
        public float tweenRate = .5f;
        public bool anchored;
        public Vector3 portraitPosition;
        public Vector3 landscapeLeftPosition;
        public Vector3 landscapeRightPosition;

        public Vector3[] positions => new Vector3[] { portraitPosition, landscapeLeftPosition, Vector3.zero, landscapeRightPosition };

        private RectTransform rect;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (anchored)
            {
                rect = transform as RectTransform;
                rect.anchoredPosition = positions[DeviceOrientationManager.deviceOrientationIndex];
            }
            else
            {
                transform.localPosition = positions[DeviceOrientationManager.deviceOrientationIndex];
            }
        }

        protected override void OnDeviceOrientationChanged(int index)
        {
            if (anchored)
            {
                if (tweenRate > 0)
                    rect.DOAnchorPos(positions[index], tweenRate).SetUpdate(UpdateType.Normal, true);
                //DOTween.To(() => rect.anchoredPosition, p => rect.anchoredPosition = p, (Vector2)positions[index], tweenRate).SetUpdate(UpdateType.Normal, true);
                else
                    rect.anchoredPosition = positions[index];
            }
            else
            {
                if (tweenRate > 0)
                    transform.DOLocalMove(positions[index], tweenRate).SetUpdate(UpdateType.Normal, true);
                else
                    transform.localPosition = positions[index];
            }
        }

        [ContextMenu("Set Current as Portrait")]
        public void SetPortrait()
        {
            rect = transform as RectTransform;
            if (anchored)
                portraitPosition = rect.anchoredPosition;
            else
                portraitPosition = transform.localPosition;
        }

        [ContextMenu("Set Current as Landscape Right")]
        public void SetLandscapeRight()
        {
            rect = transform as RectTransform;
            if (anchored)
                landscapeRightPosition = rect.anchoredPosition;
            else
                landscapeRightPosition = transform.localPosition;
        }

        [ContextMenu("Set Current as Landscape Left")]
        public void SetLandscapeLeft()
        {
            rect = transform as RectTransform;
            if (anchored)
                landscapeLeftPosition = rect.anchoredPosition;
            else
                landscapeLeftPosition = transform.localPosition;
        }

        private void Reset()
        {
            if (anchored)
            {
                rect = transform as RectTransform;
                portraitPosition = rect.anchoredPosition;
                landscapeRightPosition = rect.anchoredPosition;
                landscapeLeftPosition = rect.anchoredPosition;
            }
            else
            {
                portraitPosition = transform.localPosition;
                landscapeRightPosition = transform.localPosition;
                landscapeLeftPosition = transform.localPosition;
            }
        }
    }
}