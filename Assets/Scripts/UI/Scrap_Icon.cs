using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scrap_Icon : MonoBehaviour
{
    GameManager manager;
    Color baseColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(manager == null){
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        Color newColor = Color.Lerp(baseColor, Color.red, manager.GetScore()/100.0f);
        gameObject.GetComponent<RawImage>().color = newColor;
    }
}
