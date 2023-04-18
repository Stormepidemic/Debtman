using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractCollectAction : MonoBehaviour
{
    [SerializeField] GameObject parentObject;
    

    void CollectContract(){
        Destroy(parentObject);
    }
}
