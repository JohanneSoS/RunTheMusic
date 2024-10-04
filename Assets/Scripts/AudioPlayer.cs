using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioPlayer : MonoBehaviour
{
   //public BackGroundType bgMusicType;
   public static AudioPlayer Instance;

   public enum Sound
   {
      EnterDoor = 1,
      ExitDoor = 2,
      ButtonClickSFX = 3,
      OpenDoorSFX = 4,
      CloseDoorSFX = 5,
      MusicMainMenu = 6,
      MusicGrass = 7,
      MusicCave = 8,
      MusicDark = 9,
      MusicSpace = 10,
      MusicAutomata = 11,
      Dash = 12
   }
   
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
   
   //Play Logic
   
   [Serializable]
   public class SoundItem
   {
      public Sound sound;
      public EventReference EventReference;
   }

   public class LoopItem
   {
      public Sound Sound;
      public EventInstance EventInstance;

      public LoopItem(Sound sound, EventReference eventReference)
      {
         Sound = sound;
         EventInstance = RuntimeManager.CreateInstance(eventReference);
         StartLoop();
      }

      public void StartLoop()
      {
         this.EventInstance.start();
      }

      public void StopLoop()
      {
         EventInstance.stop(STOP_MODE.ALLOWFADEOUT);
      }

      public void Destroy()
      {
         EventInstance.stop(STOP_MODE.IMMEDIATE);
         EventInstance.release();
      }
   }

   [SerializeField] private List<SoundItem> soundItems;
   private List<LoopItem> loopQueue = new();

   
   private void PlayOneShot(Sound sound)
   {
      EventReference? eventReference = GetEventReference(sound);
      if (eventReference.HasValue)
         FMODUnity.RuntimeManager.PlayOneShot(eventReference.Value, Camera.main.transform.position);
   }

   private EventReference? GetEventReference(Sound sound)
   {
      SoundItem soundItem = soundItems.FirstOrDefault((soundItem) => soundItem.sound == sound);
      if (soundItem == null) return null;
      return soundItem.EventReference;
   }

//Starts a Loop
   private void StartLoop(Sound sound)
   {
      if (loopQueue.Any((item => item.Sound == sound)))
      {
         loopQueue.First(item => item.Sound == sound).StartLoop();
         Debug.Log("Started Loop Again");
         return;
      }

      EventReference? eventReference = GetEventReference(sound);
      if (!eventReference.HasValue) return;
      LoopItem loopItem = new LoopItem(sound, eventReference.Value);
      loopQueue.Add(loopItem);
   }

//Stops a Loop
   private void StopLoop(Sound sound)
   {
      if (!loopQueue.Any((item => item.Sound == sound))) return;
      loopQueue.First((item => item.Sound == sound)).StopLoop();
   }

   private void StopLoops()
   {
      foreach (var loopItem in loopQueue)
      {
         loopItem.StopLoop();
      }
   }

   private void OnDestroy()
   {
      //Clean UP
      foreach (var loopItem in loopQueue)
      {
         loopItem.Destroy();
      }
   }

   void OnEnable()
   {
      EventManager.OnButtonClick += OnButtonClick;
      EventManager.OnDoorEnter += OnDoorEnter;
      EventManager.OnDoorLeave += OnDoorLeave;
      EventManager.OnPlayMenuMusic += OnPlayMenuMusic;
      EventManager.OnPlayLevelMusic += OnPlayLevelMusic;
      EventManager.OnDash += OnDash;
   }
   
   void OnDisable()
   {
      EventManager.OnButtonClick -= OnButtonClick;
      EventManager.OnDoorEnter -= OnDoorEnter;
      EventManager.OnDoorLeave -= OnDoorLeave;
      EventManager.OnPlayMenuMusic -= OnPlayMenuMusic;
      EventManager.OnPlayLevelMusic -= OnPlayLevelMusic;
      EventManager.OnDash -= OnDash;
   }
   
   void OnButtonClick ()
   {
      PlayOneShot(Sound.ButtonClickSFX);
   }
   
   void OnDoorEnter (BackGroundType bgMusicType)
   {
      StopLoops();
      PlayOneShot(Sound.OpenDoorSFX);
      
      switch (bgMusicType)
      {
         case BackGroundType.GrassLands:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("LastBackground", 0);
            break;
         case BackGroundType.CaveLands:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("LastBackground", 1);
            break;
         case BackGroundType.DarkRoom:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("LastBackground", 2);
            break;
         case BackGroundType.SpaceRoom:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("LastBackground", 3);
            break;
         case BackGroundType.AutomataRoom:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("LastBackground", 4);
            break;
      }
      PlayOneShot(Sound.EnterDoor);
   }

   void OnDoorLeave(BackGroundType bgMusicType)
   {
      PlayOneShot(Sound.CloseDoorSFX);

      switch (bgMusicType)
      {
         case BackGroundType.GrassLands:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CurrentBackground", 0);
            break;
         case BackGroundType.CaveLands:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CurrentBackground", 1);
            break;
         case BackGroundType.DarkRoom:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CurrentBackground", 2);
            break;
         case BackGroundType.SpaceRoom:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CurrentBackground", 3);
            break;
         case BackGroundType.AutomataRoom:
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CurrentBackground", 4);
            break;
      }
      PlayOneShot(Sound.ExitDoor);
   }
   
   //Music Play
   void OnPlayMenuMusic()
   {
      StopLoops();
      StartLoop(Sound.MusicMainMenu);
   }

   void OnPlayLevelMusic(BackGroundType bgType)
   {
      StopLoops();
      
      switch (bgType)
      {
         case BackGroundType.GrassLands:
            StartLoop(Sound.MusicGrass);
            break;
            
         case BackGroundType.CaveLands:
            StartLoop(Sound.MusicCave);
            break;
         
         case BackGroundType.DarkRoom:
            StartLoop(Sound.MusicDark);
            break;
         case BackGroundType.SpaceRoom:
            StartLoop(Sound.MusicSpace);
            break;
         case BackGroundType.AutomataRoom:
            StartLoop(Sound.MusicAutomata);
            break;
      }
   }

   void OnDash()
   {
      PlayOneShot(Sound.Dash);
   }
}
