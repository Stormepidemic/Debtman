using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DestructionCounter : MonoBehaviour
{
    public Texture zero,one,two,three,four,five,six,seven,eight,nine;
    [SerializeField] private Texture percentageSign; //The literal '%' symbol
    private Texture[] numbers; //Our numbers 0 thru 9
    public Texture blank; //a blank, completely transparent image used to represent a number which should not be displayed, Ex: In the value 009, the leading zeros will be shown with this blank texture.
    [SerializeField] private RawImage percentPlace; //When at 100%, we need a 4th character.
    [SerializeField] private RawImage hundredsPlace; //Should either be BLANK or a '1' texture
    [SerializeField] private RawImage tensPlace;
    [SerializeField] private RawImage onesPlace;
    [SerializeField] private GameManager manager;
    [SerializeField] private float percentageTest;
    [SerializeField] private Gem_Fragment_Icon icon;

    private float currentValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        numbers = new Texture[]{zero,one,two,three,four,five,six,seven,eight,nine}; //array of textures representing our numbers
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        setNumber();
        HandleStage();
    }
    //TO DO: Make this work similarly to how the Scrap counter works, where it moves through each number between the current number and the end number
    private void setNumber(){
        float percent = manager.GetDestructionPercentage() * 100.0f + 0.01f; //manager.GetDestructionPercentage();
        string percentString = "" + percent;
        //print(percentString);
        if(percent < 10.0f){
            percentPlace.texture = blank; 
            onesPlace.texture = percentageSign; //Percent Sign in the 10's place while less than 10
            //print("percent string: " + percentString);
            //print("Des Counter:"  + Int32.Parse("" + percentString[0]));
            tensPlace.texture = numbers[Int32.Parse("" + percentString[0])];
            hundredsPlace.texture = blank;

        }else if(percent < 100.0f){
            percentPlace.texture = blank;
            onesPlace.texture = percentageSign; //Percent sign in the 1's place while less than 100
            tensPlace.texture = numbers[Int32.Parse("" + percentString[1])]; //Ones place
            hundredsPlace.texture = numbers[Int32.Parse("" + percentString[0])]; //Tens place

        }else{
            percentPlace.texture = percentageSign; // %
            onesPlace.texture = numbers[0]; // 0
            tensPlace.texture = numbers[0]; // 0
            hundredsPlace.texture = numbers[1]; // 1

        }
        currentValue = manager.GetDestructionPercentage() * 100.0f;
    }

    /*
        Handle switching the 'Stage' of the icon for this counter.
    */
    private void HandleStage(){
        if(currentValue < 33.0f){ //Base stage
            icon.SetStage(0);
        }else if((currentValue >= 33.0f) && (currentValue <= 66.6f)){
            icon.SetStage(1);
        }else{
            icon.SetStage(2);
        }
    }
}
