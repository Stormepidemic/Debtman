using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetDestructable : ResettableElement
{
    [SerializeField] private GameObject destructable; //The actual spawned destructable in the scene

    void Start(){
        gameObject.transform.rotation = destructable.transform.rotation;
        gameObject.transform.position = destructable.transform.position;
        gameObject.transform.localScale = destructable.transform.localScale;
    }

    public override void ResetElement(){
        if(destructable == null){ //Only reset if the enemy has been killed.
            destructable = Instantiate(this.prefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent); //Respawn the enemy prefab!
        }else{
            Destroy(destructable);
            destructable = Instantiate(this.prefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent); //Reset the enemy if it is still alive
        }
        
    }

    public override void DestroyElement(){
        if(destructable == null){ //Never respawn the enemy if it has been killed, and the player has reached the checkpoint
            Destroy(destructable);
        }
    }

    public override Boolean GetElementStatus(){
        if(destructable == null){
            return false;
            print("DESTROYED");
        }else{
            return true;
        }
    }
}
