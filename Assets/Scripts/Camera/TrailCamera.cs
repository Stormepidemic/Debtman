using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCamera : CameraBase
{
    private bool locked;
    private GameObject lockPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //recenter the camera to the Target
    public override void centerCamera(){
        
    }

    public override void resetDistance(){
        
    }

    public override void Lock(GameObject lockPoint){
        locked = true;
        this.lockPoint = lockPoint;
    }

    public override void Unlock(){
        locked = false;
    }

    public override void SetCameraTarget(Transform target){
        //stub
    }
    public override void SetCameraLookTarget(Transform target){
        //stub
    }
    public override void ShakeCamera(float strength, float length, Transform position){
        //stub
    }


}
