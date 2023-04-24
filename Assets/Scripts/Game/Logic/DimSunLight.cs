using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimSunLight : MonoBehaviour
{
    [SerializeField] private Light sun;
    private float baseIntensity;
    private float currentIntensity;
    private bool inside;
    private GameObject player;
    [SerializeField] float lowToIntensity;
    // Start is called before the first frame update
    void Start()
    {
        baseIntensity = sun.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if(inside){
            if(currentIntensity > lowToIntensity){
                currentIntensity -= Time.deltaTime;
            }
            
        }else{
            currentIntensity += Time.deltaTime;
        }
        if(currentIntensity < lowToIntensity){
            currentIntensity = lowToIntensity;
        }
        if(currentIntensity > baseIntensity){
            currentIntensity = baseIntensity;
        }
        sun.intensity = currentIntensity;

        if(player == null){
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
            player = null;
        }
    }
}
