using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{

    //Active Variables
    bool active = false;
    float maxDistanceToStayActive = 10f;

    // Health Variables
    float enemyHealth = 3;

    // PlayerShip Variable
    PlayerShip playerShip;

    //Move Variables
    float minDistanceFromPlayer = 3f; //Distance if not in vicinity of which, enemy will move towards the player
    float moveSpeed = 1f;

    //Rotation Variables
    float initialAngleDefect = -90; 

    //Component Variables
    Rigidbody2D rigidBody;

    //Child and Child Dependent Object Variables
    GameObject enemyShootPoint;

    //Enemy Projectile Variables
    [SerializeField] GameObject enemyProjectile;
    float projectileSpeed = 10f;

    //Shoot Variables
    bool reloaded = true;
    float reloadSpeed = 2f;
    float reloadTimerMax = 5f;
    float reloadTimerCurrent = 0f;
    float maxDistanceToShoot = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerShip = GameObject.FindObjectOfType<PlayerShip>();
        rigidBody = GetComponent<Rigidbody2D>();
        enemyShootPoint = transform.Find("EnemyShipBody").transform.Find("EnemyShootPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ActivationControl();
        MoveTowardsPlayerShip();
        RotateTowardsPlayerShip();
        ShootControl();
        HealthControl();
    }

    private void ActivationControl()
    {
        if (Vector2.Distance(transform.position, playerShip.transform.position) < maxDistanceToStayActive)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }

    private void ShootControl()
    {
        if(!active)
        {
            return;
        }
        if(Vector2.Distance(transform.position, playerShip.transform.position) > maxDistanceToShoot)
        {
            return;
        }
        if (! reloaded)
        {
            if (reloadTimerCurrent > reloadTimerMax)
            {
                reloaded = true;
                reloadTimerCurrent = 0;
            }
            else
            {
                reloadTimerCurrent += reloadSpeed * Time.deltaTime;
            }
        }
        else if (reloaded)
        {
            Shoot();
            reloaded = false;
        }
    }

    private void Shoot()
    {
        float projectileAngleInRad = -1 * transform.eulerAngles.z * Mathf.Deg2Rad;
        GameObject projectile = Instantiate(enemyProjectile, enemyShootPoint.transform.position, transform.rotation);
        projectile.GetComponent<SpriteRenderer>().sortingOrder = 2;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sin(projectileAngleInRad) * projectileSpeed, Mathf.Cos(projectileAngleInRad) * projectileSpeed);
    }

    private void RotateTowardsPlayerShip()
    {
        if(!active)
        {
            return;
        }
        transform.LookAt(playerShip.transform.position); //LookAt works in respect to 3d Space
        transform.right = playerShip.transform.position - transform.position; //Hence the modification, transform.right changes position on x axis (red arrow) while taking in consideration rotation
        transform.eulerAngles += new Vector3(0, 0, initialAngleDefect);
    }

    private void MoveTowardsPlayerShip()
    {   
        if(!active)
        {
            rigidBody.velocity = new Vector2(0, 0);
            return;
        }
        float rotAngleInRad = -1 * transform.eulerAngles.z * Mathf.Deg2Rad;
        if (Vector2.Distance(transform.position, playerShip.transform.position) > minDistanceFromPlayer)
        {
            rigidBody.velocity = new Vector2(moveSpeed*Mathf.Sin(rotAngleInRad), moveSpeed * Mathf.Cos(rotAngleInRad));
        }
        else
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
    }

    public void HitByPlayerProjectile(float damage)
    {
        enemyHealth -= damage;
    }

    private void HealthControl()
    {
        if (enemyHealth <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        if(gameObject)
        {
            Destroy(gameObject);
        }
    }
}
