using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGO;
    private Rigidbody2D playerRB;

    private float speed = 7;
    private float jumpForce = 7;

    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private bool doubleJump;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerRB = playerGO.GetComponent<Rigidbody2D>();
        }
        isGrounded = true;
        doubleJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)){
            playerRB.transform.Translate(Vector3.right * Time.deltaTime * speed);

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRB.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }


        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            if (isGrounded || doubleJump)
            {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJump = true;
        }
    }
}
