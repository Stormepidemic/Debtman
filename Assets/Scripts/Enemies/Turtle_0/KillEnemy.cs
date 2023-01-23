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
    [SerializeField] private float flySpeed = 0.1f;
    private Vector3 flyDirection;

    void Start(){
        parentObject = transform.parent.parent.gameObject;
        parentBody = transform.parent.parent.gameObject.GetComponent<Rigidbody>();
    }
    void Update(){
        if(flying == 1){
        
            float x = (float)(Random.value * 2.0);
            float y = (float)Random.value;
            float z = (float)(Random.value * 2.0);
            parentBody.velocity = flyDirection*flySpeed;
        }
    }

    void OnTriggerEnter(Collider other){ 
        if(flying != 1){
            if(other != null){ //This if-statement was here because for some reason the frame after the enemy should be killed, this causes a nullpointer exception.
                if(!other.gameObject.GetComponent<PlayerMovement>().enemyImmunity){
                    anim = enemy.GetComponent<Animator>();
                    anim.SetBool("kill",true);
                    enemy.GetComponent<Turtle_0>().isMoving(false);
                    enemy.GetComponent<AudioSource>().Play(0);
                    Destroy(hurtbox); //Destroy the hurtbox for the enemy
                    Destroy(parentObject , killTime); //Destroy the enemy after 3 seconds
                }else if(other.gameObject.GetComponent<PlayerMovement>().enemyImmunity){
                    Destroy(hurtbox); //Destroy the hurtbox for the enemy
                    flying = 1;
                    contactPosition = other.gameObject.transform;
                    flyDirection = -(contactPosition.position - gameObject.transform.position);
                    Destroy(parentObject, killTime); //Destroy the enemy after 3 seconds
                }
            }
        }
    }
}
