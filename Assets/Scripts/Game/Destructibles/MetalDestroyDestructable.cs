using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDestroyDestrucable : Destructable
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject renderer; //this is the literal model, the other model variable is just poorly named here lol.
    private Animator anim;
    [SerializeField] private float destroyTime;
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private float yOffset;
    [SerializeField] private float collectibleSpread;
    [SerializeField] private AudioSource sound;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float hitboxDelay; //How long after the destroy has begun shall the hitbox disppear?
    private float hitboxTimer;
    private bool damaged;
    //[SerializeField] private int damageStates; //The amount of levels of damage this object can take
    [SerializeField] private Material[] damageStates;
    int damageState;
    void Start(){
        anim = model.GetComponent<Animator>();
        damageState = 0;
    }
    void Update(){
        if(damaged){
            if(hitboxTimer > hitboxDelay){
                gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = true; //Disables the hitbox
                damageState++;
                
                //hitboxTimer = 0.0f;
            }else{
                gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false;
                hitboxTimer += Time.deltaTime;
                
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag =="PlayerDealDamageToEnemy" || other.gameObject.tag == "Explosion"){
            if(damageState == damageStates.Length-1){
                anim.SetBool("Broken", true);
                //Break();
                //gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false; //Disables the hitbox
                gameObject.GetComponent<Collider>().enabled = false; //Disable destroy box
                sound.pitch += UnityEngine.Random.Range(0.1f, 0.25f);
                damaged = true;
                spawnCollectibles(false);
                hitboxTimer = 0.0f;
            }else{
                damageState++;
                renderer.GetComponent<Renderer>().material = damageStates[damageState];
            }
            sound.pitch += UnityEngine.Random.Range(0.1f, 0.25f);
            particles.Play();
            sound.Play();
        }
    }

    public override void spawnCollectibles(bool autoCollect){
        for(int i = 0; i < scrapValue; i++){
            float x = Random.Range(0-collectibleSpread,collectibleSpread);
            float z = Random.Range(0-collectibleSpread,collectibleSpread);
            GameObject scrap = Instantiate(collectiblePrefab, new Vector3(transform.position.x + x,transform.position.y + yOffset,transform.position.z + z), Quaternion.identity);
            scrap.GetComponent<ScrapV2>().SetSpawned(autoCollect);
            //scrap.GetComponent<Scrap>().SetCollected(true);
        }
    }

    public override void Break(){
        
        //GameObject.Find("GameManager").GetComponent<GameManager>().PopulateDisabledObjects("destructible", gameObject);
        GameObject.Find("GameManager").GetComponent<GameManager>().IncrementDestruction(destructionWeight);
        Destroy(gameObject.transform.parent.gameObject);
    }

    public override void Ignite(){
        //NOTHING!
    }
}
