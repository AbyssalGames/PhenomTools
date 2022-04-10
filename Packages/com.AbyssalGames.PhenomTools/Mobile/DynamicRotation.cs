using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PhenomTools
{
    public class DynamicRotation : DynamicBase
    {
        public float tweenRate = .5f;
        public bool local = true;
        public Vector3 portraitRotation;
        public Vector3 landscapeLeftRotation;
        public Vector3 landscapeRightRotation;

        private Vector3[] rotations => new Vector3[] { portraitRotation, landscapeLeftRotation, Vector3.zero, landscapeRightRotation };

        protected override void OnEnable()
        {
            base.OnEnable();

            if (local)
                transform.localEulerAngles = rotations[DeviceOrientationManager.deviceOrientationIndex];
            else
                transform.eulerAngles = rotations[DeviceOrientationManager.deviceOrientationIndex];
        }

        protected override void OnDeviceOrientationChanged(int index)
        {
            if (local)
            {
                if (tweenRate > 0)
                    transform.DOLocalRotate(rotations[index], tweenRate).SetUpdate(UpdateType.Normal, true);
                else
                    transform.localEulerAngles = rotations[index];
            }
            else
            {
                if (tweenRate > 0)
                    transform.DORotate(rotations[index], tweenRate).SetUpdate(UpdateType.Normal, true);
                else
                    transform.eulerAngles = rotations[index];
            }
        }

        public void Reset()
        {
            portraitRotation = Vector3.zero;
            landscapeLeftRotation = new Vector3(0, 0, 270f);
            landscapeRightRotation = new Vector3(0, 0, 90f);
        }

        [ContextMenu("Set Current as Portrait")]
        public void SetPortrait()
        {
            if (local)
                portraitRotation = transform.localEulerAngles;
            else
                portraitRotation = transform.eulerAngles;
        }

        [ContextMenu("Set Current as Landscape Right")]
        public void SetLandscapeRight()
        {
            if (local)
                landscapeRightRotation = transform.localEulerAngles;
            else
                landscapeRightRotation = transform.eulerAngles;
        }

        [ContextMenu("Set Current as Landscape Left")]
        public void SetLandscapeLeft()
        {
            if (local)
                landscapeLeftRotation = transform.localEulerAngles;
            else
                landscapeLeftRotation = transform.eulerAngles;
        }
    }
}