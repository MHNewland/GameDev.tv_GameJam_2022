using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hidden : MonoBehaviour
{
    private float r;
    private float g;
    private float b;
    private Tilemap tm;
    private bool isHidden;
    private TilemapCollider2D hiddenTiles;

    private Color normalColor = new Color(1, 1, 1, 1);
    private Color transColor = new Color(1, 1, 1, (177 / 255f));

    private GameObject player;


    private void Start()
    {
        tm = GetComponent<Tilemap>();
        hiddenTiles = GetComponent<TilemapCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        r = tm.color.r;
        g = tm.color.g;
        b = tm.color.b;
        isHidden = true;
    }

    private void Update()
    {
        if (isHidden)
            tm.color = normalColor;
        else
            tm.color = transColor;

        if (player.GetComponent<PlayerController>().isDead)
        {
            isHidden = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) isHidden = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isHidden = true;
    }
}
