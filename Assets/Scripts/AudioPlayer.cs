using System;
using System.Resources;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioPlayer : MonoBehaviour
{
   public BackGroundType bgMusicType;
   public static AudioPlayer Instance;
   
   [SerializeField] private EventReference EnterDoorFromGrass;
   [SerializeField] private EventReference EnterDoorFromCave;
   [SerializeField] private EventReference EnterDoorFromDark;
   [SerializeField] private EventReference EnterDoorFromSpace;
   [SerializeField] private EventReference EnterDoorFromAutomata;

   [SerializeField] private EventReference ExitDoorInGrass;
   [SerializeField] private EventReference ExitDoorInCave;
   [SerializeField] private EventReference ExitDoorInDark;
   [SerializeField] private EventReference ExitDoorInSpace;
   [SerializeField] private EventReference ExitDoorInAutomata;
   
   [SerializeField] private EventReference ButtonClickSFX;
   [SerializeField] private EventReference OpenDoorSFX;
   [SerializeField] private EventReference CloseDoorSFX;
   
   [SerializeField] private EventReference MusicMainMenu;
   [SerializeField] private EventReference MusicGrass;
   [SerializeField] private EventReference MusicCave;
   [SerializeField] private EventReference MusicDark;
   [SerializeField] private EventReference MusicSpace;
   [SerializeField] private EventReference MusicAutomata;
   
   
   private EventInstance _musicInstance;
   
   
   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }


   public void StopMusic()
   {
      _musicInstance.stop(STOP_MODE.IMMEDIATE);
   }
   
   //General SFX
   
   public void PlayButtonClickSFX()
   {
      RuntimeManager.PlayOneShot(ButtonClickSFX);
   }
   


   public void PlayEnterDoor ()
   {
      RuntimeManager.PlayOneShot(OpenDoorSFX);
      
      switch (bgMusicType)
      {
         case BackGroundType.GrassLands:
            RuntimeManager.PlayOneShot(EnterDoorFromGrass);
            break;
         case BackGroundType.CaveLands:
            RuntimeManager.PlayOneShot(EnterDoorFromCave);
            break;
         case BackGroundType.DarkRoom:
            RuntimeManager.PlayOneShot(EnterDoorFromDark);
            break;
         case BackGroundType.SpaceRoom:
            RuntimeManager.PlayOneShot(EnterDoorFromSpace);
            break;
         case BackGroundType.AutomataRoom:
            RuntimeManager.PlayOneShot(EnterDoorFromAutomata);
            break;
      }
   }

   public void PlayExitDoor()
   {
      RuntimeManager.PlayOneShot(CloseDoorSFX);
      
      switch (bgMusicType)
      {
         case BackGroundType.GrassLands:
            RuntimeManager.PlayOneShot(ExitDoorInGrass);
            break;
         case BackGroundType.CaveLands:
            RuntimeManager.PlayOneShot(ExitDoorInCave);
            break;
         case BackGroundType.DarkRoom:
            RuntimeManager.PlayOneShot(ExitDoorInDark);
            break;
         case BackGroundType.SpaceRoom:
            RuntimeManager.PlayOneShot(ExitDoorInSpace);
            break;
         case BackGroundType.AutomataRoom:
            RuntimeManager.PlayOneShot(ExitDoorInAutomata);
            break;
      }
   }
   
   
   //Music Play
   
   public void PlayMenuMusic()
   {
      _musicInstance.stop(STOP_MODE.IMMEDIATE);
      _musicInstance = RuntimeManager.CreateInstance(MusicMainMenu);
      _musicInstance.start();
   }

   public void PlayLevelMusic()
   {
      _musicInstance.stop(STOP_MODE.IMMEDIATE);
      
      switch (bgMusicType)
      {
         case BackGroundType.GrassLands:
            _musicInstance = RuntimeManager.CreateInstance(MusicGrass);
            break;
            
         case BackGroundType.CaveLands:
            _musicInstance = RuntimeManager.CreateInstance(MusicCave);
            break;
         
         case BackGroundType.DarkRoom:
            _musicInstance = RuntimeManager.CreateInstance(MusicDark);
            break;
         case BackGroundType.SpaceRoom:
            _musicInstance = RuntimeManager.CreateInstance(MusicSpace);
            break;
         case BackGroundType.AutomataRoom:
            _musicInstance = RuntimeManager.CreateInstance(MusicAutomata);
            break;
      }
      _musicInstance.start();
   }

}
