using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{
    private GameManager manager;
    //[SerializeField] private Enemy enemy;
    [SerializeField] private EnemyBase enemy;
    // Start is called before the first frame update
    [SerializeField] private BoxCollider trigger;
    [SerializeField] protected bool stompable; //Can this enemy be killed by being jumped on?
    private bool entered = false;
    
    void Start(){
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other){
        string tag = other.gameObject.tag;
        PlayerMovement playerScript = GameObject.Find("Player_Character").GetComponent<PlayerMovement>();
        if(!entered){
            entered = true;
            switch(tag){
            case("PlayerDealDamageToEnemy"): //Kill enemy via spin
                enemy.Kill("spun");
            break;
            case("PlayerStomp"): //Kill enemy via stomp
                if(stompable){
                    print("STOMP");
                    enemy.Kill("stomp");
                    playerScript.BouncePlayer(enemy.GetBounceForce());
                }else{
                    manager.HandlePlayerDeath();
                }
            break;
            case("Explosion"):
                enemy.Kill("spun"); //might change this later...
            break;
            case("Death_Barrier"): //Kill enemy if they somehow get out of bounds
                enemy.Kill("spun");
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
        
}
