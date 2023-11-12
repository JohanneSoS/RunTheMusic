using System;
using System.Collections;
using System.Collections.Generic;
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

    /*public BackGroundType bgType;
    
    [SerializeField] private GameObject Grasslands;
    [SerializeField] private GameObject Cavelands;
    [SerializeField] private GameObject Darkroom;
    private Dictionary<BackGroundType, GameObject> backGroundDictionary;

    private BackGroundType triggerBackground = BackGroundType.None;*/

    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        /*bgType = BackGroundType.GrassLands;
        backGroundDictionary = new Dictionary<BackGroundType, GameObject>()
        {
            { BackGroundType.CaveLands, Cavelands },
            { BackGroundType.DarkRoom, Darkroom },
            { BackGroundType.GrassLands, Grasslands }
        };*/
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

        if (levelScript.triggerBackground != BackGroundType.None &&
            (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Return)))
        {
            levelScript.EnterDoor();
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
                levelScript.triggerBackground = BackGroundType.CaveLands;
            }
            else if (other.gameObject.CompareTag("DarkroomDoor"))
            {
                levelScript.triggerBackground = BackGroundType.DarkRoom;
            }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        levelScript.triggerBackground = BackGroundType.None;
    }

    
    /*public enum BackGroundType
    {
        None,
        GrassLands,
        CaveLands,
        DarkRoom
    }*/
    
    
}
