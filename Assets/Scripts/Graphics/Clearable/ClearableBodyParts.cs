using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Some enemies or objects may spawn body parts or lingering effects upon their destruction or death.
* This class handles those, and connects them to being a ClearableElement which can be collected and deleted upon
* some event. 
*/
public class ClearableBodyParts : ClearableElement
{
    [SerializeField] Rigidbody[] bodyParts; //The dead body parts of this enemy that get thrown around when this enemy dies.
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.clearType = ClearableCondition.PLAYER_DEATH;
    }

    //ClearableBodyParts should be spawned when an enemy dies, so their Awake() function will be called when the body parts become active in the scene.
    void Awake(){
        player = GameObject.Find("Player");
        throwBodyparts();
    }

    public override void ClearElement(){
        Destroy(gameObject);
    }

    public override void ClearElement(ClearableCondition condition){
        if(condition == this.clearType){
            Destroy(gameObject);
        }
    }

    //Add some movement to the enemy's body parts
    private void throwBodyparts(){
        foreach(Rigidbody part in bodyParts){
            part.AddExplosionForce(10.0f, part.gameObject.transform.position, 1.0f, 1.0f, ForceMode.Impulse);
            //part.AddForceAtPosition((part.gameObject.transform.position - player.transform.position)*100, player.transform.position);
            
        }
    }
    
}
