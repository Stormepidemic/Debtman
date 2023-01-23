using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseGame : MonoBehaviour
{
    // Start is called before the first frame update
    private Boolean paused = false;
    public GameObject pauseScreen;

    void Start(){
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape") && !paused){
            Time.timeScale = 0;
            paused = true;
            pauseScreen.SetActive(true);
            print("PAUSED");
        }else if(Input.GetKeyDown("escape") && paused){
            Time.timeScale = 1;
            paused = false;
            pauseScreen.SetActive(false);
            print("UNPAUSED");
        }
    }
}