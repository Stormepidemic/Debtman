using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTexture : MonoBehaviour
{
    [SerializeField] Renderer renderer;
    Material material;
    [SerializeField] float speed;
    Color defaultColor;
    [SerializeField] Color fadedOutColor;
    [SerializeField] bool inOut; //False -> Fade in, True -> Fade Out
    // Start is called before the first frame update
    void Start()
    {
        material = renderer.material;
        defaultColor = material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        Color newColor = material.GetColor("_Color");
        if(inOut){//FADE IN
            newColor = Color.Lerp(newColor, fadedOutColor, Time.deltaTime*speed);
        }else{ //FADE OUT
            newColor = Color.Lerp(newColor, defaultColor, Time.deltaTime*speed);
        }
        
        material.SetColor("_Color", newColor);
    }

    public void FadeIn(){
        inOut = false;
    }

    public void FadeOut(){
        inOut = true;
    }
}
