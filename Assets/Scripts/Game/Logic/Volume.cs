using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Volume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
