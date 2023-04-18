using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KillEnemy : MonoBehaviour
{
    public GameObject enemy;
    private Animator anim;
    public GameObject hurtbox;
    public float killTime;
    private int flying = 0;
    private Transform contactPosition; //The position at which the enemy was killed
    private GameObject parentObject;
    private Rigidbody parentBody;

    [SerializeField] private AudioSource sound;
    [SerializeField] private float bounceForce;

    [SerializeField] ParticleSystem[] particleEffects;

    void Start(){
        parentObject = transform.parent.parent.gameObject;
        parentBody = transform.parent.parent.gameObject.GetComponent<Rigidbody>();
        anim = enemy.GetComponent<Animator>();
    }
    void Update(){
    }

    void OnTriggerEnter(Collider other){ 
        
        if(other.gameObject.tag == "Player"){

            if(!other.gameObject.GetComponent<PlayerMovement>().enemyImmunity){
                kill();
                other.gameObject.GetComponent<PlayerMovement>().BouncePlayer(bounceForce);
            }else if(other.gameObject.GetComponent<PlayerMovement>().enemyImmunity){
                kill();
            }
        }
        if(other.gameObject.tag == "Explosion"){
            kill();
        }
    }

    public void kill(){
        //An animation event is used to prompt when the append this object to the list of 'destroyed' objects.
        //See: KillEnemyAction.cs
        anim.SetBool("kill",true); 
        sound.Play();
        hurtbox.SetActive(false); //Destroy the hurtbox for the enemy
        playEffects();
        
    }

    private void playEffects(){
        if(particleEffects != null){
            if(particleEffects.Length > 0){
                foreach(ParticleSystem effect in particleEffects){
                    effect.Play();
                }
            }
        }
    }
}
