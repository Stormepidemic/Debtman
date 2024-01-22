using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetEnemy : ResettableElement
{
    [SerializeField] private GameObject enemy; //The actual, living enemy in the scene. Whatever the prefab would spawn. 
    //[SerializeField] private Enemy instance;
    [SerializeField] private EnemyBase instance;
    [SerializeField] private GameObject[] DestroyObjects; //Additional objects that get destroyed when this element has to reset.
    private int destructionWeight;

    void Start(){
        destructionWeight = instance.GetDestructionWeight();
    }

    public override void ResetElement(){
        //if(enemy == null || !enemy.activeInHierarchy || !instance.GetAlive()){ //Only reset if the enemy has been killed.
        if(determineStatus()){
            if(enemy != null){
                Destroy(enemy);
            }
            //print((enemy == null) + " " + !enemy.activeInHierarchy + " " + !instance.GetAlive());
            enemy = Instantiate(this.prefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent); //Respawn the enemy prefab!
            //instance = enemy.GetComponent<EnemyBase>();
            GameObject.Find("GameManager").GetComponent<GameManager>().DecrementDestruction(destructionWeight);
        }else{
            Destroy(enemy);
            resetAdditionalObjects();
            enemy = Instantiate(this.prefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent); //Reset the enemy if it is still alive
            //instance = enemy.GetComponent<EnemyBase>();
        }
        
        
    }

    public override void DestroyElement(){
        //if(enemy == null || !enemy.activeInHierarchy || !instance.GetAlive()){ //Never respawn the enemy if it has been killed, and the player has reached the checkpoint
        if(determineStatus()){
            Destroy(gameObject);
        }
    }

    public override Boolean GetElementStatus(){
        //if(enemy == null || !enemy.activeInHierarchy || ((instance != null) && (!instance.GetAlive()))){
        if(determineStatus()){
            return false;
        }else{
            return true;
        }
    }

    private void resetAdditionalObjects(){
        foreach(GameObject obj in DestroyObjects){
            Destroy(obj);
        }
    }

    private bool determineStatus(){
        return (enemy == null);
    }

}
