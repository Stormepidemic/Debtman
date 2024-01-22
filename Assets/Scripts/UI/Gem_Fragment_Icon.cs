using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Gem_Fragment_Icon : MonoBehaviour
{

    [SerializeField] Texture[] stageZeroFrames;
    [SerializeField] Texture[] stageOneFrames;
    [SerializeField] Texture[] stageTwoFrames;
    [SerializeField] int stage;
    [SerializeField] private RawImage fragmentImage;
    Texture[][] currentStageFrames;
    int currentFrame = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentStageFrames = new Texture[][]{stageZeroFrames,stageOneFrames,stageTwoFrames};
    }

    // Update is called once per frame
    void Update()
    {
        int currentFramesLength = currentStageFrames[stage].Length;
        fragmentImage.texture = currentStageFrames[stage][currentFrame];
        currentFrame++;
        if(currentFrame >= currentFramesLength){
            currentFrame = 0;
        }
    }

    public void SetStage(int s){
        stage = s;
    }


}
