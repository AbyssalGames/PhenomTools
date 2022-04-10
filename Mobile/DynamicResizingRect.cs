using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;
using DG.Tweening;
using DG.Tweening.Plugins;

namespace PhenomTools
{
    [RequireComponent(typeof(RectTransform))]
    public class DynamicResizingRect : DynamicBase
    {
        public float tweenRate = .5f;
        public Vector2 portraitRect;
        public Vector2 landscapeLeftRect;
        public Vector2 landscapeRightRect;

        public Vector2[] rects => new Vector2[] { portraitRect, landscapeLeftRect, Vector2.zero, landscapeRightRect };

        protected RectTransform rt;

        protected override void OnEnable()
        {
            base.OnEnable();

            rt = transform as RectTransform;

            int index = DeviceOrientationManager.deviceOrientationIndex;
            rt.sizeDelta = rects[index];
        }

        protected override void OnDeviceOrientationChanged(int index)
        {
            if (tweenRate > 0)
                rt.DOSizeDelta(rects[index], tweenRate).SetUpdate(UpdateType.Normal, true);
            else
                rt.sizeDelta = rects[index];
        }

        [ContextMenu("Set Current as Portrait")]
        public void SetPortrait()
        {
            portraitRect = rt.sizeDelta;
        }

        [ContextMenu("Set Current as Landscape Right")]
        public void SetLandscapeRight()
        {
            landscapeRightRect = rt.sizeDelta;
        }

        [ContextMenu("Set Current as Landscape Left")]
        public void SetLandscapeLeft()
        {
            landscapeLeftRect = rt.sizeDelta;
        }

        private void Reset()
        {
            rt = transform as RectTransform;
            portraitRect = rt.sizeDelta;
            landscapeRightRect = rt.sizeDelta;
            landscapeLeftRect = rt.sizeDelta;
        }
    }
}