using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDestructableAction : MonoBehaviour
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

    void DestroyDestructable(){
        breakBox.GetComponent<Destructable>().Break();
    }
}
