using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    public void DisableBackground()
    {
        gameObject.SetActive(false);
    }

    public void EnableBackground()
    {
        gameObject.SetActive(true);     
    }
    
}
