using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_Door : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem[] effects;
    [SerializeField] private GameObject light;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            anim.SetBool("Activated", true);
            foreach(ParticleSystem effect in effects){
                effect.Play();
            }
            light.SetActive(true);
        }

        
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            anim.SetBool("Activated", false);
            foreach(ParticleSystem effect in effects){
                effect.Stop();
            }
            light.SetActive(false);
        }
    }   
}
