using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class TrackCameraMove : MonoBehaviour
{
    public GameObject track; //The "track", a parent GameObject whose children this script iterates through and moves the camera between
    public GameObject player; //The player that this camera focuses on
    private List<GameObject> nodes; //The list of nodes which the Camera will move between over the course of gameplay
    public float speed = 10f; //Speed at which the camera moves
    public float smoothSpeed = 0.02f; //The smoothDamp variable
    public float distanceFromPlayer = 7f;
    public float spaceAwayFromPlayer = 10f;
    private int targetNode; //The node of which the Camera is moving towards 
    private int previousNode; //The node the Camera is departing from
    private Vector3 velocity = Vector3.zero;
    private int respawnNode = 0; //The Camera Node where the camera gets set to upon a respawn of the player

    //When the character moves away from the camera, the camera should move towards the next node
    //When the character moves towards the camera, the camera should move towards the previous node

    // Start is called before the first frame update
    void Start()
    {
        nodes = new List<GameObject>();
        //Iterate through the children of the Camera Track and put them into a list
        foreach(Transform node in track.transform){
            nodes.Add(node.gameObject);
            node.GetChild(0).GetComponent<Renderer>().enabled = false;
        }
        resetCamera();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        float toPlayerDist = Vector3.Distance(gameObject.transform.position, player.transform.position);
        
        if((Math.Abs(toPlayerDist - distanceFromPlayer) < 0.1f)){
            //DO NOTHING
        }else if((toPlayerDist - distanceFromPlayer) < -0.5f){ //Player moving towards camera
            print("BACKWARD");
            
            if(targetNode > 0){ //Ensures we don't target a node that doesn't exist...
                
                if((Vector3.Distance(gameObject.transform.position, nodes.ElementAt(previousNode).transform.position) < 0.01f)){
                targetNode = previousNode;
                if(previousNode != 0){
                    previousNode = previousNode - 1;
                }
                
                
            }
            var step = speed * Time.deltaTime;
            var desiredPos = Vector3.MoveTowards(gameObject.transform.position, nodes.ElementAt(previousNode).transform.position, step);
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, desiredPos, ref velocity, smoothSpeed);
            setRotation(nodes.ElementAt(previousNode), 0.5f); //point the camera down when moving backwards
            }
        }else if((toPlayerDist - distanceFromPlayer) > 0.5f){ //Player moving away from camera
            print("FORWARD");
            if(targetNode < nodes.Count()-1){ //Ensures we don't target a node that doesn't exist...
        
                if((Vector3.Distance(gameObject.transform.position, nodes.ElementAt(targetNode).transform.position) < 0.01f)){
                previousNode = targetNode;
                targetNode = targetNode + 1;
                 
            }
            var step = speed * Time.deltaTime;
            var desiredPos = Vector3.MoveTowards(gameObject.transform.position, nodes.ElementAt(targetNode).transform.position, step);
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, desiredPos, ref velocity, smoothSpeed);
            setRotation(nodes.ElementAt(targetNode), 0f);
            }
        }
        
    }
    void setRotation(GameObject node, float offSet){ //Set the camera rotation to match the Camera_Node's
        //Get the forward vector
        float stepRotation = 0.01f;
        
        Vector3 target = node.transform.forward;
        Vector3 adjTarget = new Vector3(target.x, target.y - offSet, target.z);
        Vector3 newRotation = Vector3.RotateTowards(gameObject.transform.forward, adjTarget, stepRotation, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newRotation);
    }

    void setNode(int nodeNumber){

    }

    //Resets the camera to the default position
    public void resetCamera(){
        //Move the camera to the position of the 1st node
        gameObject.transform.position = nodes.ElementAt(respawnNode).transform.position;
        //gameObject.transform.LookAt(player.transform);
        targetNode = respawnNode + 1;
        previousNode = respawnNode;
        gameObject.transform.rotation = nodes.ElementAt(respawnNode).transform.rotation;
    }

    public void setRespawnNode(GameObject node){
        this.respawnNode = nodes.IndexOf(node);
        print(respawnNode);
    }
}
