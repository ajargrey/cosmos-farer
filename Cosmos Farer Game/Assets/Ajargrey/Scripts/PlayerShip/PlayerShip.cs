using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerShip : MonoBehaviour
{
    //Health Variables
    float playerHealth = 5f;

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

    //Fuel Variables
    float maxFuel = 100f;
    float currentFuel = 100f;
    float forwardAccelerationFuelBurnRate = 0f; //1
    float sidewayAccelerationFuelBurnRate = 0.0f; //0.5

    // Misc
    //float closeTo0 = 0.1f;

    // Component Variables
    Rigidbody2D rigidBody;

    //Child and child dependent Object Variables
    GameObject playerShipBody;
    GameObject playerShipTurret;
    GameObject playerTurretShootPoint_left;
    GameObject playerTurretShootPoint_right;

    //Player Projectile Variables
    [SerializeField] PlayerProjectile playerProjectile;
    float projectileSpeed = 10f;

    //Turret Variables
    int selectedTurretNumber = 0;//0-nil, 1-(machine) turret, 2-big turret, 3-side turret
    string selectedTurretName = "";
    GameObject selectedTurret = null;
    bool weaponSwitched = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.mass = mass;
        rigidBody.inertia = 10;
        rigidBody.drag = originalLinearDrag;
        rigidBody.angularDrag = originalAngularDrag;
        playerShipBody = gameObject.transform.Find("PlayerShipBody").gameObject;

        playerShipTurret = playerShipBody.transform.Find("PlayerShipBigTurret").gameObject;
        playerTurretShootPoint_left = playerShipTurret.transform.Find("PlayerTurretBarrelLeft").transform.Find("PlayerTurretShootPoint").gameObject;
        playerTurretShootPoint_right = playerShipTurret.transform.Find("PlayerTurretBarrelRight").transform.Find("PlayerTurretShootPoint").gameObject;
        selectedTurret = playerShipTurret;

        currentFuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        TakeInput();
        WeaponSelectionControl();
        ControlShoot();
        Move();
        ControlHealth();
        //Debug.Log("Fuel Level : " + currentFuel);
    }

    private void WeaponSelectionControl()
    {
        if(!weaponSwitched)
        {
            return;
        }

        //if command to switch weapon has been issued, before actually assigning new weapon, first stop shooting activities of old weapon
        selectedTurret.GetComponent<Animator>().SetBool("Attacking", false);

        changeTurretRotationActivityByName("disable"); //Disabling the old turret script

        switch (selectedTurretNumber)
        {
            case 0:
                selectedTurretName = "";
                break;
            case 1:
                selectedTurretName = "PlayerShipTurret"; //Machine Turret - the basic one
                break;
            case 2:
                selectedTurretName = "PlayerShipBigTurret";
                break;
            case 3:
                selectedTurretName = "PlayerShipSideTurret";
                break;
            default:
                break;
        }
        if(selectedTurretNumber!=0)
        {
            selectedTurret = playerShipBody.transform.Find(selectedTurretName).gameObject;
            changeTurretRotationActivityByName("enable"); // enabling the new turret script
        }


    }

    private void ControlShoot()
    {
        if (selectedTurretNumber==0)
        {
            return;
        }

        if( shotPressed )
        {
            //In animation event calls Shoot Function
            selectedTurret.GetComponent<Animator>().SetBool("Attacking", true);
            //Debug.Log("Should be shooting");
        }
        else if(! shotPressed)
        {
            selectedTurret.GetComponent<Animator>().SetBool("Attacking", false);
        }

        if(weaponSwitched == true)
        {
            weaponSwitched = false; //The weapon has been switched already, and we have not received any input to switch it further
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
        weaponSelectionInput();
        shootInput();
        linearMotionInput();
        sidewayMotionInput();
        rotationMotionInput();
    }

    private void weaponSelectionInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            selectedTurretNumber = 0;
            weaponSwitched = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTurretNumber = 1;
            weaponSwitched = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTurretNumber = 2;
            weaponSwitched = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedTurretNumber = 3;
            weaponSwitched = true;
        } 

        if (weaponSwitched)
        {
            //nothing for now
        }

        //Keeping a weaponSwitched Variable, helps us in changing weapons only once the key has been pressed, thus proving more optimized
    }

    private void changeTurretRotationActivityByName(String command)
    {
        if (selectedTurretName == "")
        {
            return;
        }
        if (command=="enable")
        {
            if(selectedTurretName == "PlayerShipTurret")
            {
                GameObject.Find(selectedTurretName).GetComponent<PlayerShipTurret>().activateRotation(true);
            }
            else if (selectedTurretName == "PlayerShipSideTurret")
            {
                GameObject.Find(selectedTurretName).GetComponent<PlayerShipSideTurret>().activateRotation(true);
            }
            else if (selectedTurretName == "PlayerShipBigTurret")
            {
                GameObject.Find(selectedTurretName).GetComponent<PlayerShipBigTurret>().activateRotation(true);
            }
        }
        else if (command == "disable")
        {
            if (selectedTurretName == "PlayerShipTurret")
            {
                GameObject.Find(selectedTurretName).GetComponent<PlayerShipTurret>().activateRotation(false);
            }
            else if (selectedTurretName == "PlayerShipSideTurret")
            {
                GameObject.Find(selectedTurretName).GetComponent<PlayerShipSideTurret>().activateRotation(false);
            }
            else if (selectedTurretName == "PlayerShipBigTurret")
            {
                GameObject.Find(selectedTurretName).GetComponent<PlayerShipBigTurret>().activateRotation(false);
            }
        }
    }


    private void shootInput()
    {
        if( Input.GetMouseButton(0) )
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
            float currentAngleInRad = -1 * (transform.localEulerAngles.z) * Mathf.Deg2Rad;
            float currentForwardVelocity = rigidBody.velocity.x * Mathf.Sin(currentAngleInRad) + rigidBody.velocity.y * Mathf.Cos(currentAngleInRad);
            if (currentForwardVelocity < maxLinearVelocity)
            {
                if (currentFuel >= forwardAccelerationFuelBurnRate)
                {
                    rigidBody.AddForce(new Vector2(linearForce * Mathf.Sin(currentAngleInRad), linearForce * Mathf.Cos(currentAngleInRad)));
                    currentFuel -= forwardAccelerationFuelBurnRate;
                }
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
            if(currentFuel > sidewayAccelerationFuelBurnRate)
            {
                rigidBody.AddForce(new Vector2(-1 * sidewayForce * Mathf.Cos(currentAngleInDeg), -1 * sidewayForce * Mathf.Sin(currentAngleInDeg)));
                currentFuel -= sidewayAccelerationFuelBurnRate;
            }
        }
        else if (movingRight && currentSidewayVelocity < maxSidewayVelocity)
        {
            if(currentFuel > sidewayAccelerationFuelBurnRate)
            {
                rigidBody.AddForce(new Vector2(sidewayForce * Mathf.Cos(currentAngleInDeg), sidewayForce * Mathf.Sin(currentAngleInDeg)));
                currentFuel -= sidewayAccelerationFuelBurnRate;
            }
        }

    }

    public void ShootFromTurret(bool left, string turretName)
    {
        playerShipTurret = playerShipBody.transform.Find(turretName).gameObject;
        playerTurretShootPoint_left = playerShipTurret.transform.Find("PlayerTurretBarrelLeft").transform.Find("PlayerTurretShootPoint").gameObject;
        if(left==false)
        {
            playerTurretShootPoint_right = playerShipTurret.transform.Find("PlayerTurretBarrelRight").transform.Find("PlayerTurretShootPoint").gameObject;
        }

        float projectileAngleInRad = transform.Find("PlayerShipBody").transform.Find(turretName).transform.eulerAngles.z * Mathf.Deg2Rad * -1;
        Vector3 projectilePosition = playerTurretShootPoint_left.transform.position;
        if (!left)
        {
            projectilePosition = playerTurretShootPoint_right.transform.position;
        }
        PlayerProjectile projectile = Instantiate(playerProjectile, projectilePosition, playerShipTurret.transform.rotation) as PlayerProjectile;
        projectile.GetComponent<SpriteRenderer>().sortingOrder = 2;
        projectile.SetupProjectile(turretName, projectileAngleInRad);
    }

    public void HitByPlayerProjectile(float damage)
    {
        playerHealth -= damage;
    }

    private void ControlHealth()
    {
        if (playerHealth <= 0)
        {
            DestroyPlayer();
        }
    }

    private void DestroyPlayer()
    {
        Destroy(gameObject);
    }
}
