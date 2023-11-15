using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapDoors : MonoBehaviour
{
    [SerializeField] private GameObject GrassDoor;
    [SerializeField] private GameObject CaveDoor;
    [SerializeField] private GameObject DarkroomDoor;

    //public BackGroundType doorType;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (tag)
        {
            case ("GrassDoor"):
                GrassDoor.SetActive(true);
                CaveDoor.SetActive(false);
                DarkroomDoor.SetActive(false);
                break;
            
            case ("CaveDoor"):
                GrassDoor.SetActive(false);
                CaveDoor.SetActive(true);
                DarkroomDoor.SetActive(false);
                break;
            
            case ("DarkroomDoor"):
                GrassDoor.SetActive(false);
                CaveDoor.SetActive(false);
                DarkroomDoor.SetActive(true);
                break;
        } 
    }
}
