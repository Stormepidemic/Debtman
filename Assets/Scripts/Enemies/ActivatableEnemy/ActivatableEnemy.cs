using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatableEnemy : EnemyBase
{
    public abstract void Activate();
    
    [SerializeField] private GameObject resetPoint;
    // Start is called before the first frame update
    void Start(){
        resetPoint.transform.position = gameObject.transform.position;
        resetPoint.transform.rotation = gameObject.transform.rotation;
        resetPoint.transform.localScale = gameObject.transform.localScale;
    }

    public abstract void Disable();
}
