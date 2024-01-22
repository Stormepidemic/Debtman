using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollLightmap: MonoBehaviour
{
    [SerializeField] private float ScrollX = 0.5f;
    [SerializeField] private float ScrollY = 0.5f;

    // Update is called once per frame
    void Update()
    {
        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;
        GetComponent<Renderer>().material.SetTextureOffset("_LightMap", new Vector2(OffsetX, OffsetY));
    }
}
