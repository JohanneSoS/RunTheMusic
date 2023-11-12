using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    //public PlayerMovement playerScript;
    public BackGroundType bgType;
    
    [SerializeField] private GameObject Grasslands;
    [SerializeField] private GameObject Cavelands;
    [SerializeField] private GameObject Darkroom;
    
    private Dictionary<BackGroundType, GameObject> backGroundDictionary;

    public BackGroundType triggerBackground = BackGroundType.None;
    
    // Start is called before the first frame update
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

    // Update is called once per frame
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
}
public enum BackGroundType
{
    None,
    GrassLands,
    CaveLands,
    DarkRoom
}
