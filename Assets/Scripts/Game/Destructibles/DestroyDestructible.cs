using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDestructible : MonoBehaviour
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
        if(other.gameObject.tag =="PlayerDealDamageToEnemy"){
            anim.SetBool("Broken", true);
            spawnCollectibles();
        }
        gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false; //Disables the hitbox
        sound.pitch = UnityEngine.Random.Range(0.75f + sound.pitch, 1.0f + sound.pitch);
        particles.Play(); //Play the particle effects
        sound.Play();
        Destroy(gameObject.GetComponent<Collider>());
        Destroy(gameObject.transform.parent.gameObject, sound.clip.length); //Destroys the GameObject once the animation finishes
        
    }

    private void spawnCollectibles(){
        for(int i = 0; i < scrapValue; i++){
            float x = Random.Range(0-collectibleSpread,collectibleSpread);
            float z = Random.Range(0-collectibleSpread,collectibleSpread);
            Instantiate(collectiblePrefab, new Vector3(transform.position.x + x,transform.position.y + yOffset,transform.position.z + z), Quaternion.identity);
        }
    }
}
