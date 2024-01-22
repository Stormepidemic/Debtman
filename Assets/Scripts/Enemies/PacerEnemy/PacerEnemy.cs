using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PacerEnemy : EnemyBase
{
    /*
    This class holds the logic used by a 'pacing' enemy- one that moves back and forth between two points.
    */
    public GameObject startPoint;
    public GameObject endPoint;

    public GameObject betweenLine; //A line that I've drawn between the Start and End points.

    [SerializeField] GameObject[] effects;
    [SerializeField] private Animator anim; //The animator used by this enemy.
    private Boolean canMove = true; //Can this enemy move? 
    private GameManager manager; //The GameManager instance.

    private bool entered;

    [SerializeField] float speed; //How quickly should this enemy move?
    private float distanceBetweenPoints; //Distance between the start and end points.

    [SerializeField] private GameObject bloodPrefab; //The blood prefab that gets spawned when this enemy dies.
    [SerializeField] private AudioSource deathSound;

    // Start is called before the first frame update
    void Start()
    {
        //Disable the visibility of the start and end points
        startPoint.GetComponent<Renderer>().enabled = false;
        endPoint.GetComponent<Renderer>().enabled = false;
        betweenLine.SetActive(false);

        distanceBetweenPoints = Vector3.Distance(startPoint.transform.position, endPoint.transform.position); 

        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        alive = true; //Set that this enemy is alive
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move(); //Do the enemy's movement
    }


    public override void Kill(string type){
        GameObject.Find("GameManager").GetComponent<GameManager>().IncrementDestruction(destructionWeight);
        anim.SetBool("Kill", true);
        gameObject.GetComponent<Collider>().enabled = false;
        canMove = false;
        deathSound.Play();
        Instantiate(bloodPrefab, gameObject.transform.position, gameObject.transform.rotation);
        playEffects();
        Destroy(gameObject.transform.parent.gameObject, anim.GetCurrentAnimatorStateInfo(0).length/2);
    }

    void OnTriggerEnter(Collider other){
        string tag = other.gameObject.tag;
        PlayerMovement playerScript = GameObject.Find("Player_Character").GetComponent<PlayerMovement>();
        if(!entered){
            entered = true;
            switch(tag){
            case("PlayerDealDamageToEnemy"): //Kill enemy via spin
                if(this.spinnable){
                    this.Kill("spun");
                }else{
                    manager.HandlePlayerDeath();
                }
            break;
            case("PlayerStomp"): //Kill enemy via stomp
                if(stompable){
                    this.Kill("stomp");
                    playerScript.BouncePlayer(bounceForce);
                }else{
                    manager.HandlePlayerDeath();
                }
            break;
            case("Explosion"):
                this.Kill("spun"); //might change this later...
            break;
            case("Death_Barrier"): //Kill enemy if they somehow get out of bounds
                this.Kill("spun");
            break;
            case("Player"): //Kill the player
                if(!other.gameObject.GetComponent<PlayerMovement>().enemyImmunity){
                    manager.HandlePlayerDeath();
                }
                print("DIE");   
            break;
            default:
                entered = false;
            break;
            }
        }
    }

    void OnTriggerStay(Collider other){
        this.OnTriggerEnter(other);
    }

    // private void move(){
    //     if(canMove && alive){
    //         Vector3 enemyPos = gameObject.transform.position; //Position of this enemy
            
    //         if(Vector3.Distance(enemyPos, endPoint.transform.position) > 0.1*distanceBetweenPoints){
    //             gameObject.transform.position = Vector3.MoveTowards(enemyPos, endPoint.transform.position, Time.deltaTime*speed);
    //         }else{
    //             gameObject.transform.position = Vector3.Lerp(enemyPos, endPoint.transform.position, Time.deltaTime*speed);
    //         }
    //         if(Vector3.Distance(enemyPos, endPoint.transform.position) < 0.1f){
    //             GameObject tempPoint = startPoint;
    //             startPoint = endPoint;
    //             endPoint = tempPoint;
    //             anim.SetTrigger("TurnAround");
    //             canMove = false;
    //         }
    //     }
    // }

    private void move(){
        if(canMove){
            Vector3 pos = gameObject.transform.position;
            gameObject.transform.position = Vector3.MoveTowards(pos, endPoint.transform.position, speed);
            if(Vector3.Distance(pos, endPoint.transform.position) < 0.1f){
                GameObject tempPoint = startPoint;
                startPoint = endPoint;
                endPoint = tempPoint;
                gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
            }
        }  
        
    }
    public void TurnAround(){
        anim.ResetTrigger("TurnAround");
        canMove = true;
    }

    private void playEffects(){
        if(effects != null){
            if(effects.Length > 0){
                foreach(GameObject effect in effects){
                    effect.SetActive(true);
                }
            }
        }
    }
}
