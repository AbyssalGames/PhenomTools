using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;

public abstract class DynamicBase : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        DeviceOrientationManager.onDeviceOrientationChanged += OnDeviceOrientationChanged;
    }

    protected virtual void OnDisable()
    {
        DeviceOrientationManager.onDeviceOrientationChanged -= OnDeviceOrientationChanged;
    }

    protected abstract void OnDeviceOrientationChanged(int index);
}
