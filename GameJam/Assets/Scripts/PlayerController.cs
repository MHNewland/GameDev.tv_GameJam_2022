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
    private float maxSpeed = 10;
    private float jumpForce = 7;

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
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRB.AddForce(Vector2.right * acceleration);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRB.AddForce(Vector2.left * acceleration);
        }


        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (isGrounded || (power_DJump && doubleJump))
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
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

        if (power_Float) { 
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) {

                Physics.gravity = Physics.gravity * .25f;
                if (playerRB.velocity.y < -3)
                {
                    playerRB.velocity = new Vector3(playerRB.velocity.x, -3);
                }
            }
            else
            {
                Physics.gravity = defaultGrav;
            }
        }

        if(Input.GetKeyUp(KeyCode.RightArrow) && xVel > 0)
        {
            playerRB.AddForce(Vector2.left * xVel, ForceMode2D.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) && xVel < 0)
        {
            playerRB.AddForce(Vector2.right * -xVel, ForceMode2D.Impulse);
        }

        ConstrainSpeed();

    }

    private void ConstrainSpeed()
    {
        xVel = playerRB.velocity.x;
        yVel = playerRB.velocity.y;

        if (xVel > maxSpeed)
        {
            xVel = maxSpeed;
        }

        if (xVel < -maxSpeed)
        {
            xVel = -maxSpeed;
        }

        if (yVel > maxSpeed)
        {
            yVel = maxSpeed;
        }

        playerRB.velocity = new Vector2(xVel, yVel);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            if(power_DJump) doubleJump = true;
        }
    }
}
