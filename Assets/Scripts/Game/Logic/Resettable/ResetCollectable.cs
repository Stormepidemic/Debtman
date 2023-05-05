using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetCollectable : ResettableElement
{
    [SerializeField] private GameObject collectable; //The actual spawned collectable in the scene

    void Start(){
        gameObject.transform.rotation = collectable.transform.rotation;
        gameObject.transform.position = collectable.transform.position;
        gameObject.transform.localScale = collectable.transform.localScale;
    }

    public override void ResetElement(){
        if(collectable == null){ //Only reset if the collectable has been collected.
            collectable = Instantiate(this.prefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent); //Respawn the enemy prefab!
        }else{
            Destroy(collectable);
            collectable = Instantiate(this.prefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent); //Reset the enemy if it is still alive
        }
        
    }

    public override void DestroyElement(){
        if(collectable == null){ //Never respawn the enemy if it has been killed, and the player has reached the checkpoint
            Destroy(collectable);
        }
    }

    public override Boolean GetElementStatus(){
        if(collectable == null){
            return false;
        }else{
            return true;
        }
    }
}
