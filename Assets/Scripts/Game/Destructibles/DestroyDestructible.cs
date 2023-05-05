using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDestructible : Destructable
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
    [SerializeField] private float hitboxDelay; //How long after the destroy has begun shall the hitbox disppear?
    private float hitboxTimer;
    private bool broken;
    void Start(){
        anim = model.GetComponent<Animator>();
    }
    void Update(){
        if(broken){
            if(hitboxTimer > hitboxDelay){
                gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false; //Disables the hitbox
            }else{
                hitboxTimer += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag =="PlayerDealDamageToEnemy" || other.gameObject.tag == "Explosion"){
            anim.SetBool("Broken", true);
            //Break();
            //gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false; //Disables the hitbox
            gameObject.GetComponent<Collider>().enabled = false; //Disable destroy box
            sound.pitch = UnityEngine.Random.Range(0.75f, 1.0f);
            particles.Play(); //Play the particle effects
            sound.Play();
            broken = true;
            spawnCollectibles();
        }
        
    }

    public override void spawnCollectibles(){
        for(int i = 0; i < scrapValue; i++){
            float x = Random.Range(0-collectibleSpread,collectibleSpread);
            float z = Random.Range(0-collectibleSpread,collectibleSpread);
            GameObject scrap = Instantiate(collectiblePrefab, new Vector3(transform.position.x + x,transform.position.y + yOffset,transform.position.z + z), Quaternion.identity);
            scrap.GetComponent<Scrap>().SetSpawned(true);
            scrap.GetComponent<Scrap>().SetCollected(true);
        }
    }

    public override void Break(){
        
        //GameObject.Find("GameManager").GetComponent<GameManager>().PopulateDisabledObjects("destructible", gameObject);
        
        Destroy(gameObject.transform.parent.gameObject);
    }

    public override void Ignite(){
        //NOTHING!
    }
}
