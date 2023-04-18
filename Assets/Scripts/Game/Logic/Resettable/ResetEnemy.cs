using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetEnemy : ResettableElement
{
    [SerializeField] private GameObject enemy; //The actual, living enemy in the scene

    public override void ResetElement(){
        if(enemy == null){ //Only reset if the enemy has been killed.
            enemy = Instantiate(this.prefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent); //Respawn the enemy prefab!
        }else{
            Destroy(enemy);
            enemy = Instantiate(this.prefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent); //Reset the enemy if it is still alive
        }
        
    }

    public override void DestroyElement(){
        if(enemy == null){ //Never respawn the enemy if it has been killed, and the player has reached the checkpoint
            Destroy(gameObject);
        }
    }

    public override Boolean GetElementStatus(){
        if(enemy == null){
            return false;
        }else{
            return true;
        }
    }
}
