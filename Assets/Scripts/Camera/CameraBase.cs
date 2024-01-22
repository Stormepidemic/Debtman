using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void centerCamera();
    public abstract void resetDistance();
    public abstract void Lock(GameObject lockPoint);
    public abstract void Unlock();
    public abstract void SetCameraTarget(Transform target);
    public abstract void SetCameraLookTarget(Transform target);
    //Strength: How far away from the camera's standard focus point will this shake take it?
    //Length: How long until the camera returns to normal rotation functionality
    //Position: Optional, where in the world the source of the shake is.
    public abstract void ShakeCamera(float strength, float length, Transform position);
}
