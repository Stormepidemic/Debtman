using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private GameObject walkParticles;
    [SerializeField] private Animator animator;
    [SerializeField] private Animate_Blink faceScript;

    private void DoFootStep(){
        GameObject parent = gameObject.transform.parent.gameObject;
        AudioSource audio = parent.GetComponent<AudioSource>();
        audio.pitch = Random.Range(0.75f, 1.0f);
        audio.Play();
        walkParticles.GetComponent<ParticleSystem>().Play();
    }

    private void DoRespawn(){
        GameObject parent = gameObject.transform.parent.gameObject;
        GameObject manager = GameObject.Find("GameManager");
        manager.GetComponent<GameManager>().HandlePlayerRespawn();
        print("HEREE!");
    }

    //A wait animation should end naturally via playermovement, but we need this to move from the waiting animation back to the standard idle animation
    private void EndWaitingAnimation(){
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetInteger("Waiting", 0);
    }

    private void EndDoubleJumpAnimation(){
        animator.SetBool("Double Jump", false);
    }

    private void ChangeFace(int faceNumber){
        faceScript.SetFacialExpression(faceNumber);
    }

    
}
