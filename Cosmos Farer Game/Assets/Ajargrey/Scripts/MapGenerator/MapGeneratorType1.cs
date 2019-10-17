using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorType1 : MonoBehaviour
{

    //This Generator currently divides the given space into a grid of squares and spawns enemies and debris at 
    // the cross points by deciding randomly

    // Grid Coordinates
    float leftMostPoint = -100f;
    float rightMostPoint = 100f;
    float upMostPoint = 100f;
    float downMostPoint = -100f;

    float gridUnitSize = 1f;

    // Probabilty Factors
    int totalSpawnProbabilityFactor = 500;
    int enemySpawnProbabilityFactor = 1;
    int asteroidSpawnProbabilityFactor = 10;

    //EnemyShip Variables
    [SerializeField] GameObject enemyShipPrefab;

    //Asteroid (Debris) Variables
    [SerializeField] GameObject asteroidPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for(float x = leftMostPoint; x < rightMostPoint; x+=gridUnitSize)
        {
            for(float y = downMostPoint; y < upMostPoint; y+=gridUnitSize)
            {
                int randomNumber = UnityEngine.Random.Range(0, totalSpawnProbabilityFactor);
                if(randomNumber <= enemySpawnProbabilityFactor)
                {
                    Instantiate(enemyShipPrefab, new Vector3(x, y), Quaternion.identity);
                }
                else if(randomNumber>enemySpawnProbabilityFactor && randomNumber<=enemySpawnProbabilityFactor+asteroidSpawnProbabilityFactor)
                {
                    Instantiate(asteroidPrefab, new Vector3(x, y), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
