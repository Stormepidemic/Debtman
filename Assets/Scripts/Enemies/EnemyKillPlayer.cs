using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{
    private GameManager manager;
    // Start is called before the first frame update
    [SerializeField] private BoxCollider trigger;
    
    void Start(){
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag != "PlayerDealDamageToEnemy"){
            //Calls to the Game Manager to kill the player and send them back to spawn.
            if(other.gameObject.tag == "Player"){
                if(!other.gameObject.GetComponent<PlayerMovement>().enemyImmunity){
                //manager.HandlePlayerDamage();
                manager.HandlePlayerDeath();
            }
            }
            
        }
    }
}
