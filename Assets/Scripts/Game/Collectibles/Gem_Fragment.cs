using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gem_Fragment : MonoBehaviour
{
    private Boolean collected = false;
    [SerializeField] private GameObject manager;
    
    [SerializeField] private AudioSource collectSound;
    [SerializeField] private AudioSource destroySound;
    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private Animator anim;

    private Boolean spawned; //If this scrap was spawned by another object or was present at the beginning

    private bool objectDestroyed; //This is used so the gameObject isn't destroyed before the audio clip can finish
    // Start is called before the first frame update
    void Start()
    {
        
        manager = GameObject.Find("GameManager");
    }

    void Awake(){
        
        manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if(manager == null){
            manager = GameObject.Find("GameManager");
        }
        
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerDealDamageToEnemy"){
            anim.SetBool("Collected", true);
            collectSound.pitch = UnityEngine.Random.Range(0.75f, 1.0f);
            collectSound.Play();
            collected = true;
            collectEffect.Play();
            Destroy(GetComponent<BoxCollider>()); //removes the collider from the object
            Destroy(gameObject, collectSound.clip.length);
            
        }else if(other.gameObject.tag == "Explosion"){
            destroySound.pitch = UnityEngine.Random.Range(0.75f, 1.0f);
            destroySound.Play();
            Destroy(GetComponent<BoxCollider>()); //removes the collider from the object
            Destroy(gameObject, destroySound.clip.length);
        }
        
    }

    public void SetSpawned(Boolean spawned){
        this.spawned = spawned;
    }

    public void SetCollected(Boolean collected){
        this.collected = collected;
    }

    public Boolean GetSpawned(){
        return this.spawned;
    }

    public Boolean GetCollected(){
        return this.collected;
    }
}
