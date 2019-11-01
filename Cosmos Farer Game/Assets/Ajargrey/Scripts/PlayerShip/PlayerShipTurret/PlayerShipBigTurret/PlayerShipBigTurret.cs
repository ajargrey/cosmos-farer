using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBigTurret : MonoBehaviour
{

    //Mouse Variables
    float initialAngleDeflection = 90f;

    //Rotation
    float rotationVelocity = 300f;
    float defaultRotationVelocity = 300f;
    Quaternion targetRotation;


    //Master Parent
    PlayerShip playerShip;

    //Position Point Brother Object
    GameObject playerShipBigTurretPosition;

    //Rotation Angle Limits
    float minRotationDegAnglePos = 45f;
    float maxRotationDegAnglePos = 180f;

    float minRotationDegAngleNeg = -180f;
    float maxRotationDegAngleNeg = -75f;

    // Start is called before the first frame update
    void Start()
    {
        playerShip = transform.root.gameObject.GetComponent<PlayerShip>();
        playerShipBigTurretPosition = playerShip.transform.Find("PlayerShipBody").transform.Find("PlayerShipBigTurretPosition").gameObject;
        targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0 - initialAngleDeflection);
    }

    // Update is called once per frame
    void Update()
    {
        getMouseAngleAndSetRotationVelocity();
    }

    private void getMouseAngleAndSetRotationVelocity()
    {
        LimitRotation();

        Vector2 mousePosInWorldUnits = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseAngleInRad = Mathf.Atan2((mousePosInWorldUnits.y - transform.position.y), (mousePosInWorldUnits.x - transform.position.x));
        float finalAngleInDeg = mouseAngleInRad * Mathf.Rad2Deg - initialAngleDeflection;
        targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, mouseAngleInRad * Mathf.Rad2Deg - initialAngleDeflection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationVelocity * Time.deltaTime);
        if (GetComponent<Rigidbody2D>().angularVelocity > 0 && playerShip.GetComponent<Rigidbody2D>().angularVelocity > 0)
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
        transform.position = playerShipBigTurretPosition.transform.position;

        //Debug.Log(mouseAngleInRad);
        LimitRotation();
    }

    private void LimitRotation()
    {
        return; //TODO
    }

    public void ShootFromBigTurret(int left)
    {
        bool isLeft = true;
        if (left == 0)
        {
            isLeft = false;
        }
        if (left == 1)
        {
            isLeft = true;
        }
        playerShip.ShootFromTurret(isLeft, "PlayerShipBigTurret");
    }
}
