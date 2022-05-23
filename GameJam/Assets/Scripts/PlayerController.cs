using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGO;
    private Rigidbody2D playerRB;

    private float acceleration = 20;
    private float maxXSpeed = 5;
    private float maxVSpeed = 10;
    private float jumpForce = 10;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private bool doubleJump;

    [SerializeField]
    private bool power_DJump;

    [SerializeField]
    private bool power_Float;

    private Vector3 defaultGrav = Physics.gravity;

    [SerializeField]
    private float xVel;

    [SerializeField]
    private float yVel;

    Vector3 newPosition;
    Vector3 prevPos, rayPos;


    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerRB = playerGO.GetComponent<Rigidbody2D>();
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
        HandleInput();
        ConstrainSpeed();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRB.AddForce(Vector2.right * acceleration);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) && xVel > 0)
        {
            playerRB.AddForce(Vector2.left * xVel, ForceMode2D.Impulse);
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRB.AddForce(Vector2.left * acceleration);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) && xVel < 0)
        {
            playerRB.AddForce(Vector2.right * -xVel, ForceMode2D.Impulse);
        }


        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (isGrounded || (power_DJump && doubleJump))
            {
                //playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                if (!isGrounded)
                {
                    doubleJump = false;
                }
                else
                {
                    isGrounded = false;
                }
            }
        }
    }

    private void ConstrainSpeed()
    {
        xVel = playerRB.velocity.x;
        yVel = playerRB.velocity.y;

        if (xVel > maxXSpeed)
        {
            xVel = maxXSpeed;
        }

        if (xVel < -maxXSpeed)
        {
            xVel = -maxXSpeed;
        }

        if (yVel > maxVSpeed)
        {
            yVel = maxVSpeed;
        }

        if(power_Float && yVel < -3)
        {
            yVel = -3;
        }

        playerRB.velocity = new Vector2(xVel, yVel);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            if (power_DJump) doubleJump = true;
        }
    }

}
