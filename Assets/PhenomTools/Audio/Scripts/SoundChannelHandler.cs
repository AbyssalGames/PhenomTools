using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhenomTools
{
    [Serializable]
    public class SoundChannel
    {
        public float defaultVolume;
        public bool isMuted;
        
        //[HideInInspector]
        public float currentVolume;

        public IEnumerator fadeCoroutine;
    }

    [Serializable]
    public class DSG
    {
        public bool createOnAwake;

        public GameObject prefab;

        [HideInInspector]
        public GameObject group;

        public void CreateDynamicSoundGroup()
        {
            if (prefab != null && group == null)
            {
                group = UnityEngine.Object.Instantiate(prefab);
                SoundChannelHandler.DynamicSoundGroupCreated(this);
            }
        }

        public void DestroyDynamicSoundGroup()
        {
            if (group != null)
            {
                UnityEngine.Object.Destroy(group);
                SoundChannelHandler.DynamicSoundGroupDestroyed(this);
            }
        }
    }

    public class SoundChannelHandler : MonoBehaviour
    {
        public static event Action<SoundChannel> volumeUpdatedEvent;
        public static event Action<DSG> dynamicSoundGroupCreatedEvent;
        public static event Action<DSG> dynamicSoundGroupDestroyedEvent;

        [Serializable]
        public class SoundChannelDict : SerializableDictionaryBase<string, SoundChannel> { }
        public SoundChannelDict soundChannels = new SoundChannelDict() 
        { 
            { "Master", new SoundChannel { defaultVolume = 1f } },
            { "SFX", new SoundChannel { defaultVolume = 1f } },
            { "Music", new SoundChannel { defaultVolume = 1f } }
        };

        [Serializable]
        public class DSGDict : SerializableDictionaryBase<string, DSG> { }
        [SerializeField]
        private DSGDict dynamicSoundGroups = null;

        public void Initialize()
        {
            AdjustChannelVolume("Master", PlayerPrefs.GetFloat("Master_Volume", soundChannels["Master"].defaultVolume));

            foreach (KeyValuePair<string, SoundChannel> channel in soundChannels)
            {
                if (channel.Key == "Master")
                    continue;

                AdjustChannelVolume(channel.Key, PlayerPrefs.GetFloat(channel.Key + "_Volume", channel.Value.defaultVolume));
                //channel.Value.currentVolume = PlayerPrefs.GetFloat(channel.Key + "_Volume", channel.Value.defaultVolume);
            }

            foreach (DSG group in dynamicSoundGroups.Values)
            {
                if (group.createOnAwake)
                    group.CreateDynamicSoundGroup();
            }
        }

        public void AdjustChannelVolume(string channelName, float newValue, bool overrideSettings = false, float fadeDuration = 0f)
        {
            if (soundChannels.TryGetValue(channelName, out SoundChannel channel))
            {
                if (!overrideSettings)
                    channel.currentVolume = newValue;

                if (!channel.isMuted)
                {
                    if (channelName == "Master")
                    {
                        channel.currentVolume = newValue;
                        MasterAudio.MasterVolumeLevel = newValue;
                        PlayerPrefs.SetFloat(channelName + "_Volume", newValue);
                    }
                    else
                    {
                        if (MasterAudio.GrabBusByName(channelName) != null)
                        {
                            if (fadeDuration == 0f)
                                MasterAudio.SetBusVolumeByName(channelName, newValue);
                            else
                                MasterAudio.FadeBusToVolume(channelName, newValue, fadeDuration);

                            PlayerPrefs.SetFloat(channelName + "_Volume", newValue);
                            volumeUpdatedEvent?.Invoke(channel);
                        }
                        else
                        {
                            PhenomConsole.Log("No Master Audio Bus with name: " + channelName, PhenomLogType.Error);
                        }
                    }
                }
            }
            else
                PhenomConsole.Log("No Sound Channel with bus name: " + channelName, PhenomLogType.Error);
        }

        public void ToggleChannelMute(string channelName, bool muted)
        {
            if (soundChannels.TryGetValue(channelName, out SoundChannel channel))
            {
                channel.isMuted = muted;

                if (MasterAudio.GrabBusByName(channelName) == null)
                    return;

                if (muted)
                    MasterAudio.FadeBusToVolume(channelName, 0, .2f);
                else
                    MasterAudio.FadeBusToVolume(channelName, channel.currentVolume, .2f);
            }
            else
                PhenomConsole.Log("No Sound Channel with bus name: " + channelName, PhenomLogType.Error);
        }

        //public bool TryGetChannelIndexByName(string channelName, out int index)
        //{
        //    index = GetChannelIndexByName(channelName);
        //    return index > -1;
        //}

        //public int GetChannelIndexByName(string channelName)
        //{
        //    for (int i = 0; i < soundChannels.Length; i++)
        //    {
        //        if (soundChannels[i].busName == channelName)
        //            return i;
        //    }

        //    return -1;
        //}

        //public bool TryGetChannelByName(string channelName, out SoundChannel channel)
        //{
        //    channel = GetChannelByName(channelName);
        //    return channel != null;
        //}

        //public SoundChannel GetChannelByName(string channelName)
        //{
        //    foreach (SoundChannel c in soundChannels.Values)
        //    {
        //        if (c.busName == channelName)
        //            return c;
        //    }

        //    return null;
        //}

        public static void DynamicSoundGroupCreated(DSG group)
        {
            dynamicSoundGroupCreatedEvent?.Invoke(group);

            //SoundManager.channels.AdjustChannelVolume(channel, channel.currentVolume);
            //SoundManager.channels.ToggleChannelMute(channel, channel.isMuted);
        }

        public static void DynamicSoundGroupDestroyed(DSG group)
        {
            dynamicSoundGroupDestroyedEvent?.Invoke(group);
        }
    }
}