using System;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class SoundScript : MonoBehaviour
{
    
    [SerializeField] private EventReference MusicMenu;
    [SerializeField] private EventReference MusicLevel;

    //public static SoundScript Instance;
    
    private EventInstance _musicInstance;

    
    public void PlayMainMusic()
    {
        _musicInstance.stop(STOP_MODE.IMMEDIATE);
        _musicInstance = RuntimeManager.CreateInstance(MusicMenu);
        _musicInstance.start();
    }

    public void OnDestroy()
    {
        _musicInstance.stop(STOP_MODE.IMMEDIATE);
    }

    // private void Start()
    // {
    //     PlayMainMusic();
    // }
}