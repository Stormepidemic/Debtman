using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDestructibleAction : MonoBehaviour
{
    [SerializeField] private GameObject breakBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyDestructible(){
        breakBox.GetComponent<Destructible>().Break();
    }
}
