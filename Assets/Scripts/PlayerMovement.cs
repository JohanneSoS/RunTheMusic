using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private float horizontal;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpingPower = 10f;
    [SerializeField] private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public LevelScript levelScript;

    public GameObject LastDoorEntered;
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        Flip();
      
        if (horizontal != 0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);   
        }
        
        if (rb.velocity.y != 0f)
        { 
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

        if (levelScript.bgType == BackGroundType.SpaceRoom)
        {
            anim.SetBool("IsSpaceShip", true);
            rb.gravityScale = 0.5f;
            rb.mass = 0.5f;
            jumpingPower = 5.5f;
            speed = 5f;
        }
        else
        {
            anim.SetBool("IsSpaceShip", false);
            rb.gravityScale = 2f;
            rb.mass = 1f;
            jumpingPower = 10f;
            speed = 6f;
        }

        if (levelScript.bgType == BackGroundType.AutomataRoom)
        {
            anim.SetBool("IsRobot", true);
        }
        else
        {
            anim.SetBool("IsRobot", false);
        }

        if (levelScript.triggerBackground != BackGroundType.None &&
            (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Return)))
        {
            levelScript.lastBackground = levelScript.bgType;
            levelScript.bgType = levelScript.triggerBackground;
            //make the Player freeze midair, so he waits for the entire 4secs and stays on the colider
            
            /*switch (levelScript.lastBackground)
            {case BackGroundType.GrassLands:}*/
            AudioPlayer.Instance.StopMusic();
            AudioPlayer.Instance.PlayEnterDoor();
            StartCoroutine(EnterRoom());
            Time.timeScale = 0f;
        }
    }

    IEnumerator EnterRoom()
    {
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1f;
        levelScript.EnterDoor();
        levelScript.SwapDoors();
        Debug.Log("Resume");
        AudioPlayer.Instance.PlayExitDoor();
        yield return new WaitForSecondsRealtime(1);
        AudioPlayer.Instance.PlayLevelMusic();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }
    
    private void Flip() 
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localeScale = transform.localScale;
            localeScale.x *= -1f;
            transform.localScale = localeScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
            if (other.gameObject.CompareTag("CaveDoor"))
            {
                LastDoorEntered = other.gameObject;
                levelScript.triggerBackground = BackGroundType.CaveLands;
            }
            else if (other.gameObject.CompareTag("DarkroomDoor"))
            {
                LastDoorEntered = other.gameObject;
                levelScript.triggerBackground = BackGroundType.DarkRoom;
            }
            else if (other.gameObject.CompareTag("GrassDoor"))
            {
                LastDoorEntered = other.gameObject;
                levelScript.triggerBackground = BackGroundType.GrassLands;
            }
            else if (other.gameObject.CompareTag("SpaceDoor"))
            {
                LastDoorEntered = other.gameObject;
                levelScript.triggerBackground = BackGroundType.SpaceRoom;
            }
            else if (other.gameObject.CompareTag("AutomataDoor"))
            {
                LastDoorEntered = other.gameObject;
                levelScript.triggerBackground = BackGroundType.AutomataRoom;
            }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        levelScript.triggerBackground = BackGroundType.None;
    }
    
}
