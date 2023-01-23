using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToAndFromPlatform : MonoBehaviour
{
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;
    [SerializeField] private Boolean activated;
    [SerializeField] private GameObject barrier; //Some kind of collider to stop the player from moving off of the platform. This can be optional.
    [SerializeField] private GameObject platform;
    [SerializeField] private float speed;
    private float distanceBetweenPoints;
    void Start()
    {
        //Disable the visibility of the start and end points
        start.GetComponent<Renderer>().enabled = false;
        end.GetComponent<Renderer>().enabled = false;
        barrier.SetActive(false);
        distanceBetweenPoints = Vector3.Distance(start.transform.position, end.transform.position);
    }

    void FixedUpdate()
    {
        if(activated){
            
            Vector3 platformPos = platform.transform.position;
            
            if(Vector3.Distance(platformPos, end.transform.position) > 0.1*distanceBetweenPoints){
                platform.transform.position = Vector3.MoveTowards(platformPos, end.transform.position, Time.deltaTime*speed);
            }else{
                platform.transform.position = Vector3.Lerp(platformPos, end.transform.position, Time.deltaTime*speed);
            }
            if(Vector3.Distance(platformPos, end.transform.position) < 0.1f){
                GameObject tempPoint = start;
                start = end;
                end = tempPoint;
                activated = false;
                barrier.SetActive(activated);
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            activated = true;
            barrier.SetActive(activated);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        
    }
}
