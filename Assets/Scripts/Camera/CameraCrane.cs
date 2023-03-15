using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCrane : MonoBehaviour
{
    /*
    This script handles the logic of moving a gameObject to different positions. This is done so we don't actually have
    to recalculate the distance between the Camera and the offset from the CameraNode position whenever the player moves.
    */

    private GameObject currentNode;
    [SerializeField] private float moveSpeed;
    public float smoothSpeed = 0.02f; //The smoothDamp variable
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move(){
        var step = moveSpeed * Time.deltaTime;
        var desiredPos = Vector3.MoveTowards(gameObject.transform.position, currentNode.transform.position, step);
        gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, desiredPos, ref velocity, smoothSpeed);
    }

    public void setCurrentNode(GameObject node){
        currentNode = node;
    }
}
