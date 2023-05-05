using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable_Door : Interactible
{
    Boolean activated;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate(){
        activated = true;
        anim.SetBool("Open", true);
        sound.Play();
        GameObject.Find("GameManager").GetComponent<GameManager>().PopulateDisabledObjects("interactible", gameObject);
    }

    public override void Deactivate(){
        activated = false;
        anim.SetBool("Open", false);

    }

    public override Boolean GetActive(){
        return activated;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            Activate();
        }
    }
}
