using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInVolume : MonoBehaviour
{
    [SerializeField] GameObject CameraOverlay;
    private GameObject player;
    // Start is called before the first frame update

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            player = other.gameObject;
            CameraOverlay.GetComponent<FadeTexture>().FadeIn(); //Fade in the overlay
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            CameraOverlay.GetComponent<FadeTexture>().FadeOut(); //Fade out the overlay
        }
    }

    void Update(){
        if(player == null){
            CameraOverlay.GetComponent<FadeTexture>().FadeOut(); //Fade out the overlay
        }
    }
}
