using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WoodDestructable : Destructable
{
    [SerializeField] ParticleSystem fireParticles;
    [SerializeField] GameObject breakBox;
    [SerializeField] private int burnTime; //How many frames the object will burn for once touched
    [SerializeField] private GameObject fireBox;
    [SerializeField] private GameObject spreadBox;
    [SerializeField] private ParticleSystem flames;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private int frameCounter;
    [SerializeField] private GameObject model;
    private Boolean onFire;
    // Start is called before the first frame update
    void Start()
    {
        frameCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(onFire){
            if(frameCounter > burnTime){
                //this.Break();
                model.GetComponent<Animator>().SetBool("Broken", true);
                //Break();
                spawnCollectibles();
                onFire = false;
                frameCounter = 0;
                spreadBox.SetActive(false);
                
            }
            frameCounter++;
        }
        
    }


    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Player"){
            flames.Play();
            smoke.Play();
            fireBox.SetActive(true);
            onFire = true;
        }
    }

    public override void spawnCollectibles(){
        breakBox.GetComponent<DestroyDestructible>().spawnCollectibles();
    }

    public override void Break(){
        // //fireBox.SetActive(false);
        // flames.Stop();
        // smoke.Stop();
        // onFire = false;
        // spreadBox.SetActive(false);
        // //GameObject.Find("GameManager").GetComponent<GameManager>().PopulateDisabledObjects("destructible", gameObject);
        breakBox.GetComponent<DestroyDestructible>().Break(); //Call to the breakBox script
        
    }

    public override void Ignite(){
        
        flames.Play();
        smoke.Play();
        fireBox.SetActive(true);
        onFire = true;
    }
}
