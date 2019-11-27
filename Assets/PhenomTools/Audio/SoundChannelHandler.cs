using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SoundChannel
{
    public string busName;

    public float defaultVolume;

    public bool isMuted;
    public bool createOnAwake;

    public GameObject dynamicSoundGroupPrefab;

    [HideInInspector]
    public GameObject dynamicSoundGroup;
    [HideInInspector]
    public float currentVolume;

    public IEnumerator fadeCoroutine;

    public void CreateDynamicSoundGroup()
    {
        if (dynamicSoundGroup == null)
        {
            dynamicSoundGroup = UnityEngine.Object.Instantiate(dynamicSoundGroupPrefab);
            SoundChannelHandler.DynamicSoundGroupCreated(this);
        }
    }

    public void DestroyDynamicSoundGroup()
    {
        if (dynamicSoundGroup != null)
        {
            UnityEngine.Object.Destroy(dynamicSoundGroup);
            SoundChannelHandler.DynamicSoundGroupDestroyed(this);
        }
    }
}

public class SoundChannelHandler : MonoBehaviour
{
    public static event Action<SoundChannel> volumeUpdatedEvent;
    public static event Action<SoundChannel> dynamicSoundGroupCreatedEvent;
    public static event Action<SoundChannel> dynamicSoundGroupDestroyedEvent;

    [SerializeField]
    private SoundChannel[] soundChannels = new SoundChannel[5]
    {
        new SoundChannel{ busName = "Master", currentVolume = .5f, defaultVolume = .5f },
        new SoundChannel{ busName = "Slot", currentVolume = .9f, defaultVolume = .9f },
        new SoundChannel{ busName = "Race", currentVolume = .9f, defaultVolume = .9f },
        new SoundChannel{ busName = "Music", currentVolume = .5f, defaultVolume = .5f, createOnAwake = true },
        new SoundChannel{ busName = "UI", currentVolume = .3f, defaultVolume = .3f, createOnAwake = true }
    };

    public void Initialize()
    {
        AdjustChannelVolume("Master", soundChannels[GetChannelIndexByName("Master")].defaultVolume);

        foreach (SoundChannel channel in soundChannels)
        {
            if (channel.createOnAwake)
            {
                channel.currentVolume = channel.defaultVolume;
                channel.CreateDynamicSoundGroup();
            }
        }
    }
    public void AdjustChannelVolume(string channelName, float newValue, bool overrideSettings = false, float fadeDuration = 0f)
    {
        if (channelName == "Master")
        {
            if (TryGetChannelByName(channelName, out SoundChannel channel))
                channel.currentVolume = newValue;

            MasterAudio.MasterVolumeLevel = newValue;
        }
        else
        {
            if (TryGetChannelByName(channelName, out SoundChannel channel))
                AdjustChannelVolume(channel, newValue, overrideSettings, fadeDuration);
            else
                PhenomConsole.Log("No Sound Channel with bus name: " + channelName, LogType.Error);
        }
    }
    public void AdjustChannelVolume(SoundChannel channel, float newValue, bool overrideSettings = false, float fadeDuration = 0f)
    {
        if (!overrideSettings)
            channel.currentVolume = newValue;

        if (!channel.isMuted)
        {
            if (MasterAudio.GrabBusByName(channel.busName) != null)
            {
                if (fadeDuration == 0f)
                    MasterAudio.SetBusVolumeByName(channel.busName, newValue);
                else
                    MasterAudio.FadeBusToVolume(channel.busName, newValue, fadeDuration);

                volumeUpdatedEvent?.Invoke(channel);
            }
            else
                PhenomConsole.Log("No Master Audio Bus with name: " + channel.busName, LogType.Error);
        }
    }

    public void ToggleChannelMute(string channelName, bool muted)
    {
        if (TryGetChannelByName(channelName, out SoundChannel channel))
            ToggleChannelMute(channel, muted);
    }
    public void ToggleChannelMute(SoundChannel channel, bool muted)
    {
        channel.isMuted = muted;

        if (MasterAudio.GrabBusByName(channel.busName) == null)
            return;

        if (muted)
            MasterAudio.FadeBusToVolume(channel.busName, 0, .2f);
        else
            MasterAudio.FadeBusToVolume(channel.busName, channel.currentVolume, .2f);
    }

    public bool TryGetChannelIndexByName(string channelName, out int index)
    {
        index = GetChannelIndexByName(channelName);
        return index > -1;
    }

    public int GetChannelIndexByName(string channelName)
    {
        for (int i = 0; i < soundChannels.Length; i++)
        {
            if (soundChannels[i].busName == channelName)
                return i;
        }

        return -1;
    }

    public bool TryGetChannelByName(string channelName, out SoundChannel channel)
    {
        channel = GetChannelByName(channelName);
        return channel != null;
    }

    public SoundChannel GetChannelByName(string channelName)
    {
        foreach (SoundChannel c in soundChannels)
        {
            if (c.busName == channelName)
                return c;
        }

        return null;
    }

    public static void DynamicSoundGroupCreated(SoundChannel channel)
    {
        dynamicSoundGroupCreatedEvent?.Invoke(channel);

        SoundManager.channels.AdjustChannelVolume(channel, channel.currentVolume);
        SoundManager.channels.ToggleChannelMute(channel, channel.isMuted);
    }

    public static void DynamicSoundGroupDestroyed(SoundChannel channel)
    {
        dynamicSoundGroupDestroyedEvent?.Invoke(channel);
    }
}
