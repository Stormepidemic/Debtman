using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives_Counter : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    public Text counter;
    // Start is called before the first frame update
    void Start()
    {
        //counter = gameObject.GetComponent<Text>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        counter.text = "" + manager.GetLives();
    }
}
