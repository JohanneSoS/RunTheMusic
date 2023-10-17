using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePortalScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisablePortal()
    {
        gameObject.SetActive(false);
    }
}
