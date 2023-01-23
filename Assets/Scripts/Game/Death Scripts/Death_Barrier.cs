using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Barrier : MonoBehaviour
{
    private GameManager manager;
    [SerializeField] private BoxCollider trigger;
    
    void Start(){
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    

    void OnTriggerExit(Collider other){
        //Calls to the Game Manager to kill the player and send them back to spawn.
        if(other.gameObject.tag == "Player"){
            manager.HandlePlayerDeath();
        }
    }

    public void reinstate(){
    }

}
