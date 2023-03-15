using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDestructible : Destructible
{
    [SerializeField] private int scrapValue;
    [SerializeField] private GameObject model;
    private Animator anim;
    [SerializeField] private float destroyTime;
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private float yOffset;
    [SerializeField] private float collectibleSpread;
    [SerializeField] private AudioSource sound;
    [SerializeField] private ParticleSystem particles;

    void Start(){
        anim = model.GetComponent<Animator>();
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag =="PlayerDealDamageToEnemy" || other.gameObject.tag == "Explosion"){
            anim.SetBool("Broken", true);
            //Break();
            gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false; //Disables the hitbox
            gameObject.GetComponent<Collider>().enabled = false; //Disable destroy box
            sound.pitch = UnityEngine.Random.Range(0.75f + sound.pitch, 1.0f + sound.pitch);
            particles.Play(); //Play the particle effects
            sound.Play();
            
            spawnCollectibles();
        }
        
    }

    public override void spawnCollectibles(){
        for(int i = 0; i < scrapValue; i++){
            float x = Random.Range(0-collectibleSpread,collectibleSpread);
            float z = Random.Range(0-collectibleSpread,collectibleSpread);
            Instantiate(collectiblePrefab, new Vector3(transform.position.x + x,transform.position.y + yOffset,transform.position.z + z), Quaternion.identity);
        }
    }

    public override void Break(){
        anim.SetBool("Broken", false);
        GameObject.Find("GameManager").GetComponent<GameManager>().PopulateDisabledObjects("destructible", gameObject);
        anim.Rebind();
        anim.Update(0f);
    }

    public override void Reset(){
        anim.SetBool("Broken", false);
        gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true; //reenable destroy box
        particles.Stop(); //Play the particle effects
        sound.Stop();
    }
}
