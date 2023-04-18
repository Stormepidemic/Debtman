using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerDeathEffects : MonoBehaviour
{   
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private Rigidbody[] bodyParts;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Rigidbody part in bodyParts){
            //part.AddExplosionForce(10.0f, part.gameObject.transform.position, 1.0f, 1.0f, ForceMode.Impulse);
            part.AddForceAtPosition(-part.gameObject.transform.up*100, transform.position);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = playerCharacter.transform.rotation;

        
    }


}
