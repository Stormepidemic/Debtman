using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private string axisOfRotation;
    [SerializeField] private float speed;
    [SerializeField] private int direction;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(axisOfRotation == "x"){
            gameObject.transform.Rotate(Time.deltaTime * speed, 0.0f, 0.0f);
        }else if(axisOfRotation == "y"){
            gameObject.transform.Rotate(0.0f, Time.deltaTime * speed, 0.0f);
        }else if(axisOfRotation == "z"){
            gameObject.transform.Rotate(0.0f, 0.0f, Time.deltaTime * speed);
        }
    }
}
