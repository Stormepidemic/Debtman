using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Falling_Platform : MonoBehaviour
{
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;
    [SerializeField] private Boolean activated;
    [SerializeField] private GameObject platform;
    [SerializeField] private float speed;
    private int counter;
    [SerializeField] private int fallDelay;
    private Boolean respawning;
    private float distanceBetweenPoints;
    //[SerializeField] private GameObject inactiveParticles;
    [SerializeField] private GameObject activeParticles;
    private ParticleSystem inactive;
    private ParticleSystem active;
    [SerializeField] private float activeStartingRateOverTime;
    [SerializeField] private float respawnTime;
    [SerializeField] private float respawnTimer;
    

    void Start()
    {
        //Disable the visibility of the start and end points
        start.GetComponent<Renderer>().enabled = false;
        end.GetComponent<Renderer>().enabled = false;
        distanceBetweenPoints = Vector3.Distance(start.transform.position, end.transform.position);
        //inactive = inactiveParticles.GetComponent<ParticleSystem>();
        active = activeParticles.GetComponent<ParticleSystem>();
        
    }

    void FixedUpdate()
    {
        if(activated){
            active.Play();
            var em = active.emission;
            em.rateOverTime = (float)counter*10.0f;
            if(counter > fallDelay){
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
                    counter = 0;
                    respawning = true;
                    em.rateOverTime = activeStartingRateOverTime;
                    active.Stop();
                }
            }else{
                counter = counter + 1;
            }
            
        }
        if(respawning){
            if(respawnTimer > respawnTime){
                RespawnPlatform();
            }else{
                respawnTimer++;
            }
            
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            activated = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        
    }

    void RespawnPlatform(){
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
            respawning = false;
            gameObject.GetComponent<BoxCollider>().enabled = true;
            respawnTimer = 0.0f;
        }
    }
}
