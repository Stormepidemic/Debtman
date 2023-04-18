using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpread : MonoBehaviour
{
    [SerializeField] private Destructable des;
    [SerializeField] private GameObject spreadBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Fire" || other.gameObject.tag == "Player"){
            Invoke("Spread", 0.5f);
        }
    }

    // void OnTriggerStay(Collider other){
    //     if(other.gameObject.tag == "Fire"){
    //         Invoke("Spread", 0.5f);
    //     }
    // }

    private void Spread(){
        spreadBox.SetActive(true);
        des.Ignite();
        //spreadBox.SetActive(false); 
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Fire"){
            spreadBox.SetActive(false);
        }
    }
}
