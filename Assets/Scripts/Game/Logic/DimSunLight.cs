using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimSunLight : Volume
{
    [SerializeField] private Light sun;
    private float baseIntensity;
    private float currentIntensity;
    private bool inside;
    private GameObject player;
    [SerializeField] float lowToIntensity; //The intensity that the light gets dimmed to when the player enters
    [SerializeField] bool DimOrBrighten; //True for Dim, False for Brighten
    // Start is called before the first frame update
    void Start()
    {
        baseIntensity = sun.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if(DimOrBrighten){ //dim the light
            if(inside){
                if(currentIntensity > lowToIntensity){
                    currentIntensity -= Time.deltaTime;
                }
            
            }else{
                if(currentIntensity < baseIntensity){
                    currentIntensity += Time.deltaTime;
                }
            }
        
            if(currentIntensity > baseIntensity){
                currentIntensity = baseIntensity;
            }
            sun.intensity = currentIntensity;

        }else{ //brighten the light
            if(inside){
                if(currentIntensity < lowToIntensity){
                    currentIntensity += Time.deltaTime;
                }
            
            }else{
                if(currentIntensity > baseIntensity){
                    currentIntensity -= Time.deltaTime;
                }
            }
        
            if(currentIntensity < baseIntensity){
                currentIntensity = baseIntensity;
            }
            sun.intensity = currentIntensity;
        }
        
        //Handle the player dying and being moved back to an area where the sunlight should be reset to it's base intensity
        if((player == null)){
            currentIntensity = baseIntensity;
            inside = false;
        }
}    


    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            inside = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            inside = false;
            //player = null;
        }
    }
}
