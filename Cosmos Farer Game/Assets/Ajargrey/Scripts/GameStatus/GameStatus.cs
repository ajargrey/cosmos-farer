using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{

    // Pause Variables
    bool isPaused = false;

    //TimeScale
    int currentTimeScale = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        GetPauseInput();
        PauseControl();
        Debug.Log(isPaused);
    }

    private void PauseControl()
    {
        if (isPaused)
        {
            Time.timeScale = 0.0f;
        }
        else if (!isPaused)
        {
            Time.timeScale = currentTimeScale;
        }
    }

    private void GetPauseInput()
    {
        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            isPaused = !isPaused;
            Debug.Log("Pressed Escape");
        }
    }
}
