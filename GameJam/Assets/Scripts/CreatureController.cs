using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{

    MoveController move;
    float speed;
    float direction;
    float jumpPower;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<MoveController>();
        direction = 1;
        speed = 4;
        jumpPower = 10;
    }

    // Update is called once per frame
    void Update()
    {
        move.Run(direction, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Creature collistion: " + collision.tag);

        switch (collision.tag)
        {
            case "Jump":
                {
                    move.Jump(jumpPower);
                    break;
                }
            case "Turn":
                {
                    direction *= -1;
                    break;
                }
            default:
                break;
        }
    }
}
