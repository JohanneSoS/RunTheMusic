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

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    public LevelScript levelScript;
    public Blackscreen blackScreenScript;
    public SwapDoors[] swapDoorsScripts;

    public GameObject LastDoorEntered;
    private Animator anim;

    private bool lockedPlayerMovement;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (isDashing)
        {
            return;
        }
        
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
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

        if (levelScript.triggerBackground != BackGroundType.None)
        {
            foreach (var swapDoor in swapDoorsScripts)
            {
                swapDoor.ActivateOutline(levelScript.triggerBackground);
            }
            blackScreenScript.BlackScreen = Blackscreen.BlackScreenState.Cinematic;
            blackScreenScript.ChangeBlackScreenState();
        }

        else if (lockedPlayerMovement == false)
        {
            foreach (var swapDoor in swapDoorsScripts)
            {
                swapDoor.DeactivateAllOutlines();
            } 
            blackScreenScript.BlackScreen = Blackscreen.BlackScreenState.NoScreen;
            blackScreenScript.ChangeBlackScreenState();
        }
        
        if (levelScript.triggerBackground != BackGroundType.None &&
            (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Return)))
        {
            //blackScreenScript.BlackScreen = Blackscreen.BlackScreenState.BlackScreen;
            //blackScreenScript.ChangeBlackScreenState();
            levelScript.lastBackground = levelScript.bgType;
            levelScript.bgType = levelScript.triggerBackground;
            AudioPlayer.Instance.StopMusic();
            AudioPlayer.Instance.PlayEnterDoor();
            StartCoroutine(EnterRoom());
            Time.timeScale = 0f;
            lockedPlayerMovement = true;
        }
    }

    IEnumerator EnterRoom()
    {
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1f;
        lockedPlayerMovement = false;
        levelScript.EnterDoor();
        levelScript.SwapDoors();
        Debug.Log("Resume");
        AudioPlayer.Instance.PlayExitDoor();
        yield return new WaitForSecondsRealtime(1);
        blackScreenScript.BlackScreen = Blackscreen.BlackScreenState.NoScreen;
        blackScreenScript.ChangeBlackScreenState();
        AudioPlayer.Instance.PlayLevelMusic();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        
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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
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
