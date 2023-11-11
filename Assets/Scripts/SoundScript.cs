using System;
using FMOD.Studio;
using FMODUnity;
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
        _musicInstance = RuntimeManager.CreateInstance(MusicMenu);
        _musicInstance.start();
    }

    // private void Start()
    // {
    //     PlayMainMusic();
    // }
}