using DarkTonic.MasterAudio;
using UnityEngine;

namespace PhenomTools
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;

        private static AudioListener _activeAudioListener;
        public static AudioListener activeAudioListener
        {
            get
            {
                if (_activeAudioListener == null || !_activeAudioListener.gameObject.activeInHierarchy)
                    _activeAudioListener = FindObjectOfType<AudioListener>();

                return _activeAudioListener;
            }
        }

        public static SoundChannelHandler channels => instance?._channels;

        [SerializeField]
        private SoundChannelHandler _channels = null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                SetAudioConfiguration();

                if(channels != null)
                    channels.Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void SetAudioConfiguration()
        {
            AudioConfiguration config = AudioSettings.GetConfiguration();

            if (AudioSettings.driverCapabilities == AudioSpeakerMode.Stereo)
                config.speakerMode = AudioSettings.driverCapabilities;
            else if (AudioSettings.driverCapabilities >= AudioSpeakerMode.Mode5point1)
                config.speakerMode = AudioSpeakerMode.Quad;

            //PhenomConsole.Log("Audio Speaker Mode set to: " + config.speakerMode.ToString());

            AudioSettings.Reset(config);
        }

        public static PlaySoundResult Play3DSoundFollowTransform(Sound sound, Transform trans)
        {
            if (sound == null) return null;
            return Play3DSoundFollowTransform(sound.groupName, trans, sound.clip == null ? "" : sound.clip.name, sound.volumePercent, sound.pitch, sound.delaySoundTime);
        }
        public static PlaySoundResult Play3DSoundFollowTransform(string groupName, Transform trans, string clipName = null, float volumePercent = 1f, float? pitch = null, float delaySoundTime = 0f)
        {
            if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(groupName)) return null;
            return MasterAudio.PlaySound3DFollowTransform(groupName, trans, volumePercent, pitch, delaySoundTime, clipName);
        }

        public static PlaySoundResult Play3DSoundAtPointAndFollowTransform(Sound sound, Vector3 point, Transform trans = null, Space space = Space.World)
        {
            if (sound == null) return null;
            return Play3DSoundAtPointAndFollowTransform(sound.groupName, point, trans, sound.clip == null ? "" : sound.clip.name, space, sound.volumePercent, sound.pitch, sound.delaySoundTime);
        }
        public static PlaySoundResult Play3DSoundAtPointAndFollowTransform(string groupName, Vector3 point, Transform trans = null, string clipName = null, Space space = Space.World, float volumePercent = 1f, float? pitch = null, float delaySoundTime = 0f)
        {
            if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(groupName)) return null;

            if (space == Space.Self)
                point = trans.InverseTransformPoint(point);

            PlaySoundResult sound = MasterAudio.PlaySound3DAtVector3(groupName, point, volumePercent, pitch, delaySoundTime, clipName);
            sound.ActingVariation.transform.SetParent(trans);

            return sound;
        }

        public static PlaySoundResult Play3DSoundAtTransform(Sound sound, Transform trans)
        {
            if (sound == null) return null;
            return Play3DSoundAtTransform(sound.groupName, trans, sound.clip == null ? "" : sound.clip.name, sound.volumePercent, sound.pitch, sound.delaySoundTime);
        }
        public static PlaySoundResult Play3DSoundAtTransform(string groupName, Transform trans, string clipName = null, float volumePercent = 1f, float? pitch = null, float delaySoundTime = 0f)
        {
            if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(groupName)) return null;
            return MasterAudio.PlaySound3DAtTransform(groupName, trans, volumePercent, pitch, delaySoundTime, clipName);
        }

        public static PlaySoundResult Play3DSoundAtPoint(Sound sound, Vector3 point)
        {
            if (sound == null) return null;
            return Play3DSoundAtPoint(sound.groupName, point, sound.clip == null ? "" : sound.clip.name, sound.volumePercent, sound.pitch, sound.delaySoundTime);
        }
        public static PlaySoundResult Play3DSoundAtPoint(string groupName, Vector3 point, string clipName = null, float volumePercent = 1f, float? pitch = null, float delaySoundTime = 0f)
        {
            if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(groupName)) return null;
            return MasterAudio.PlaySound3DAtVector3(groupName, point, volumePercent, pitch, delaySoundTime, clipName);
        }

        public static PlaySoundResult Play2DSound(Sound sound)
        {
            if (sound == null) return null;
            return Play2DSound(sound.groupName, sound.clip == null ? "" : sound.clip.name, sound.volumePercent, sound.pitch, sound.delaySoundTime);
        }
        public static PlaySoundResult Play2DSound(string groupName, string clipName = null, float volumePercent = 1f, float pitch = 1f, float delaySoundTime = 0f)
        {
            if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(groupName)) return null;
            return MasterAudio.PlaySound(groupName, volumePercent, pitch, delaySoundTime, clipName);
        }

        public static void RemoveSound(ref PlaySoundResult sound)
        {
            if (sound != null && sound.ActingVariation != null)
                sound.ActingVariation.Stop();

            sound = null;
        }

        public static void StopSound(PlaySoundResult sound)
        {
            if (sound != null && sound.ActingVariation != null)
                sound.ActingVariation.Stop();
        }
    }
}