using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosive : Destructable
{
    [SerializeField] private GameObject model;
    private Animator anim;
    [SerializeField] private float destroyTime;
    [SerializeField] private float primeTime;
    [SerializeField] private AudioSource sound;
    [SerializeField] private ParticleSystem explodeParticles;
    [SerializeField] private ParticleSystem primedParticles0;
    [SerializeField] private ParticleSystem primedParticles1;
    [SerializeField] private GameObject explosionRadius;
    [SerializeField] private GameObject explosionLight;

    private GameManager manager;

    void Start(){
        anim = model.GetComponent<Animator>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    


    void OnCollisionEnter(Collision other){
        if((other.gameObject.tag == "Player") || (other.gameObject.tag == "PlayerDealDamageToEnemy") || (other.gameObject.tag == "Fire")){
            //manager.HandlePlayerDeath(); //KILL the player!
            prime();
            
        }else if(other.gameObject.tag == "Explosion"){
            explode();
        }
        
        
    }

    void OnTriggerEnter(Collider other){
        if(other != null){
            if((other.gameObject.tag == "PlayerDealDamageToEnemy")){
            //manager.HandlePlayerDeath(); //KILL the player!
                explode();
            
            }else if(other.gameObject.tag == "Explosion"){
                explode();
            }else if(other.gameObject.tag == "Fire"){
                prime();
            }
        }
        
    }

    private void prime(){
        primedParticles0.Play();
        primedParticles1.Play();
        Invoke("explode", primeTime/2);
    }

    void explode(){
        anim.SetBool("Explode", true); //Play explosion animation
        explosionRadius.SetActive(true); //Enable the explosion radius
        explosionLight.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = false; //Disables the hitbox
        //Destroy(gameObject.GetComponent<Collider>()); //Destroy the hitbox
        sound.pitch = UnityEngine.Random.Range(0.5f, 1.0f);
        primedParticles0.Stop();
        primedParticles1.Stop();
        explodeParticles.Play(); //Play the particle effects
        //sound.Play();
        AudioSource.PlayClipAtPoint(sound.GetComponent<AudioSource>().clip, gameObject.transform.position, 1.0f);
        // if(explosionLight != null){
        //     Destroy(explosionLight, 0.3f);
        //     if(explosionRadius != null){
        //         Destroy(explosionRadius, 0.3f);
        //     }
        // }
        // Destroy(this.gameObject, sound.clip.length); 
    }

    public override void spawnCollectibles(){
        //Overriden to promote code reuse, this doesn't actually spawn anything...
    }

    public override void Break(){
        Destroy(gameObject);
    }

    public override void Ignite(){
        
    }

    
}
