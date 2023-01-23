using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{
    private int frameCount;
    [SerializeField] private Texture[] frames;
    private int frame = 0;
    [SerializeField] private bool effectiveOnPause;
    [SerializeField] private int speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool run = false;
        if(effectiveOnPause){
            run = true;
        }else if(Time.timeScale > 0){
            run = true;
        }
        if(run){ //makes it so this texture animation is beholden to the timescale, so it will pause if the game is paused
            if(frame >= frames.Length){
            frame = 0;
        }else{
            gameObject.GetComponent<RawImage>().texture = frames[frame];
            step();
            }
        }
        
        void step(){ //Steps forward 1 frame of animation
            if(frame%speed == 0){
                frame = frame + speed;
            }
        }
    }
}
