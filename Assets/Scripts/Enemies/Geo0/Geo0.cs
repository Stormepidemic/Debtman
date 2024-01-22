using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Geo0 : Enemy
{
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    private Boolean isMove = true;
    [SerializeField] private Animator anim;
    [SerializeField] private float speed;
    private float distanceBetweenPoints;
    [SerializeField] GameObject deadModel;
    [SerializeField] Rigidbody[] bodyParts;
    [SerializeField] GameObject enemyModel;
    [SerializeField] GameObject[] aliveEffects;
    [SerializeField] GameObject destroyLight;
    // Start is called before the first frame update
    void Start()
    {
        startPoint.GetComponent<Renderer>().enabled = false;
        endPoint.GetComponent<Renderer>().enabled = false;
        distanceBetweenPoints = Vector3.Distance(startPoint.transform.position, endPoint.transform.position);
        alive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //This code is mostly the same as it is in BackAndForthMovement.cs, with just variables renamed for this purpose
        if(isMove && alive){
            Vector3 enemyPos = gameObject.transform.position;
            
            if(Vector3.Distance(enemyPos, endPoint.transform.position) > 0.1*distanceBetweenPoints){
                gameObject.transform.position = Vector3.MoveTowards(enemyPos, endPoint.transform.position, Time.deltaTime*speed);
            }else{
                gameObject.transform.position = Vector3.Lerp(enemyPos, endPoint.transform.position, Time.deltaTime*speed);
            }
            if(Vector3.Distance(enemyPos, endPoint.transform.position) < 0.1f){
                GameObject tempPoint = startPoint;
                startPoint = endPoint;
                endPoint = tempPoint;

                anim.SetBool("TurnAround", true);
                isMove = false;
            }
                
        }
    }
    


    public void TurnAround(){
        anim.SetBool("TurnAround", false);
        isMove = true;
    }

    public override void kill(string type){
        deadModel.SetActive(true);
        isMove = false;
        
        //Play particle effects
        GameObject player = GameObject.Find("Player_Character");
        foreach(Rigidbody part in bodyParts){
            //part.AddExplosionForce(10.0f, part.gameObject.transform.position, 1.0f, 1.0f, ForceMode.Impulse);
            part.AddForceAtPosition((part.gameObject.transform.position - player.transform.position)*200, part.transform.position);
        }
        foreach(GameObject effect in aliveEffects){
            effect.SetActive(false);
        }
        alive = false;
        GameObject.Find("GameManager").GetComponent<GameManager>().IncrementDestruction(destructionWeight);
        enemyModel.SetActive(false);
        Destroy(destroyLight, 0.5f);

    }



}
