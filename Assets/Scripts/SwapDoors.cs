using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapDoors : MonoBehaviour
{
    [SerializeField] private GameObject GrassDoor;
    [SerializeField] private GameObject CaveDoor;
    [SerializeField] private GameObject DarkroomDoor;
    [SerializeField] private GameObject SpaceDoor;
    [SerializeField] private GameObject AutomataDoor;

    [SerializeField] private Material selectedDoorMaterial;

   // Update is called once per frame
    void Update()
    {
        switch (tag)
        {
            case ("GrassDoor"):
                GrassDoor.SetActive(true);
                CaveDoor.SetActive(false);
                DarkroomDoor.SetActive(false);
                SpaceDoor.SetActive(false);
                AutomataDoor.SetActive(false);
                break;
            
            case ("CaveDoor"):
                GrassDoor.SetActive(false);
                CaveDoor.SetActive(true);
                DarkroomDoor.SetActive(false);
                SpaceDoor.SetActive(false);
                AutomataDoor.SetActive(false);
                break;
            
            case ("DarkroomDoor"):
                GrassDoor.SetActive(false);
                CaveDoor.SetActive(false);
                DarkroomDoor.SetActive(true);
                SpaceDoor.SetActive(false);
                AutomataDoor.SetActive(false);
                break;
            
            case("SpaceDoor"):
                GrassDoor.SetActive(false);
                CaveDoor.SetActive(false);
                DarkroomDoor.SetActive(false);
                SpaceDoor.SetActive(true);
                AutomataDoor.SetActive(false);
                break; 
            
            case("AutomataDoor"):
                GrassDoor.SetActive(false);
                CaveDoor.SetActive(false);
                DarkroomDoor.SetActive(false);
                SpaceDoor.SetActive(false);
                AutomataDoor.SetActive(true);
                break;
        }
    }

    public void ActivateOutline(BackGroundType selectedDoor)
    {
        switch (selectedDoor)
        {
            case (BackGroundType.GrassLands):
                selectedDoorMaterial = GrassDoor.GetComponent<Renderer>().material;
                break;
            
            case (BackGroundType.DarkRoom):
                selectedDoorMaterial = DarkroomDoor.GetComponent<Renderer>().material;
                break;
            
            case (BackGroundType.CaveLands):
                selectedDoorMaterial = CaveDoor.GetComponent<Renderer>().material;
                break;
                
            case (BackGroundType.AutomataRoom):
                selectedDoorMaterial = AutomataDoor.GetComponent<Renderer>().material;
                break;
            
            case (BackGroundType.SpaceRoom):
                selectedDoorMaterial = SpaceDoor.GetComponent<Renderer>().material;
                break;
        }
        
        selectedDoorMaterial.SetFloat("_Thickness", 0.03f);
    }

    public void DeactivateAllOutlines()
    {
        var doorOutline = GrassDoor.GetComponent<Renderer>().material;
        doorOutline.SetFloat("_Thickness", 0.0f);
        doorOutline = CaveDoor.GetComponent<Renderer>().material;
        doorOutline.SetFloat("_Thickness", 0.0f);
        doorOutline = DarkroomDoor.GetComponent<Renderer>().material;
        doorOutline.SetFloat("_Thickness", 0.0f);
        doorOutline = SpaceDoor.GetComponent<Renderer>().material;
        doorOutline.SetFloat("_Thickness", 0.0f);
        doorOutline = AutomataDoor.GetComponent<Renderer>().material;
        doorOutline.SetFloat("_Thickness", 0.0f);
    }
}
