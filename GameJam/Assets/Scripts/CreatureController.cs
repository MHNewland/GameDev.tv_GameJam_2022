using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreatureController : MonoBehaviour
{

    MoveController move;
    float speed;
    float direction;
    float jumpPower;
    int spawnPointNum;

    [SerializeField]
    private GameObject[] spawnPoints;

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
            case "Spikes":
                {
                    RespawnCreature();
                    break;
                }
            case "Woorld border":
                {
                    RespawnCreature();
                    break;
                }
            case "Player":
                {
                    if (collision.gameObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D feet))
                    {
                        RespawnCreature();
                    }
                    break;
                }
            default:
                break;
        }

    }

    private void RespawnCreature()
    {
        spawnPointNum = Random.Range(0, spawnPoints.Length);
        transform.position = spawnPoints[spawnPointNum].transform.position;
    }
}
