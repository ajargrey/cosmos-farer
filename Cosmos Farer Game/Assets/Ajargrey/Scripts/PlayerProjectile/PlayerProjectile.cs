using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{
    //Mass
    float originalMass = 1f;

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
}
