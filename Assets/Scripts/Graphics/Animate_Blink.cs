using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Animate_Blink : MonoBehaviour
{
    [SerializeField] private Texture baseTexture;
    [SerializeField] private Texture blink;
    private int frameCounter;
    [SerializeField] private float blinkFrequency = 120; //How many frames should pass before a blink occurs?
    [SerializeField] private float blinkLength = 10; //Amount of frames that the 'blinking face' should be shown for
    private Boolean blinking;
    private int lastBlink = 0;
    [SerializeField] private bool effectiveOnPause;
    // Update is called once per frame
    void Update()
    {
        //Determine if this current frame should be animated or stepped over
        bool run = false;
        if(effectiveOnPause){
            run = true;
        }else if(Time.timeScale > 0){
            run = true;
        }

        if(run){ //makes it so this texture animation is beholden to the timescale, so it will pause if the game is paused
            frameCounter = frameCounter + 1;
        if((frameCounter % blinkFrequency) == 0){
            blinking = true;
            GetComponent<Renderer>().material.SetTexture("_MainTex", blink);
            lastBlink = frameCounter;
        }else{
            if(blinking && ((frameCounter - lastBlink) > blinkLength)){
                GetComponent<Renderer>().material.SetTexture("_MainTex", baseTexture);
                blinking = false;
                }
            }
        }
    
        
    }
}
