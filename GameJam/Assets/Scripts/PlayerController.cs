using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 rightRayVector;
    private Vector2 leftRayVector;
    private RaycastHit2D rightRay;
    private RaycastHit2D leftRay;
    private Rigidbody2D playerRB;
    private BoxCollider2D feet;
    private CapsuleCollider2D playerColider;
    private SpriteRenderer playerSprite;
    private Animator playerAnimator;

    private float moveSpeed = 5;
    private float jumpForce = 13;

    [SerializeField]
    private GameObject playerGO;

    [SerializeField]
    private GameObject[] SpawnPoints;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private bool doubleJump;

    [SerializeField]
    private bool power_DJump;

    [SerializeField]
    private bool power_Float;

    [SerializeField]
    private float yVel;

    [SerializeField]
    private float xVel;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerRB = playerGO.GetComponent<Rigidbody2D>();
            feet = playerGO.GetComponent<BoxCollider2D>();
            playerColider = playerGO.GetComponent<CapsuleCollider2D>();
            playerSprite = playerGO.GetComponent<SpriteRenderer>();
            playerAnimator = playerGO.GetComponent<Animator>();
        }

        isGrounded = true;
        doubleJump = false;
        power_DJump = false;
        power_Float = false;


        Physics.gravity = Physics.gravity * 100;

    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        PlayerState();
        ConstrainSpeed();
    }

    private void PlayerState()
    {

        if(playerRB.velocity.y < -.15f)
        {
            playerAnimator.SetBool("IsFalling", true);
        }
        else
        {
            playerAnimator.SetBool("IsFalling", false);
        }

    }

    

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        xVel = playerRB.velocity.x;
        rightRayVector = new Vector2(playerGO.transform.position.x, playerGO.transform.position.y);
        rightRay = Physics2D.Raycast(rightRayVector, Vector2.right, 0.3f);

        Debug.DrawRay(rightRayVector, Vector2.right * 0.3f, Color.red);
        if (rightRay.collider != null)
        {
            if (!rightRay.collider.CompareTag("Ground"))
            {
                Debug.Log("not ground");
                playerRB.velocity = new Vector2(moveInput.x * moveSpeed, playerRB.velocity.y);
            }
            else
            {
                Debug.Log(rightRay.collider.tag);
            }
        }
        else
        {
            Debug.Log("null");
            playerRB.velocity = new Vector2(moveInput.x * moveSpeed, playerRB.velocity.y);

        }

    }
    
    void FlipSprite()
    {
        bool playerHasXVel = playerRB.velocity.x != 0;
        if (playerHasXVel)
        {
            playerSprite.flipX = playerRB.velocity.x < 0;
            playerAnimator.SetBool("IsRunning", true);
        }
        else
        {
            playerAnimator.SetBool("IsRunning", false);
        }
    }

    void OnJump(InputValue value)
    {
     
        if (value.isPressed)
        {
            if (isGrounded || (power_DJump && doubleJump))
            {
                playerAnimator.SetBool("IsJumping", true);
                playerAnimator.SetBool("IsFalling", false);
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                if (isGrounded)
                {
                    isGrounded = false;
                }
                else
                {
                    doubleJump = false;
                }
            }
        }
    }

    private void ConstrainSpeed()
    {
        yVel = playerRB.velocity.y;

        if(power_Float && playerRB.velocity.y < -3)
        {
            yVel = -3;
        }

        playerRB.velocity = new Vector2(playerRB.velocity.x, yVel);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (feet.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                playerAnimator.SetBool("IsJumping", false);
                isGrounded = true;
                if (power_DJump) doubleJump = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isGrounded && collision.collider.CompareTag("Ground"))
        {
            if (feet.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                playerAnimator.SetBool("IsJumping", false);
                isGrounded = true;
                if (power_DJump) doubleJump = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            int spID = (int)Math.Round((double)(UnityEngine.Random.Range(0, SpawnPoints.Length)),1);
            playerGO.transform.position = SpawnPoints[spID].transform.position;
            playerRB.velocity = Vector2.zero;
        }
    }


}
