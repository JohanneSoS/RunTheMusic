using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    //[SerializeField] PlayerMovement background;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableBackground()
    {
        gameObject.SetActive(false);
    }

    public void EnableBackground()
    {
        gameObject.SetActive(true);     
    }
    
}
