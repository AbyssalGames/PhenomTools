using UnityEngine;

namespace PhenomTools
{
    [CreateAssetMenu(fileName = "Sound Object")]
    public class Sound : ScriptableObject
    {
        //public Sound()
        //{
        //this.groupName = name;
        //this.clipName = clipName;
        //this.volumePercent = volumePercent;
        //this.pitch = pitch;
        //this.delaySoundTime = delaySoundTime;
        //}

        public string groupName;
        public AudioClip clip;
        public float volumePercent = 1f;
        public float pitch = 1f;
        public float delaySoundTime = 0f;
    }
}
