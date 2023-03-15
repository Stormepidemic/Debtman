using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScrapCounter : MonoBehaviour
{
    public Texture zero,one,two,three,four,five,six,seven,eight,nine;
    private Texture[] numbers;
    public Texture blank;
    public RawImage tensPlace;
    public RawImage onesPlace;
    public int soulCount;
    public Text count;
    [SerializeField] private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        numbers = new Texture[]{zero,one,two,three,four,five,six,seven,eight,nine}; //array of textures representing our numbers
    }

    // Update is called once per frame
    void Update()
    {
        setNumber();
    }

    private void setNumber(){
        string snum = "" + manager.GetScore();
        if(snum.Length == 1){ //Single digit number
            tensPlace.texture = blank;
            onesPlace.texture = numbers[Int32.Parse(snum)];
        }else{ //double digit number
            tensPlace.texture = numbers[Int32.Parse(snum.Substring(0,1))];
            onesPlace.texture = numbers[Int32.Parse(snum.Substring(1,1))];
        }
    }
}
