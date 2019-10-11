using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn1 : MonoBehaviour
{

    //Input States
    bool upPressed = false;
    bool downPressed = false;
    bool leftPressed = false;
    bool rightPressed = false;

    //Movement Variables
    float moveForwardSpeed = 0.2f;
    float moveBackwardSpeed = 0.1f;

    //Rotation Variables
    float rotationSpeed = 100f;

    //Automatic Rotation Variables
    float automaticRotationSpeed = 5f;

    //Component Variables
    Rigidbody2D rigidBody;

    //Children Variables
    GameObject head;

    //Head Rotation Variables
    float headRotationSpeed = 20f;
    float initialDeflectionInDeg = 90f;
    float headRotationThresholdInDeg = 30f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        head = gameObject.transform.Find("PlayerPawn1Body").gameObject.transform.Find("PlayerPawn1Head").gameObject;
        print(head);
    }

    // Update is called once per frame
    void Update()
    {
        TakeInput();
        Move();
        HeadRotationControl();
        AutomatedBodyRotationControl();
    }

    private void AutomatedBodyRotationControl()
    {
        //Debug.Log( (head.transform.rotation.z - transform.rotation.z) );
        if (Mathf.Abs(head.transform.eulerAngles.z - transform.eulerAngles.z) > headRotationThresholdInDeg)
        {
            Vector2 mousePosInWorldUnits = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float mouseAngleInRad = Mathf.Atan2((mousePosInWorldUnits.y - head.transform.position.y), (mousePosInWorldUnits.x - head.transform.position.x));
            float finalAngleInDeg = mouseAngleInRad * Mathf.Rad2Deg - initialDeflectionInDeg;
            var targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, finalAngleInDeg);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, automaticRotationSpeed*Time.timeScale);
            //Debug.Log("Should Initialise automated head rotation");
            //head.transform.eulerAngles = new Vector3(head.transform.eulerAngles.x, head.transform.eulerAngles.y, transform.eulerAngles.z);
        }

    }

    private void Move()
    {
        ControlLinearMotion();
        ControlRotation();
    }

    private void HeadRotationControl()
    {
        Vector2 mousePosInWorldUnits = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseAngleInRad = Mathf.Atan2((mousePosInWorldUnits.y - head.transform.position.y), (mousePosInWorldUnits.x - head.transform.position.x));
        float finalAngleInDeg = mouseAngleInRad * Mathf.Rad2Deg - initialDeflectionInDeg;
        var targetRotation = Quaternion.Euler(head.transform.eulerAngles.x, head.transform.eulerAngles.y, finalAngleInDeg);
        head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, targetRotation, headRotationSpeed*Time.timeScale);
    }

    private void ControlLinearMotion()
    {
        Vector2 currentVelocity = Vector2.zero;
        float currentAngleInRad = -1 * (transform.localEulerAngles.z) * Mathf.Deg2Rad;
        if (upPressed)
        {
            currentVelocity += new Vector2(moveForwardSpeed * Mathf.Sin(currentAngleInRad), moveForwardSpeed * Mathf.Cos(currentAngleInRad));
        }
        else if (downPressed)
        {
            currentVelocity -= new Vector2(moveBackwardSpeed * Mathf.Sin(currentAngleInRad), moveBackwardSpeed * Mathf.Cos(currentAngleInRad));
        }
        rigidBody.velocity = currentVelocity;
    }

    private void ControlRotation()
    {
        float currentAngularVelocity = 0f;
        if (leftPressed)
        {
            currentAngularVelocity += rotationSpeed;
        }
        else if (rightPressed)
        {
            currentAngularVelocity -= rotationSpeed;
        }
        rigidBody.angularVelocity = currentAngularVelocity;
    }

    private void TakeInput()
    {
        LinearMotionInput();
        sidewayMotionInput();
    }

    private void sidewayMotionInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            leftPressed = true;
            rightPressed = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rightPressed = true;
            leftPressed = false;
        }
        else
        {
            rightPressed = false;
            leftPressed = false;
        }
    }

    private void LinearMotionInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            upPressed = true;
            downPressed = false;
        }  
        else if (Input.GetKey(KeyCode.S))
        {
            downPressed = true;
            upPressed = false;
        }
        else
        {
            upPressed = false;
            downPressed = false;
        }
    }
}
