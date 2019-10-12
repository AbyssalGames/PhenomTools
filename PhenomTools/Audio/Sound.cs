using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Object")]
public class Sound : ScriptableObject
{
    public Sound(string bankName, string clipName = null, float volumePercent = 1f, float pitch = 1f, float delaySoundTime = 0f)
    {
        this.bankName = bankName;
        this.clipName = clipName;
        this.volumePercent = volumePercent;
        this.pitch = pitch;
        this.delaySoundTime = delaySoundTime;
    }

    public string bankName;
    public string clipName;
    public float volumePercent = 1f;
    public float pitch = 1f;
    public float delaySoundTime = 0f;
}
