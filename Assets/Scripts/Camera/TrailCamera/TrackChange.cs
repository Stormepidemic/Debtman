using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;
using Cinemachine;
public class TrackChange : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private CinemachinePathBase newTrack;
    [SerializeField] private bool ResetUponExit; //Reset the active track to the previous track if this is enabled
    private CinemachinePathBase originalTrack;
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
            CinemachineTrackedDolly dolly = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
            originalTrack = dolly.m_Path;
            dolly.m_Path = newTrack;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player" && ResetUponExit){
            CinemachineTrackedDolly dolly = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
            dolly.m_Path = originalTrack;
        }
    }
}
