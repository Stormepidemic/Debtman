using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMove : MonoBehaviour
{
    //The plane of movement of the character that this script listens to
    public bool x;
    public bool y;
    public bool z;
    //These values are saved so when camera movement axis get locked, they are locked to the current value
    private float lastX;
    private float lastY;
    private float lastZ;

    //public GameObject track; //The parent gameobject which holds all of the camera's movement nodes
    private List<GameObject> nodes; //The list of nodes which the Camera will move between over the course of gameplay
    private GameObject player;
    public float speed;
    private Vector3 velocity = Vector3.zero; 
    public float smoothSpeed;
    public float distanceFromPlayer;
    private float defaultDistance;
    [SerializeField] private GameObject target; //The target for this camera's movement
    [SerializeField] private GameObject lookTarget; //The target that this camera will rotate to look towards
    [SerializeField] private float defaultHeight;
    [SerializeField] private float lookSpeed;
    private float height;
    // Start is called before the first frame update
    void Start()
    {
        // nodes = new List<GameObject>();
        // //Iterate through the children of the Camera Track and put them into a list
        // foreach(Transform node in track.transform){
        //     nodes.Add(node.gameObject);
        //     //node.GetChild(0).GetComponent<Renderer>().enabled = false;
        // }
        defaultDistance = distanceFromPlayer;
        height = defaultHeight;
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     target = GameObject.Find("MainCamera_Target_0");
    //     player = GameObject.FindGameObjectsWithTag("Player")[0];
    //     Transform cam = transform;
    //     float movement = 0;
    //     if(x){
    //         movement = Math.Abs(target.transform.position.x - cam.position.x);
    //     }
    //     if(y){
    //         movement = Math.Abs(target.transform.position.y - cam.position.y);
    //     }
    //     if(z){
    //         movement = Math.Abs(target.transform.position.z - cam.position.z);
    //     }
        
    //     if(movement > 0.1f){
    //         Vector3 desiredPos = new Vector3(target.transform.position.x, height, target.transform.position.z - distanceFromPlayer);
    //         //cam.position = Vector3.MoveTowards(cam.position, desiredPos, speed);
    //         gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, desiredPos, ref velocity, smoothSpeed);
            
    //     }

        
    // }

    
    // Update is called once per frame
    void Update()
    {
        target = GameObject.Find("MainCamera_Target_0");
        lookTarget = GameObject.Find("MainCamera_LookTarget_0");
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        Transform cam = transform;
        float movement = 0;
        Vector3 desiredPos = cam.transform.position;
        //The x,y and z components of the move camera vector
        if(x){ //If x is UNLOCKED
            movement += Math.Abs(target.transform.position.x - cam.position.x);
            lastX = target.transform.position.x;
        }
        if(y){ //if y is UNLOCKED
            movement += Math.Abs(target.transform.position.y - cam.position.y);
            lastY = target.transform.position.y - cam.position.y;
        }
        if(z){ //if z is UNLOCKED
            movement += Math.Abs(target.transform.position.z - cam.position.z);
            lastZ = target.transform.position.z;
        }

        desiredPos = new Vector3(lastX, lastY+height, lastZ - distanceFromPlayer);
        if(movement > 0.1f){
        //desiredPos = new Vector3(target.transform.position.x, height, target.transform.position.z - distanceFromPlayer);
        //cam.position = Vector3.MoveTowards(cam.position, desiredPos, speed);
        gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, desiredPos, ref velocity, smoothSpeed);
        }
        setRotation(); //Rotate to point at the player
        

        
    }


    //recenter the camera to the Target
    public void centerCamera(){
        transform.LookAt(player.transform, Vector3.up);
    }

    public void resetDistance(){
        distanceFromPlayer = defaultDistance;
        height = defaultHeight;
    }   

    public void SetHeight(float h){
        height = h;
    }

    void setRotation(){
        Vector3 targetPos = lookTarget.transform.position;
        Vector3 direction = targetPos - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
    }
}
