using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private GameObject target;

    void Start(){
        target = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        gameObject.transform.LookAt(target.transform, -Vector3.up);
        gameObject.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
    }
}
