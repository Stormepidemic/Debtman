using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [SerializeField] private GameObject lockPoint;
    private CameraMove cameraScript;

    void Start()
    {
        cameraScript = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraMove>();
    }

    void Update(){
        if(cameraScript == null){
            cameraScript = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraMove>();
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            cameraScript.Lock(lockPoint);
        }
    } 

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            cameraScript.Unlock();
        }
    } 
}
