using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [SerializeField] private GameObject lockPoint;
    private CameraMove cameraScript;
    [SerializeField] private float rotationFactor;

    void Start()
    {
        cameraScript = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraMove>();

        //If the rotation factor isn't set, just use the Camera's default.
        if(rotationFactor == null || rotationFactor == 0.0f){
            rotationFactor = cameraScript.GetComponent<CameraMove>().GetRotationFactor();
        }
    }

    void Update(){
        if(cameraScript == null){
            cameraScript = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraMove>();
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            cameraScript.Lock(lockPoint);
            cameraScript.GetComponent<CameraMove>().SetRotationFactor(rotationFactor);
        }
    } 

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            cameraScript.Unlock();
            cameraScript.resetDistance();
        }
    } 
}
