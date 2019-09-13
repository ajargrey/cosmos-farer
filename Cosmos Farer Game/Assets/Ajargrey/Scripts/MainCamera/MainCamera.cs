using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    //Follow Player Variables
    PlayerShip playerShip;

    //Zoom Variables
    float zoomSpeed = 5f;
    float smoothSpeed = 10f;
    float minOrtho = 1f;
    float maxOrtho = 20f;
    float targetOrtho;
    
    // Start is called before the first frame update
    void Start()
    {
        GetPlayerShipGameObject();
        SetTargetOrtho();
    }

    private void SetTargetOrtho()
    {
        targetOrtho = Camera.main.orthographicSize;
    }

    private void GetPlayerShipGameObject()
    {
        playerShip = FindObjectOfType<PlayerShip>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        ZoomControl();
    }

    private void ZoomControl()
    {
        ResetZoomSpeeds();
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }

    private void ResetZoomSpeeds()
    {
        zoomSpeed = Mathf.RoundToInt(Camera.main.orthographicSize);
        smoothSpeed = 2 * zoomSpeed;
    }

    private void FollowPlayer()
    {
        Vector3 newPosition = playerShip.transform.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
