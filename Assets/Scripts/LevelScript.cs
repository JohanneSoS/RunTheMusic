using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using FMOD;
using FMOD.Studio;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public PlayerMovement playerScript;
    //public Blackscreen blackScreenScript;

    [SerializeField] private GameObject Grasslands;
    [SerializeField] private GameObject Cavelands;
    [SerializeField] private GameObject Darkroom;
    [SerializeField] private GameObject Spaceroom;
    [SerializeField] private GameObject Automataroom;
    /*[SerializeField] private GameObject DoorToCave;
    [SerializeField] private GameObject DoorToDarkroom;
    [SerializeField] private GameObject DoorToGrassland;*/

    [SerializeField] Animator plattformAnim;
    [SerializeField] private List<Animator> plattforms;
    private int animPlattform;
    
    private Dictionary<BackGroundType, GameObject> backGroundDictionary;

    public BackGroundType bgType;
    public BackGroundType triggerBackground = BackGroundType.None;
    public BackGroundType lastBackground;

    
    void Start()
    {
        bgType = BackGroundType.GrassLands;
        backGroundDictionary = new Dictionary<BackGroundType, GameObject>()
        {
            { BackGroundType.CaveLands, Cavelands },
            { BackGroundType.DarkRoom, Darkroom },
            { BackGroundType.GrassLands, Grasslands },
            { BackGroundType.SpaceRoom, Spaceroom },
            { BackGroundType.AutomataRoom, Automataroom },
        };
        AudioPlayer.Instance.bgMusicType = bgType;
        AudioPlayer.Instance.PlayLevelMusic();
    }

    
    void Update()
    {
        switch (bgType)
        {
            case BackGroundType.GrassLands:
                foreach (var plattformAnim in plattforms)
                {
                    plattformAnim.SetInteger("plattform", 1);
                }
                break;
            case BackGroundType.CaveLands:
                foreach (var plattformAnim in plattforms)
                {
                    plattformAnim.SetInteger("plattform", 1);
                }
                break;
            case BackGroundType.DarkRoom:
                foreach (var plattformAnim in plattforms)
                {
                    plattformAnim.SetInteger("plattform", 3);
                }
                break;
            case BackGroundType.SpaceRoom:
                foreach (var plattformAnim in plattforms)
                {
                    plattformAnim.SetInteger("plattform", 4);
                }
                break;
            case BackGroundType.AutomataRoom:
                foreach (var plattformAnim in plattforms)
                {
                    plattformAnim.SetInteger("plattform", 5);
                }
                break;
        }

        AudioPlayer.Instance.bgMusicType = bgType;
    }

    public void EnterDoor()
    {
        //bgType = triggerBackground;
        triggerBackground = BackGroundType.None;

        foreach (var backGroundKeyValue in backGroundDictionary)
        {
            backGroundKeyValue.Value.SetActive(false);
            //backGroundKeyValue.Value.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
        }

        backGroundDictionary[bgType].SetActive(true);
        //backGroundDictionary[bgType].GetComponent<FMODUnity.StudioEventEmitter>().Play();
          
    }

    public void SwapDoors()
    {
        switch (lastBackground)
        {
            case BackGroundType.GrassLands:
                playerScript.LastDoorEntered.tag = "GrassDoor";
                break;
            //noch nicht komplett
            case BackGroundType.CaveLands:
                playerScript.LastDoorEntered.tag = "CaveDoor";
                break;
            case BackGroundType.DarkRoom:
                playerScript.LastDoorEntered.tag = "DarkroomDoor";
                break;
            case BackGroundType.SpaceRoom:
                playerScript.LastDoorEntered.tag = "SpaceDoor";
                break;
            case BackGroundType.AutomataRoom:
                playerScript.LastDoorEntered.tag = "AutomataDoor";
                break;
            
        }
        //playerScript.LastDoorEntered.tag = //Tür wird zu Tür vom letzten Hintergrund
        
        //
    }
   
}

public enum BackGroundType
{
    None,
    GrassLands,
    CaveLands,
    DarkRoom,
    SpaceRoom,
    AutomataRoom
}
