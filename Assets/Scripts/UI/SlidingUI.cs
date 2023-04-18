using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlidingUI : MonoBehaviour
{
    [SerializeField] Vector2 offsetVector;

    [SerializeField] float timeToRemain; //How long this UI remains until it slides back
    float timer;

    [SerializeField] Vector2 desiredLocation;
    Vector2 currentPosition;

    private Vector2 startPosition;

    [SerializeField] Boolean showing;
    [SerializeField] float moveSpeed;

    [SerializeField] GameObject slidePoint;
    void Start(){
        //offsetVector = new Vector2(offsetX, offsetY);
        startPosition = new Vector2(0.0f, 0.0f);
        desiredLocation = slidePoint.GetComponent<RectTransform>().anchoredPosition;
        currentPosition = startPosition;

    }

    public void Hide(){
        desiredLocation = slidePoint.GetComponent<RectTransform>().anchoredPosition;
        showing = false;
    }

    public void Show(){
        desiredLocation = startPosition;
        showing = true;
        timer = 0;
    }

    void Update(){
        
        if(showing){
                //Show();
                gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition, desiredLocation, moveSpeed*Time.deltaTime);
            }else{ 
                //Hide();
                gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition, desiredLocation, moveSpeed*Time.deltaTime);
            }
                
            if(timer > timeToRemain){
                Hide();
            }
            timer += Time.deltaTime;
            
        }
        
        
    }


