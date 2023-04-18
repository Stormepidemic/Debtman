using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantTexture : MonoBehaviour
{
    //This script is used to add automatic texture variation to objects
    [SerializeField] Material[] textures;
    // Start is called before the first frame update
    void Start()
    {
        int value = Random.Range(0, textures.Length);
        gameObject.GetComponent<Renderer>().material = textures[value];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
