using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinRotator : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0){
            gameObject.transform.Rotate(0.0f, 0.0f, 100.0f, Space.Self);
        }
    }
}
