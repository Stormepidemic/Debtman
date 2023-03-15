using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable_Object : MonoBehaviour
{
    [SerializeField] private Material inactive;
    [SerializeField] private Material active;
    [SerializeField] private ParticleSystem emittor;
    [SerializeField] private ParticleSystem burst;
    [SerializeField] private GameObject model;

    public void Activate(){
        model.GetComponent<Collider>().enabled = true;
        model.GetComponent<Renderer>().material = active;
        emittor.Play();
        burst.Play();
    }

    public void Deactivate(){
        model.GetComponent<Collider>().enabled = false;
        model.GetComponent<Renderer>().material = inactive;
        emittor.Stop();
        
    }
}
