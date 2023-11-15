using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public PlayerMovement playerScript;

    [SerializeField] private GameObject Grasslands;
    [SerializeField] private GameObject Cavelands;
    [SerializeField] private GameObject Darkroom;
    /*[SerializeField] private GameObject DoorToCave;
    [SerializeField] private GameObject DoorToDarkroom;
    [SerializeField] private GameObject DoorToGrassland;*/
    
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
            { BackGroundType.GrassLands, Grasslands }
        };
        backGroundDictionary[bgType].GetComponent<FMODUnity.StudioEventEmitter>().Stop();
        backGroundDictionary[bgType].GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    
    void Update()
    {
        
    }

    public void EnterDoor()
    {
        bgType = triggerBackground;
        triggerBackground = BackGroundType.None;

        foreach (var backGroundKeyValue in backGroundDictionary)
        {
            backGroundKeyValue.Value.SetActive(false);
            backGroundKeyValue.Value.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
        }
            
        backGroundDictionary[bgType].SetActive(true);
        backGroundDictionary[bgType].GetComponent<FMODUnity.StudioEventEmitter>().Play();
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
    DarkRoom
}
