using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 upVector = -Vector3.up;
    [SerializeField] private float x = 90.0f, y = 0.0f, z = 0.0f;

    void Start(){
        target = GameObject.Find("Player_Character");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target == null){
            target = GameObject.Find("Player_Character");
        }
        gameObject.transform.LookAt(target.transform, upVector);
        gameObject.transform.Rotate(x, y, z, Space.Self);
    }
}
