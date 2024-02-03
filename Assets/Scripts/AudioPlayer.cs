using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioPlayer : MonoBehaviour
{
   public LevelScript levelScript;
   [SerializeField] private EventReference EnterDoorFromGrass;
   [SerializeField] private EventReference EnterDoorFromCave;
   [SerializeField] private EventReference EnterDoorFromDark;
   [SerializeField] private EventReference EnterDoorFromSpace;
   [SerializeField] private EventReference EnterDoorFromAutomata;
   
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
   
   
   //General SFX
   
   public void PlayButtonClickSFX()
   {
      RuntimeManager.PlayOneShot(ButtonClickSFX);
   }
   
   public void PlayOpenDoorSFX()
   {
      RuntimeManager.PlayOneShot(OpenDoorSFX);
   }
   
   public void PlayCloseDoorSFX()
   {
      RuntimeManager.PlayOneShot(CloseDoorSFX);
   }
   
   
   //Music Oneshots

   public void PlayEnterDoorFromGrass()
   {
      RuntimeManager.PlayOneShot(EnterDoorFromGrass);
   }
   
   public void PlayEnterDoorFromCave()
   {
      RuntimeManager.PlayOneShot(EnterDoorFromCave);
   }
   
   public void PlayEnterDoorFromDark()
   {
      RuntimeManager.PlayOneShot(EnterDoorFromDark);
   }
   
   public void PlayEnterDoorFromSpace()
   {
      RuntimeManager.PlayOneShot(EnterDoorFromSpace);
   }
   
   public void PlayEnterDoorFromAutomata()
   {
      RuntimeManager.PlayOneShot(EnterDoorFromAutomata);
   }
   
   
   
   //Music Play
   
   public void PlayMusicMainMenu()
   {
      _musicInstance.stop(STOP_MODE.IMMEDIATE);
      _musicInstance = RuntimeManager.CreateInstance(MusicMainMenu);
      _musicInstance.start();
   }

   public void PlayLevelMusic()
   {
      _musicInstance.stop(STOP_MODE.IMMEDIATE);
      switch (levelScript.bgType)
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
