using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effiguy_0_Actions : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftFootEffect, rightFootEffect;
    [SerializeField] private Effiguy_0 effiguyScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoLeftFootStep(){
        leftFootEffect.Play();
        effiguyScript.StepForward();
    }
    
    void DoRightFootStep(){
        rightFootEffect.Play();
        effiguyScript.StepForward();
    }
}
