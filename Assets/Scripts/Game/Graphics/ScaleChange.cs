using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : Volume
{
    [SerializeField] private GameObject obj;
    private Vector3 baseScale;
    private Vector3 currentScale;
    private bool inside;
    private GameObject player;
    [SerializeField] Vector3 toScale; //The scale that the object gets scaled to when the player enters
    [SerializeField] private float changeSpeed;
    // Start is called before the first frame update
    void Start()
    {
        baseScale = obj.transform.localScale;
        currentScale = baseScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(inside){
            currentScale = Vector3.Lerp(currentScale, toScale, Time.deltaTime*changeSpeed);
            obj.gameObject.transform.localScale = currentScale;
        }else{
            currentScale = Vector3.Lerp(currentScale, baseScale, Time.deltaTime*changeSpeed);
            obj.gameObject.transform.localScale = currentScale;
        }
        //Handle the player dying and being moved back to an area where the sunlight should be reset to it's base intensity
        if((player == null)){
            currentScale = baseScale;
            inside = false;
        }
        
    }
   


    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            inside = true;
            player = other.gameObject;
        }
    }
    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"){
            inside = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            inside = false;
            //player = null;
        }
    }
}