using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerShip : MonoBehaviour
{

    //Mass
    float mass = 10f;

    //Left and Right Input Variables
    bool movingLeft = false;
    bool movingRight = false;

    //Left and Right movement variables
    float maxSidewayVelocity = 20f;
    float sidewayForce = 5f;
    

    //Forward and backward Input Variables
    bool movingForward = false;
    bool movingBackward = false;

    // Forward and backward movement variables
    float maxLinearVelocity = 5f;
    float linearForce = 50f;
    float originalLinearDrag = 0.5f;
    float breakDrag = 15f;
    

    //Rotation Input Variables
    bool rotatingLeft = false;
    bool rotatingRight = false;

    //Rotation variabless
    float maxAngularVelocity = 100f;
    float originalAngularDrag = 0.5f;
    float angularForce = 5f;

    //Shoot Input variables
    bool shotPressed = false;

    // Misc
    //float closeTo0 = 0.1f;

    // Component Variables
    Rigidbody2D rigidBody;

    //Child and child dependent Object Variables
    GameObject playerShipBody;
    GameObject playerShipTurret;
    GameObject playerTurretShootPoint;

    //Player Projectile Variables
    [SerializeField] GameObject playerProjectile;
    float projectileSpeed = 10f;



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.mass = mass;
        rigidBody.inertia = 10;
        rigidBody.drag = originalLinearDrag;
        rigidBody.angularDrag = originalAngularDrag;
        playerShipBody = gameObject.transform.Find("PlayerShipBody").gameObject;
        playerShipTurret = playerShipBody.transform.Find("PlayerShipTurret").gameObject;
        playerTurretShootPoint = playerShipTurret.transform.Find("PlayerTurretBarrel").transform.Find("PlayerTurretShootPoint").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        TakeInput();
        ControlShoot();
        Move();
    }

    private void ControlShoot()
    {
        if( shotPressed )
        {                           
            //In animation event calls Shoot Function
            playerShipTurret.GetComponent<Animation>().Play("PlayerTurretBarrelShootAnimation");
        }
    }

    public void Move()
    {
        controlLinearMotion();
        controlSidewayMotion();
        controlRotation();
    }


    private void TakeInput()
    {
        shootInput();
        linearMotionInput();
        sidewayMotionInput();
        rotationMotionInput();
    }

    private void shootInput()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            shotPressed = true;
        }
        else
        {
            shotPressed = false;
        }
    }

    private void linearMotionInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            movingForward = true;
            movingBackward = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movingBackward = true;
            movingForward = false;
        }
        else
        {
            movingForward = false;
            movingBackward = false;
        }
    }

    private void sidewayMotionInput()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            movingLeft = true;
            movingRight = false;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            movingRight = true;
            movingLeft = false;
        }
        else
        {
            movingRight = false;
            movingLeft = false;
        }
    }

    private void rotationMotionInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rotatingLeft = true;
            rotatingRight = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotatingRight = true;
            rotatingLeft = false;
        }
        else
        {
            rotatingLeft = false;
            rotatingRight = false;
        }
    }

    private void controlRotation()
    {
        if (rotatingRight && rigidBody.angularVelocity > -1 * maxAngularVelocity)
        {
            rigidBody.AddTorque(-1 * angularForce);
        }
        else if (rotatingLeft && rigidBody.angularVelocity < maxAngularVelocity)
        {
            rigidBody.AddTorque(angularForce);
        }
     
    }

    private void controlLinearMotion()
    {
        if (movingForward)
        {   
            float currentAngleInDeg = -1 * (transform.localEulerAngles.z) * Mathf.Deg2Rad;
            float currentForwardVelocity = rigidBody.velocity.x * Mathf.Sin(currentAngleInDeg) + rigidBody.velocity.y * Mathf.Cos(currentAngleInDeg);
            if (currentForwardVelocity < maxLinearVelocity)
            {
                rigidBody.AddForce(new Vector2(linearForce * Mathf.Sin(currentAngleInDeg), linearForce * Mathf.Cos(currentAngleInDeg)));
            }
        }

        if (movingBackward)
        {
            rigidBody.drag = breakDrag;
        }
        else if(!movingBackward)
        {
            rigidBody.drag = originalLinearDrag;
        }

        
    }

    private void controlSidewayMotion()
    {
        
        float currentAngleInDeg = (transform.localEulerAngles.z) * Mathf.Deg2Rad;
        float currentSidewayVelocity = rigidBody.velocity.x * Mathf.Cos(currentAngleInDeg) + rigidBody.velocity.y * Mathf.Sin(currentAngleInDeg);

        if (movingLeft && -1*currentSidewayVelocity > -1*maxSidewayVelocity)
        {
            rigidBody.AddForce(new Vector2(-1 * sidewayForce * Mathf.Cos(currentAngleInDeg), -1 * sidewayForce * Mathf.Sin(currentAngleInDeg)));
        }
        else if (movingRight && currentSidewayVelocity < maxSidewayVelocity)
        {
            rigidBody.AddForce(new Vector2(sidewayForce * Mathf.Cos(currentAngleInDeg), sidewayForce * Mathf.Sin(currentAngleInDeg)));

        }

    }

    public void Shoot()
    {
        float projectileAngleInRad = transform.Find("PlayerShipBody").transform.Find("PlayerShipTurret").transform.eulerAngles.z * Mathf.Deg2Rad * -1;
        GameObject projectile = Instantiate(playerProjectile, playerTurretShootPoint.transform.position, playerShipTurret.transform.rotation);
        projectile.GetComponent<SpriteRenderer>().sortingOrder = 2;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2( Mathf.Sin(projectileAngleInRad) * projectileSpeed, Mathf.Cos(projectileAngleInRad) * projectileSpeed );
    }
}
