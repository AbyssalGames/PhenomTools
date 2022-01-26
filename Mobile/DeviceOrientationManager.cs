using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;

namespace PhenomTools
{
    public class DeviceOrientationManager : MonoBehaviour
    {
        public static int deviceOrientationIndex;
        public static event Action<int> onDeviceOrientationChanged;

#if UNITY_EDITOR
        public DeviceOrientation forceOrientation = DeviceOrientation.Portrait;
#endif

        private DeviceOrientation orientationCache = DeviceOrientation.Portrait;

#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitialize()
        {
            CoroutineHolder.instance.gameObject.AddComponent<DeviceOrientationManager>();
        }
#endif

        private void Update()
        {
#if UNITY_EDITOR
            if (forceOrientation != orientationCache)
            {
                int index = forceOrientation.ToIndex();

                deviceOrientationIndex = index;
                PhenomUtils.DelayActionByFrames(2, () => onDeviceOrientationChanged?.Invoke(deviceOrientationIndex));

                //Physics.gravity = GetOrientationDirection(index) * -10f;

                orientationCache = forceOrientation;
            }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.deviceOrientation != orientationCache 
            && Input.deviceOrientation != DeviceOrientation.FaceDown 
            && Input.deviceOrientation != DeviceOrientation.FaceUp 
            && Input.deviceOrientation != DeviceOrientation.Unknown 
            && Input.deviceOrientation != DeviceOrientation.PortraitUpsideDown)
        {
            int index = Input.deviceOrientation.ToIndex();

            deviceOrientationIndex = index;
            onDeviceOrientationChanged?.Invoke(index);

            //Physics.gravity = GetOrientationDirection(index) * -10f;

            orientationCache = Input.deviceOrientation;
        }
#endif
        }

        //private Vector3 GetOrientationDirection(int index)
        //{
        //    switch (index)
        //    {
        //        default: return Vector3.up;
        //        case 1: return Vector3.right;
        //        case 2: return Vector3.down;
        //        case 3: return Vector3.left;
        //    }
        //}
    }
}