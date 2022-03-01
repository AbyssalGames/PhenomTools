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
    public class DynamicRect : DynamicBase
    {
        public Vector2 portraitRect;
        public Vector2 landscapeLeftRect;
        public Vector2 landscapeRightRect;

        //[Space]
        //public Vector2 portraitScreenMult;
        //public Vector2 landscapeLeftScreenMult;
        //public Vector2 landscapeRightScreenMult;

        public Vector2[] rects => new Vector2[] { portraitRect, landscapeLeftRect, Vector2.zero, landscapeRightRect };
        //public Vector2[] mults => new Vector2[] { portraitScreenMult, landscapeLeftScreenMult, Vector2.zero, landscapeRightScreenMult };

        protected RectTransform rt;

        protected override void OnEnable()
        {
            base.OnEnable();

            rt = transform as RectTransform;

            int index = DeviceOrientationManager.deviceOrientationIndex;
            rt.sizeDelta = rects[index];// + new Vector2(Screen.width * mults[index].x, Screen.height * mults[index].y);
        }

        protected override void OnDeviceOrientationChanged(int index)
        {
            rt.DOSizeDelta(rects[index], .5f).SetUpdate(UpdateType.Normal, true);
            //rt.DODeltaScale(rects[index] /*+ new Vector2(Screen.width * mults[index].x, Screen.height * mults[index].y)*/, .5f).SetUpdate(UpdateType.Normal, true);
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