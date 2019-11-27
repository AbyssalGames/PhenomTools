using DarkTonic.MasterAudio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;// { get; private set; }

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
    public static MusicHandler music => instance?._music;

    [SerializeField]
    private SoundChannelHandler _channels = null;
    [SerializeField]
    private MusicHandler _music = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            SetAudioConfiguration();

            channels?.Initialize();
            //music?.Initialize();
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

        //Debug.LogError("Audio Speaker Mode set to: " + config.speakerMode.ToString());

        AudioSettings.Reset(config);
    }

    public static PlaySoundResult Play3DSoundFollowTransform(Sound sound, Transform trans)
    {
        if (sound == null) return null;
        return Play3DSoundFollowTransform(sound.bankName, trans, sound.clipName, sound.volumePercent, sound.pitch, sound.delaySoundTime);
    }
    public static PlaySoundResult Play3DSoundFollowTransform(string bankName, Transform trans, string clipName = null, float volumePercent = 1f, float? pitch = null, float delaySoundTime = 0f)
    {
        if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(bankName)) return null;
        return MasterAudio.PlaySound3DFollowTransform(bankName, trans, volumePercent, pitch, delaySoundTime, clipName);
    }

    public static PlaySoundResult Play3DSoundAtPointAndFollowTransform(Sound sound, Vector3 point, Transform trans = null, Space space = Space.World)
    {
        if (sound == null) return null;
        return Play3DSoundAtPointAndFollowTransform(sound.bankName, point, trans, sound.clipName, space, sound.volumePercent, sound.pitch, sound.delaySoundTime);
    }
    public static PlaySoundResult Play3DSoundAtPointAndFollowTransform(string bankName, Vector3 point, Transform trans = null, string clipName = null, Space space = Space.World, float volumePercent = 1f, float? pitch = null, float delaySoundTime = 0f)
    {
        if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(bankName)) return null;

        if (space == Space.Self)
            point = trans.InverseTransformPoint(point);

        PlaySoundResult sound = MasterAudio.PlaySound3DAtVector3(bankName, point, volumePercent, pitch, delaySoundTime, clipName);
        sound.ActingVariation.transform.SetParent(trans);

        return sound;
    }

    public static PlaySoundResult Play3DSoundAtTransform(Sound sound, Transform trans)
    {
        if (sound == null) return null;
        return Play3DSoundAtTransform(sound.bankName, trans, sound.clipName, sound.volumePercent, sound.pitch, sound.delaySoundTime);
    }
    public static PlaySoundResult Play3DSoundAtTransform(string bankName, Transform trans, string clipName = null, float volumePercent = 1f, float? pitch = null, float delaySoundTime = 0f)
    {
        if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(bankName)) return null;
        return MasterAudio.PlaySound3DAtTransform(bankName, trans, volumePercent, pitch, delaySoundTime, clipName);
    }

    public static PlaySoundResult Play3DSoundAtPoint(Sound sound, Vector3 point)
    {
        if (sound == null) return null;
        return Play3DSoundAtPoint(sound.bankName, point, sound.clipName, sound.volumePercent, sound.pitch, sound.delaySoundTime);
    }
    public static PlaySoundResult Play3DSoundAtPoint(string bankName, Vector3 point, string clipName = null, float volumePercent = 1f, float? pitch = null, float delaySoundTime = 0f)
    {
        if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(bankName)) return null;
        return MasterAudio.PlaySound3DAtVector3(bankName, point, volumePercent, pitch, delaySoundTime, clipName);
    }

    public static PlaySoundResult Play2DSound(Sound sound)
    {
        if (sound == null) return null;
        return Play2DSound(sound.bankName, sound.clipName, sound.volumePercent, sound.pitch, sound.delaySoundTime);
    }
    public static PlaySoundResult Play2DSound(string bankName, string clipName = null, float volumePercent = 1f, float pitch = 1f, float delaySoundTime = 0f)
    {
        if (!MasterAudio.SoundsReady || string.IsNullOrWhiteSpace(bankName)) return null;
        return MasterAudio.PlaySound(bankName, volumePercent, pitch, delaySoundTime, clipName);
    }

    //public PlaySoundResult Play2DSoundForced(string bankName, string clipName = null, float volumePercent = 1f, float delaySoundTime = 0f)
    //{
    //    //if (GameFlowManager.instance.SoundEffectsDisabled())
    //    //    return null;

    //    return MasterAudio.PlaySound(bankName, volumePercent, null, delaySoundTime, clipName);
    //}

    //public PlaySoundResult Play2DSoundBroadcast(string bankName, string clipName = null, float volumePercent = 1f, float delaySoundTime = 0f)
    //{
    //    //if (GameFlowManager.instance.SoundEffectsDisabled())
    //    //    return null;

    //    return MasterAudio.PlaySound(bankName, volumePercent, null, delaySoundTime, clipName);
    //}

    //public PlaySoundResult PlayWeaponFire(WeaponSounds newSound, Transform attachTrans, bool followTrans)
    //{
    //    //  Don't play any primary SFX if the race is finished on this machine
    //    if (GameFlowManager.instance.SoundEffectsDisabled())
    //        return null;

    //    if (!followTrans)
    //        return Play3DSoundAtLocation("Weapons", newSound.ToString(), attachTrans);

    //    return Play3DSoundFollow("Weapons", newSound.ToString(), attachTrans);
    //}

    //public PlaySoundResult PlayWeaponHit(WeaponSounds newSound, Vector3 location)
    //{
    //    //  Don't play any primary SFX if the race is finished on this machine
    //    if (GameFlowManager.instance.SoundEffectsDisabled())
    //        return null;

    //    var temp = MasterAudio.PlaySound3DAtVector3("WeaponHits", location, 1, null, 0, newSound.ToString());

    //    //if (newSound == WeaponSounds.Hit_Dynamite && temp != null && temp.ActingVariation != null)
    //    //   Debug.LogError("Bomb hit played at location: " + location.ToString());

    //    return temp;
    //    //return MasterAudio.PlaySound3DAtVector3("WeaponHits", location, 1, null, 0, newSound.ToString());
    //}

    //public PlaySoundResult MenuSound(string clipName)
    //{
    //    //if (GameFlowManager.instance.SoundEffectsDisabled())
    //    //    return null;

    //    return Play2DSound("MenuSounds", clipName);
    //}



    //public PlaySoundResult PlayRaceSound(RaceSounds _raceSound, Transform location, bool followTrans = false)
    //{
    //    if (GameFlowManager.instance.SoundEffectsDisabled())
    //        return null;

    //    if (!followTrans)
    //        return Play3DSoundAtLocation("RaceSounds", _raceSound.ToString(), location);

    //    return Play3DSoundFollow("RaceSounds", _raceSound.ToString(), location);
    //}

    //public PlaySoundResult PlayCollisionSound(CollisionType collisionType, Transform location, bool followTrans = false)
    //{
    //    if (GameFlowManager.instance.SoundEffectsDisabled())
    //        return null;

    //    if (!followTrans)
    //        return Play3DSoundAtLocation("Collision_" + collisionType.ToString(), null, location);

    //    return Play3DSoundFollow("Collision_" + collisionType.ToString(), null, location);
    //}

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

    //public PlaySoundResult PlayCarSound(CarSounds newSound, Transform emitterTransform = null, float volume = 1f)
    //{
    //    if (GameFlowManager.instance.SoundEffectsDisabled())
    //        return null;

    //    // If it's a 'default' setting, which is none, don't play anything
    //    if (newSound == CarSounds.None || lastCarSound == newSound)
    //    {
    //        lastCarSound = newSound;
    //        return null;
    //    }

    //    lastCarSound = newSound;

    //    if (emitterTransform != null)
    //        return Play3DSoundFollow("CarSounds", newSound.ToString(), emitterTransform, volume);
    //    else
    //        return Play3DSoundAtLocation("CarSounds", newSound.ToString(), transform, volume);
    //}

    //public PlaySoundResult PlayCarSound2D(CarSounds newSound, float volume = 1f)
    //{
    //    if (GameFlowManager.instance.SoundEffectsDisabled())
    //        return null;

    //    // If it's a 'default' setting, which is none, don't play anything
    //    if (newSound == CarSounds.None || lastCarSound == newSound)
    //    {
    //        lastCarSound = newSound;
    //        return null;
    //    }

    //    lastCarSound = newSound;

    //    return Play2DSound("CarSounds", newSound.ToString(), volume);
    //}

    //private PlaySoundResult currentTireSound = null;

    //public PlaySoundResult PlayTireSound(TireSounds newSound, Transform emitterTransform = null)
    //{
    //    if (GameFlowManager.instance.SoundEffectsDisabled())
    //        return null;

    //    if (currentTireSound != null)
    //        RemoveSound(ref currentTireSound);

    //    // If it's a 'default' setting, which is none, don't play anything
    //    if (newSound == TireSounds.None || lastTireSound == newSound)
    //    {
    //        lastTireSound = newSound;
    //        return currentTireSound;
    //    }

    //    lastTireSound = newSound;

    //    if (emitterTransform != null)
    //        currentTireSound = Play3DSoundFollow("Tires", newSound.ToString(), emitterTransform);
    //    else
    //        currentTireSound = Play3DSoundAtLocation("Tires", newSound.ToString(), transform);

    //    return currentTireSound;
    //}

    //public PlaySoundResult PlayVO(VOType voType, int carType, Transform transform = null, float delay = 0f, string variationName = null)
    //{
    //    //Debug.Log("Play VO: " + voType.ToString() + '_' + CharacterIndex.GetCharacterTypeStringByIndex(carType));
    //    if (transform == null)
    //        return MasterAudio.PlaySound(voType.ToString() + '_' + CharacterIndex.GetCharacterTypeStringByIndex(carType), 1, null, delay, variationName);
    //    else
    //        return MasterAudio.PlaySound3DFollowTransform(voType.ToString() + '_' + CharacterIndex.GetCharacterTypeStringByIndex(carType), transform, 1, null, delay, variationName);
    //}

    //public static void AssignSoundListenersToNewButton(Selectable newButton)
    //{
    //    if(newButton.GetComponent<Button>())
    //        newButton.GetComponent<Button>().onClick.AddListener(instance.ButtonClickSound);

    //    if (newButton.GetComponent<Toggle>())
    //        newButton.GetComponent<Toggle>().onValueChanged.AddListener(instance.ButtonClickSound);
    //}

    //public void ButtonClickSound()
    //{
    //    Play2DSound(new Sound("ButtonClick", "ButtonDown"));
    //}
    //public void ButtonClickSound(bool on)
    //{
    //    if (on)
    //        Play2DSound(new Sound("ButtonClick", "ButtonDown"));
    //    else
    //        Play2DSound(new Sound("ButtonClick", "ButtonUp"));
    //}
}

//[Serializable]
//public class SoundProfile
//{
//    public string bankName;
//    public string clipName;
//    public float duration;
//}