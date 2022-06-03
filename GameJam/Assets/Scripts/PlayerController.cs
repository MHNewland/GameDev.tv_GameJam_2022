using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 rayVector;
    private RaycastHit2D rightRay;
    private RaycastHit2D leftRay;
    private Rigidbody2D playerRB;
    private BoxCollider2D feet;
    private CapsuleCollider2D playerColider;
    private SpriteRenderer playerSprite;
    private Animator playerAnimator;
    private MoveController playerMoveController;

    [SerializeField]
    private ParticleSystem deathParticle;

    [SerializeField]
    private float moveSpeed = 5;

    [SerializeField]
    private float jumpForce = 13;

    [SerializeField]
    private GameObject playerGO;

    [SerializeField]
    private GameObject SpawnPoint;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private bool doubleJump;

    [SerializeField]
    public bool power_DJump { get; private set; }

    [SerializeField]
    public bool power_Float { get; private set; }

    [SerializeField]
    public bool power_Speed { get; private set; }

    [SerializeField]
    private float yVel;

    [SerializeField]
    private float xVel;

    [SerializeField]
    public bool isDead { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        SpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

        if (playerGO != null)
        {
            playerRB = playerGO.GetComponent<Rigidbody2D>();
            feet = playerGO.GetComponent<BoxCollider2D>();
            playerColider = playerGO.GetComponent<CapsuleCollider2D>();
            playerSprite = playerGO.GetComponent<SpriteRenderer>();
            playerAnimator = playerGO.GetComponent<Animator>();
            playerMoveController = playerGO.GetComponent<MoveController>();
        }

        isDead = false;
        isGrounded = true;
        doubleJump = false;
        power_DJump = false;
        power_Float = false;


        Physics.gravity = Physics.gravity * 100;

    }

    // Update is called once per frame
    void Update()
    {
        xVel = playerRB.velocity.x;
        if (!isDead)
        {
            Run();
            PlayerState();
            ConstrainSpeed();
        }
    }

    private void PlayerState()
    {

        if (playerRB.velocity.y < -.15f)
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
        if (isDead) { return; }
        moveInput = value.Get<Vector2>();
        if (power_Speed) moveInput *=2f;
    }

    void Run()
    {
        if (isDead) { return; }

        playerMoveController.Run(moveInput.x, moveSpeed);

    }

    void OnJump(InputValue value)
    {
        if (isDead) { return; }
        if (value.isPressed)
        {
            if (isGrounded || (power_DJump && doubleJump))
            {
                playerAnimator.SetBool("IsJumping", true);
                playerAnimator.SetBool("IsFalling", false);
                playerMoveController.Jump(jumpForce);
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
        switch (collision.collider.tag)
        {
            case ("Death"):
                {
                    if(collision.collider.TryGetComponent<Tilemap>( out Tilemap tile))
                    {
                        switch (tile.name)
                        {
                            case "Spikes":
                                {
                                    power_Float = true;
                                    break;
                                }
                            case "World border":
                                {
                                    power_DJump = true;
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    StartCoroutine(Die());
                    break;
                }
            case ("Creature"):
                {
                    
                    power_Speed = true;
                    StartCoroutine(Die());
                    break;
                }
            default:
                break;
        }

    }

    //fix glitch of jumping into a wall and not hitting the ground properly.
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
        switch (collision.tag)
        {
            case "Ground":
                {
                    if (feet.IsTouchingLayers(LayerMask.GetMask("Ground")))
                    {
                        playerAnimator.SetBool("IsJumping", false);
                        isGrounded = true;
                        if (power_DJump) doubleJump = true;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

    private IEnumerator Die()
    {

        isDead = true;
        deathParticle.Play();
        playerRB.velocity = Vector3.zero;
        playerRB.bodyType = RigidbodyType2D.Static;
        playerGO.transform.Rotate(new Vector3(0, 90, 0));

        yield return new WaitForSeconds(1);

        playerGO.transform.position = SpawnPoint.transform.position;
        playerGO.transform.Rotate(new Vector3(0, -90, 0));
        playerRB.bodyType = RigidbodyType2D.Dynamic;
        playerRB.velocity = Vector3.zero;
        isDead = false;
        moveInput = Vector2.zero;



    }
}
