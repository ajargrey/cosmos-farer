using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{
    //Mass
    float originalMass = 1f;

    //Damage
    float damage = 1f;

    //Speed
    float projectileSpeed = 10f;

    //Sprite Array
    [SerializeField] Sprite[] spriteArray;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().mass = originalMass;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
       collision.transform.SendMessage("HitByPlayerProjectile", damage);
       ProjectileCollided();
    }


    private void ProjectileCollided()
    {
        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    public void SetupProjectile(String turretName, float projectileAngleInRad)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sin(projectileAngleInRad) * projectileSpeed, Mathf.Cos(projectileAngleInRad) * projectileSpeed);
        switch(turretName)
        {
            case "PlayerShipTurret":
                GetComponent<SpriteRenderer>().sprite = spriteArray[1];
                GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;

            case "PlayerShipBigTurret":
                GetComponent<SpriteRenderer>().sprite = spriteArray[2];
                GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;

            case "PlayerShipSideTurret":
                GetComponent<SpriteRenderer>().sprite = spriteArray[3];
                GetComponent<SpriteRenderer>().sortingOrder = -5;
                break;

            default:
                break;
                
        }
    }

}
