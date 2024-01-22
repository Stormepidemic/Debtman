using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistanceChange : Volume
{
    // Start is called before the first frame update
    private GameObject cam;
    [SerializeField] private float distance;
    [SerializeField] private float height;
    [SerializeField] private float rotationFactor;
    void Start()
    {
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        
        //If the rotation factor isn't set, just use the Camera's default.
        if(rotationFactor == null || rotationFactor == 0.0f){
            rotationFactor = cam.GetComponent<CameraMove>().GetRotationFactor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(cam == null){
            cam = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        }
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"){
            cam.GetComponent<CameraMove>().distanceFromPlayer = distance;
            cam.GetComponent<CameraMove>().SetHeight(height);
            cam.GetComponent<CameraMove>().SetRotationFactor(rotationFactor);
        }
    }

    void OnTriggerExit(){
        cam.GetComponent<CameraMove>().resetDistance();
    }
}
