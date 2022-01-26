using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PhenomTools
{
    public class DynamicRotation : DynamicBase
    {
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
                transform.DOLocalRotate(rotations[index], .5f).SetUpdate(UpdateType.Normal, true);
            //transform.DOLocalRotateQuaternion(Quaternion.Euler(rotations[index]), .5f).SetUpdate(UpdateType.Normal, true);
            else
                transform.DORotate(rotations[index], .5f).SetUpdate(UpdateType.Normal, true);
            //transform.DORotateQuaternion(Quaternion.Euler(rotations[index]), .5f).SetUpdate(UpdateType.Normal, true);
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