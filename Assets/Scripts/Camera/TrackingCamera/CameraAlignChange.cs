using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlignChange : MonoBehaviour
{
    [SerializeField] private GameObject alignPoint;
    [SerializeField] private bool x,y,z;
    private GameObject player;
    private bool playerInside; //Is the player inside this collider?
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player==null && playerInside){ //Just in case the player dies within this volume, we need to reset it.
            GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraMove>().ResetMovementAxis();
            playerInside = false;
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraMove>().SetMovementAxis(x,y,z,alignPoint);
            playerInside = true;
            player = other.gameObject;
        }
    }
    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"){
            GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraMove>().SetMovementAxis(x,y,z,alignPoint);
            playerInside = true;
            player = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraMove>().ResetMovementAxis();
            playerInside = true;
        }
    }

}
