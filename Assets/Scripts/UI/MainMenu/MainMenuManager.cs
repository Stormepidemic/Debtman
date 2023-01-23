using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using System;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] videos; //The videos that are played when the game launches and serve as a buffer between start and the main menu
    private int currentVideoIndex = 0;
    [SerializeField] GameObject menu;
    void Start()
    {
        menu.SetActive(false);
        for(int i = 0; i<videos.Length; i++){
            videos[i].SetActive(false);
        }
        videos[currentVideoIndex].GetComponent<VideoPlayer>().frame = videos[currentVideoIndex].GetComponent<VideoPlayer>().frame + 2;
    }

    // Update is called once per frame
    void Update()
    {
        //Play all of the intro videos
        if(currentVideoIndex < videos.Length){
            videos[currentVideoIndex].SetActive(true);
            int videoFrameCount = Convert.ToInt32(videos[currentVideoIndex].GetComponent<VideoPlayer>().frameCount);
            int currentFrame = Convert.ToInt32(videos[currentVideoIndex].GetComponent<VideoPlayer>().frame);
            if(currentFrame == videoFrameCount-1){
                switchVideo();
            }
        }else{
            menu.SetActive(true);
        }
        
    }

    //Switch between videos
    void switchVideo(){
        videos[currentVideoIndex].SetActive(false);
        if(currentVideoIndex < videos.Length){
            currentVideoIndex = currentVideoIndex + 1;
        }
    }
}
