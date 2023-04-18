using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Contract : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private Boolean collected;
    [SerializeField] private ParticleSystem collectEffect;
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
            anim.SetBool("Collected", true);
            collected = true;
            collectEffect.Play();
            //Logic to handle adding the contract to the inventory, add that shit here
            GameObject.Find("GameManager").GetComponent<GameManager>().CollectInventoryCollectable("contract");
        }
    }
}
