using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistanceChange : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject cam;
    [SerializeField] private float distance;
    [SerializeField] private float height;
    void Start()
    {
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    // Update is called once per frame
    void Update()
    {
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"){
            cam.GetComponent<CameraMove>().distanceFromPlayer = distance;
            cam.GetComponent<CameraMove>().SetHeight(height);
        }
    }

    void OnTriggerExit(){
        cam.GetComponent<CameraMove>().resetDistance();
    }
}
