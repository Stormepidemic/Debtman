using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraLifeBar : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        //counter = gameObject.GetComponent<Text>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Slider>().value = manager.GetScore()/100.0f;
    }
}
