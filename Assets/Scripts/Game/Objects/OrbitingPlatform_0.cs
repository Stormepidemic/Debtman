using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrbitingPlatform_0 : MonoBehaviour
{
    [SerializeField] private GameObject platform1;
    [SerializeField] private GameObject platform2;
    [SerializeField] private float distanceFromMiddle;
    [SerializeField] private float speed;
    private GameObject player;
    [SerializeField] private int direction; //+ for clockwise, - for counter clockwise

    
    // Update is called once per frame
    void FixedUpdate()
    {
        platform1.transform.RotateAround(gameObject.transform.position, Vector3.up, direction*(speed * Time.deltaTime));
        platform2.transform.RotateAround(gameObject.transform.position, Vector3.up, direction*(speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Player"){
            player = collision.gameObject;
        }
    }
}
