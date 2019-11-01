using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Using dynamic body, so same force as that of ship can be provided in reverse direction to prevent turret from rotating in opposite direction 
// Hence keep all drags and gravity scale at 0; (applicable on all turrets on player ship)

public class PlayerShipSideTurret : MonoBehaviour
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
    GameObject playerShipSideTurretPosition;

    //Rotation Angle Limits
    float minRotationDegAnglePos = 0f;
    float maxRotationDegAnglePos = 90f;

    float minRotationDegAngleNeg = -90f;
    float maxRotationDegAngleNeg = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerShip = transform.root.gameObject.GetComponent<PlayerShip>();
        playerShipSideTurretPosition = playerShip.transform.Find("PlayerShipBody").transform.Find("PlayerShipSideTurretPosition").gameObject;
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
        transform.position = playerShipSideTurretPosition.transform.position;

        //Debug.Log(mouseAngleInRad);
        LimitRotation();
    }

    private void LimitRotation()
    {
        return; //TODO
    }

    public void ShootFromSideTurret(int left=1)
    {
        //There is only one barrel that shoots, assumed to be left for the sake of generality
        bool isLeft = true;
        playerShip.ShootFromTurret(isLeft, "PlayerShipSideTurret");
    }
}
