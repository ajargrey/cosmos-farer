﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipTurret : MonoBehaviour
{

    //Mouse Variables
    float initialAngleDeflection = 90f;

    //Rotation
    float rotationVelocity = 300f;
    float defaultRotationVelocity = 300f;

    //Master Parent
    PlayerShip playerShip;

    //Position Point Brother Object
    GameObject playerShipTurretPosition;


    // Start is called before the first frame update
    void Start()
    {
        playerShip = transform.root.gameObject.GetComponent<PlayerShip>();
        playerShipTurretPosition = playerShip.transform.Find("PlayerShipBody").transform.Find("PlayerShipTurretPosition").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        getMouseAngleAndSetRotationVelocity();
    }

    private void getMouseAngleAndSetRotationVelocity()
    {
        Vector2 mousePosInWorldUnits = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseAngleInRad = Mathf.Atan2((mousePosInWorldUnits.y - transform.position.y), (mousePosInWorldUnits.x - transform.position.x));
        float finalAngleInDeg = mouseAngleInRad * Mathf.Rad2Deg - initialAngleDeflection ;
        var targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, mouseAngleInRad*Mathf.Rad2Deg - initialAngleDeflection ) ;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationVelocity * Time.deltaTime) ;
        if(GetComponent<Rigidbody2D>().angularVelocity > 0  && playerShip.GetComponent<Rigidbody2D>().angularVelocity > 0)
        {
            rotationVelocity = defaultRotationVelocity - playerShip.GetComponent<Rigidbody2D>().angularVelocity;
        }
        else if (GetComponent<Rigidbody2D>().angularVelocity > 0 && playerShip.GetComponent<Rigidbody2D>().angularVelocity < 0)
        {
            rotationVelocity = defaultRotationVelocity - playerShip.GetComponent<Rigidbody2D>().angularVelocity;
        }
        else if (GetComponent<Rigidbody2D>().angularVelocity < 0 && playerShip.GetComponent<Rigidbody2D>().angularVelocity > 0)
        {
            rotationVelocity = defaultRotationVelocity - playerShip.GetComponent<Rigidbody2D>().angularVelocity;
        }
        else if (GetComponent<Rigidbody2D>().angularVelocity < 0 && playerShip.GetComponent<Rigidbody2D>().angularVelocity < 0)
        {
            rotationVelocity = defaultRotationVelocity - playerShip.GetComponent<Rigidbody2D>().angularVelocity;
        }
        // Debug.Log(playerShip.GetComponent<Rigidbody2D>().angularVelocity + " " + targetRotation.z);
        transform.position = playerShipTurretPosition.transform.position;

    }

    public void ShootFromTurret(int left)
    {
        bool isLeft = true;
        if(left==0)
        {
            isLeft = false;
        }
        if(left==1)
        {
            isLeft = true;
        }
        playerShip.ShootFromTurret(isLeft, "PlayerShipTurret");
    }
}
