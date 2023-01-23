using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackAndForthMovement : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject platform;
    public float speed;
    public Boolean waitForPlayer = false; //Will only move when the player activates it.
    private float distanceBetweenPoints;

    // Start is called before the first frame update
    void Start()
    {
        //Disable the visibility of the start and end points
        startPoint.GetComponent<Renderer>().enabled = false;
        endPoint.GetComponent<Renderer>().enabled = false;
        distanceBetweenPoints = Vector3.Distance(startPoint.transform.position, endPoint.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(waitForPlayer!){
            Vector3 platformPos = platform.transform.position;
            
            if(Vector3.Distance(platformPos, endPoint.transform.position) > 0.1*distanceBetweenPoints){
                platform.transform.position = Vector3.MoveTowards(platformPos, endPoint.transform.position, Time.deltaTime*speed);
            }else{
                platform.transform.position = Vector3.Lerp(platformPos, endPoint.transform.position, Time.deltaTime*speed);
            }
            if(Vector3.Distance(platformPos, endPoint.transform.position) < 0.1f){
                GameObject tempPoint = startPoint;
                startPoint = endPoint;
                endPoint = tempPoint;
            }
        }
    }
}
