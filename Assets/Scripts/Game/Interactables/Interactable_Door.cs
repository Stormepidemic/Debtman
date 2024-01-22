using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable_Door : Interactable
{
    Boolean activated;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource sound;
    [SerializeField] ParticleSystem[] effects;

    public override void Activate(){
        
        if(sound != null && !activated){
            sound.Play();
        }
        if(effects != null && effects.Length > 0 && !activated){
            PlayEffects();
        }
        anim.SetBool("Open", true);

        GameObject.Find("GameManager").GetComponent<GameManager>().PopulateDisabledObjects("interactable", gameObject);
        activated = true;
    }

    public override void Deactivate(){
        activated = false;
        anim.SetBool("Open", false);

    }

    public override Boolean GetActive(){
        return activated;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player" && !activated){
            Activate();
        }
    }

    void PlayEffects(){
        foreach(ParticleSystem effect in effects){
            effect.Play();
        }
    }
}
