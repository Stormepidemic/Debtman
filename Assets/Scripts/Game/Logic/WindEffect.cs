using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : Volume
{
    PlayerMovement playerInstance;
    [SerializeField] Vector3 windDirection;
    [SerializeField] float strength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerInstance != null){
            playerInstance.ApplyExternalForce(windDirection, strength);
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.GetComponent<PlayerMovement>() != null){
            playerInstance = other.GetComponent<PlayerMovement>();
        }
    }

    void OnTriggerExit(Collider other){
        playerInstance = null;
    }

    void OnTriggerStay(Collider other){
        if(other.GetComponent<PlayerMovement>() != null){
            playerInstance = other.GetComponent<PlayerMovement>();
        }
    }
}
