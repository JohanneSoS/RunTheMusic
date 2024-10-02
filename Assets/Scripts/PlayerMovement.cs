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
    public SwapDoors[] swapDoorsScripts;

    public GameObject LastDoorEntered;
    
    private Animator anim;
    public bool lockedPlayerMovement;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void OnEnable()
    {
        EventManager.OnEnterDoorHover += OnEnterDoorHover;
        EventManager.OnLockPlayerMovement += OnLockPlayerMovement;
        EventManager.OnUnlockPlayerMovement += OnUnlockPlayerMovement;
    }
    void OnDisable()
    {
        EventManager.OnEnterDoorHover -= OnEnterDoorHover;
        EventManager.OnLockPlayerMovement -= OnLockPlayerMovement;
        EventManager.OnUnlockPlayerMovement -= OnUnlockPlayerMovement;
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
            EventManager.OnDash();
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
            EventManager.OnDoorEnter(levelScript.triggerBackground);
        }
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
        if (other.gameObject.TryGetComponent<SwapDoors>(out var door))
        {
            levelScript.triggerBackground = GetBackGroundTypeFromDoorTag(other.gameObject.tag);
            //EnterDoorTriggerState(door);
            OnEnterDoorHover(door);
            LastDoorEntered = other.gameObject;
        }
    }

    private void OnLockPlayerMovement()
    {
        Time.timeScale = 0f;
        lockedPlayerMovement = true;
    }

    private void OnUnlockPlayerMovement()
    {
        Time.timeScale = 1f;
        lockedPlayerMovement = false;
    }
    
    private void OnEnterDoorHover(SwapDoors door)
    {
        door.ActivateOutline(levelScript.triggerBackground);
    
        EventManager.OnChangeBlackScreenState(Blackscreen.BlackScreenState.Cinematic);
    }

    private BackGroundType GetBackGroundTypeFromDoorTag(string doorTag)
    {
        return doorTag switch
        {
            "CaveDoor" => BackGroundType.CaveLands,
            "DarkroomDoor" => BackGroundType.DarkRoom,
            "GrassDoor" => BackGroundType.GrassLands,
            "SpaceDoor" => BackGroundType.SpaceRoom,
            "AutomataDoor" => BackGroundType.AutomataRoom,
            _ => throw new ArgumentException("BackgroundType does not exist")
        };
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<SwapDoors>(out var door))
        {
            levelScript.triggerBackground = BackGroundType.None;
            door.DeactivateAllOutlines();
            
            EventManager.OnChangeBlackScreenState(Blackscreen.BlackScreenState.NoScreen);
        }
    }
}
