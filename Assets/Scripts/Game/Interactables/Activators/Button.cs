using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Button : Interactable
{
    [SerializeField] private Interactable interactableObject;
    Boolean activated;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource sound;
    [SerializeField] GameObject effects;

    [SerializeField] Renderer renderer;
    [SerializeField] Material activatedMaterial;
    [SerializeField] Material inactiveMaterial;

    private enum ButtonType{
        ONOFF,
        MOMENTARY
    }

    [SerializeField] private ButtonType type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            Activate();
            
        }
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"){
            Activate();  
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            if(type == ButtonType.MOMENTARY){
                Deactivate();
            }

        }
    }

 public override void Activate(){
        if(sound != null && !activated){
            sound.Play();
        }
        activated = true;
        interactableObject.Activate();
        anim.SetBool("On", true);

        GameObject.Find("GameManager").GetComponent<GameManager>().PopulateDisabledObjects("interactable", gameObject);

        effects.SetActive(true); //Enable the effects for this button
        renderer.material = activatedMaterial;
    }

    public override void Deactivate(){
        activated = false;
        interactableObject.Deactivate();
        anim.SetBool("On", false); 
        effects.SetActive(false); //Disable the effects for this button
        renderer.material = inactiveMaterial;
    }

    public override Boolean GetActive(){
        return activated;
    }
}
