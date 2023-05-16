using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Platform : Interactible
{
    private bool activated;
    [SerializeField] private GameObject[] pathPoints;
    private GameObject nextPoint;
    private int pathIndex; //The index of nextPoint in pathPoints
    private bool moving;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        pathIndex = 1;
        foreach(GameObject point in pathPoints){
            point.GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(moving && activated){
            //Vector3 platformPos = gameObject.transform.position;
            //gameObject.transform.position = Vector3.Lerp(pathPoints[pathIndex-1].transform.position, nextPoint.transform.position, Time.deltaTime*speed);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextPoint.transform.position, Time.deltaTime*speed);
            if(Vector3.Distance(gameObject.transform.position, nextPoint.transform.position) < 0.1f){
                if(pathIndex+1 < pathPoints.Length){
                    pathIndex++;
                    nextPoint = pathPoints[pathIndex];
                }else{
                    moving = false;
                }
                
            }
        }
    }

    public override void Activate(){
        activated = true;
        moving = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().PopulateDisabledObjects("interactible", gameObject);
        nextPoint = pathPoints[1]; //The first target point is set
        pathIndex = 1; 
    }

    public override void Deactivate(){
        moving = false;
        activated = false;
        gameObject.transform.position = pathPoints[0].transform.position; //Reset to the first point
    }

    public override bool GetActive(){
        return activated;
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Player"){
            if(!moving && !activated){
                print("HERE");
                Activate();
            }
            
        }
    }
}
