using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetInteractable : ResettableElement
{
    [SerializeField] private Interactable interactableObject;
    private Boolean activated;

    public override void ResetElement(){
        interactableObject.Deactivate();
        
    }

    //The name of this function isn't representative of what it actually does- but is the same for consistancy. 
    public override void DestroyElement(){
        if(interactableObject.GetActive()){
            Destroy(gameObject);
        }
        
    }


    public override Boolean GetElementStatus(){
        if(interactableObject.GetActive()){
            return false;
        }else{
            return true;
        }
    }
}
