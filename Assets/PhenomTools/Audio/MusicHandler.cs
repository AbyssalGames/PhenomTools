using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour
{
    //private static PlaySoundResult currentSong;
    ////private static string currentMusicName;

    //public void Initialize()
    //{
    //    RaceState.stateChangedEvent += OnRaceStateChanged;
    //    Utilities.DelayActionByFrames(1, () => PlayMusic("Derelict"));
    //    //SceneManager.sceneLoaded += OnLevelFinishedLoad;
    //}

    //private void OnDestroy()
    //{
    //    RaceState.stateChangedEvent -= OnRaceStateChanged;
    //    //SceneManager.sceneLoaded -= OnLevelFinishedLoad;
    //}

    //private void OnRaceStateChanged()
    //{
    //    //switch (RaceState.current)
    //    //{
    //    //    case RaceState.State.None:
    //    //        PlayMusic("Chill");
    //    //        break;
    //    //    case RaceState.State.CarSelect:
    //    //        PlayMusic("Hype");
    //    //        break;
    //    //}

    //    switch (RaceState.current)
    //    {
    //        case RaceState.State.None:
    //            PlayMusic("Derelict");
    //            break;
    //        case RaceState.State.CarSelect:
    //            PlayMusic("VoxKraft Pt1");
    //            break;
    //        case RaceState.State.LoadingTrack:
    //            PlayMusic("VoxKraft Pt2");
    //            break;
    //    }
    //}

    ////private void OnLevelFinishedLoad(Scene scene, LoadSceneMode mode)
    ////{
    ////    if (scene.name.Contains("Track"))
    ////        currentMusicName = scene.name;
    ////}

    //public static PlaySoundResult PlayMusic(string musicName = null/*, Transform transformToFollow = null*/)
    //{
    //    Debug.Log("Play Music: " + musicName);

    //    if (string.IsNullOrEmpty(musicName))
    //        return null;

    //    //if (transformToFollow == null)
    //    //{
    //    //    if (SoundManager.activeAudioListener != null)
    //    //        transformToFollow = SoundManager.activeAudioListener.transform;
    //    //    else
                
    //    //}

    //    if (currentSong != null)
    //    {
    //        PlaySoundResult musicFadeCache = currentSong;
    //        musicFadeCache.ActingVariation.Stop();
    //    }

    //    //if (transformToFollow != null)
    //    //    currentSong = MasterAudio.PlaySound3DFollowTransform(musicName, transformToFollow);
    //    //else {
    //        currentSong = MasterAudio.PlaySound(musicName);
    //        //currentSong.ActingVariation.
    //            //}

    //    return currentSong;
    //}
}
