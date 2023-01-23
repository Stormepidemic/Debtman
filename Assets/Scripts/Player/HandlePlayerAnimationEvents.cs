using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlayerAnimationEvents : MonoBehaviour
{

    private void DoFootStep(){
        GameObject parent = gameObject.transform.parent.gameObject;
        AudioSource audio = parent.GetComponent<AudioSource>();
        audio.pitch = Random.Range(0.75f, 1.0f);
        audio.Play();
    }

    private void DoRespawn(){
        GameObject parent = gameObject.transform.parent.gameObject;
        GameObject manager = GameObject.Find("GameManager");
        manager.GetComponent<GameManager>().HandlePlayerRespawn();
        print("HEREE!");
    }

    
}
