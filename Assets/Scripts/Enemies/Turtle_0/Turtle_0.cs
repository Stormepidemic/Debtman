using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Turtle_0 : Enemy
{
    public GameObject startPoint;
    public GameObject endPoint;
    public float speed;
    private Animator anim;
    public GameObject Turtle;
    private Boolean isMove = true;
    [SerializeField] private GameObject hurtbox;
    // Start is called before the first frame update
    void Start()
    {
        //Disable the visibility of the start and end points
        startPoint.GetComponent<Renderer>().enabled = false;
        endPoint.GetComponent<Renderer>().enabled = false;
        //gameObject.transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMove){
            Vector3 pos = gameObject.transform.position;
            gameObject.transform.position = Vector3.MoveTowards(pos, endPoint.transform.position, speed);
            if(Vector3.Distance(pos, endPoint.transform.position) < 0.1f){
                GameObject tempPoint = startPoint;
                startPoint = endPoint;
                endPoint = tempPoint;
                gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
            }
        }
    }

    public void isMoving(Boolean canMove){
        isMove = canMove;
    }

    public override void Reset(){
        isMoving(true);
        hurtbox.GetComponent<KillEnemy>().Reset();
    }

    public override void kill(){
        isMoving(false);
        hurtbox.GetComponent<KillEnemy>().kill();
        
    }

    public override void Disable(){
        GameObject manager = GameObject.Find("GameManager");
        manager.GetComponent<GameManager>().PopulateDisabledObjects("enemy", gameObject);
        //hurtbox.GetComponent<KillEnemy>().Disable();
        isMoving(false);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
