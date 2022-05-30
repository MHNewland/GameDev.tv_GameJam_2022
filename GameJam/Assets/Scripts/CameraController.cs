using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject playerGO;
    private GameObject cam;
    

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        cam = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y, this.transform.position.z);
        constrainCamera();
    }

    private void constrainCamera()
    {
        if (cam.transform.position.y < -11) cam.transform.position = new Vector3(cam.transform.position.x, -11, cam.transform.position.z);
    }
}
