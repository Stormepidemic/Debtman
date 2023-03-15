using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject cameraLookTarget;
    [SerializeField] private float lookSpeed;
    private GameObject[] cameraNodeObjects;
    private CameraNode[] cameraNodes;

    // Start is called before the first frame update
    void Start()
    {
        cameraLookTarget = GameObject.Find("Camera_Look_Target");
        cameraNodeObjects = GameObject.FindGameObjectsWithTag("CameraNode");
        cameraNodes = new CameraNode[cameraNodeObjects.Length];
        //This for loop just gets the scripts of the CameraNodes.
        for(int i = 0; i < cameraNodeObjects.Length; i++){
            cameraNodes[i] = cameraNodeObjects[i].GetComponent<CameraNode>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        setRotation();
        MoveCamera();
    }

    void setRotation(){
        Vector3 target = cameraLookTarget.transform.position;
        Vector3 direction = target - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
    }


    public void MoveCamera(){
        CameraCrane parentCrane = gameObject.transform.parent.gameObject.GetComponent<CameraCrane>();
        parentCrane.setCurrentNode(findNodeClosestToPlayer());
    }

    private GameObject findNodeClosestToPlayer(){
        float closestDistance = 1000.0f;
        GameObject closestNode = null;
        for(int i = 0; i < cameraNodes.Length; i++){
            if((cameraNodes[i].getPlayerDistance()) < closestDistance){
                closestDistance = cameraNodes[i].getPlayerDistance();
                closestNode = cameraNodeObjects[i];
            }
        }
        return closestNode;
    }
}
