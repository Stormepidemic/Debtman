using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Candle_Untimed : Interactible
{
    [SerializeField] private GameObject[] activatableObjects;
    [SerializeField] private GameObject[] effects;
    [SerializeField] private int spawnDelay;
    private int counter;
    Boolean activated;
    private int objectIndex;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(activated){
            //This code counts frames and activates an object after a set amount of frames have passed
            counter++;
            //We want the platforms to spawn in a certain order, so a normal FOR is used rather than foreach
            if(counter >= spawnDelay){
                counter = 0;
                if(objectIndex < activatableObjects.Length){
                    activatableObjects[objectIndex].GetComponent<Activatable_Object>().Activate();
                    objectIndex++;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("PlayerDealDamageToEnemy") || other.CompareTag("Explosion")){
            Activate();
        }
    }

    public override void Activate(){
        activated = true;
        foreach(GameObject obj in effects){ //Enable the particles & lighting
            obj.SetActive(true);
        }
        gameObject.GetComponent<Collider>().enabled = false; //Disable the trigger
        GameObject.Find("GameManager").GetComponent<GameManager>().PopulateDisabledObjects("interactible", gameObject);
    }

    public override void Deactivate(){
        activated = false;
        foreach(GameObject obj in effects){ //Enable the particles & lighting
            obj.SetActive(false);
        }
        gameObject.GetComponent<Collider>().enabled = true; //Enable the trigger
        objectIndex = 0; //This is reset because we want the platforms or whatever to spawn in an expliclt order, so using a normal for loop above can achieve that.
        foreach(GameObject activatable in activatableObjects){
            activatable.GetComponent<Activatable_Object>().Deactivate();
        }

    }
    
}
