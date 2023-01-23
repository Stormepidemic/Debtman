using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject active; //The 'active' version of the checkpoint model
    [SerializeField] private GameObject inactive; //The 'inactive' version of the checkpoint model. Set as visible by default
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject playerSpawn;
    [SerializeField] private GameObject cameraSpawn;
    private bool activated;
    
    void Start(){
        gameManager = GameObject.Find("GameManager");
    }
    private void OnTriggerEnter(Collider other){
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(!activated){
            if(other.tag == player.tag){
            Destroy(gameObject.GetComponent<BoxCollider>());
            setRespawnPoint();
            //Activate/Deactivate the models
            active.SetActive(true); 
            inactive.SetActive(false);
            //Play sound
            //gameObject.GetComponent<AudioSource>().Play(0);
            activated = true;
            
            }
        }
    }

    private void setRespawnPoint(){
        gameManager.GetComponent<GameManager>().SetPlayerSpawn(playerSpawn, cameraSpawn);
    }
    
}
