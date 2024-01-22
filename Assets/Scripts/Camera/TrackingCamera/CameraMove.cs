using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine;

public class CameraMove : CameraBase
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
    [SerializeField] private float cameraRotationFactor;
    [SerializeField] private float DEFAULT_ROTATION_FACTOR;
    [SerializeField] private float defaultHeight;
    [SerializeField] private float lookSpeed;
    [SerializeField] private GameObject axisAlignmentObject; //Used to align the camera to a position in the world. Defaults to the Player target.
    [SerializeField] private float height;
    [SerializeField] private float heightFromPlayer;
    [SerializeField] private GameObject startingAlignmentObject;
    //Values associated with locking the camera to a spesific point
    private Boolean locked;
    private GameObject lockPoint;

    private bool shaking; //Is this camera currently shaking?
    private float shakeStrength;
    [SerializeField] private float shakeResponseTime;
    private float shakeResponseTimeCounter;

    [SerializeField] private Vector3 BackwardsVector; //The direction which would be the vector pointing to the camera, from the player's direction.
    

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
        if(axisAlignmentObject == null){
            axisAlignmentObject = target;
        }

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
        if(!locked){
            if(target != null){
                Transform cam = transform;
                float movement = 0;
                Vector3 desiredPos = cam.transform.position;
                //The x,y and z components of the move camera vector
                if(x){ //If x is UNLOCKED
                    movement += Math.Abs(target.transform.position.x - cam.position.x);
                    lastX = target.transform.position.x;
                }else{ //X is LOCKED
                    movement += Math.Abs(axisAlignmentObject.transform.position.x - cam.position.x);
                    lastX = axisAlignmentObject.transform.position.x;
                }
                if(y){ //if y is UNLOCKED
                    movement += Math.Abs(target.transform.position.y - cam.position.y);
                    lastY = target.transform.position.y - cam.position.y;

                }else{ //Y is LOCKED
                    movement += Math.Abs(axisAlignmentObject.transform.position.y - cam.position.y);
                    lastY = axisAlignmentObject.transform.position.y;
                }
                if(z){ //if z is UNLOCKED
                    movement += Math.Abs(target.transform.position.z - cam.position.z);
                    lastZ = target.transform.position.z;
                }else{ //Z is LOCKED
                    movement += Math.Abs(axisAlignmentObject.transform.position.z - cam.position.z);
                    lastZ = axisAlignmentObject.transform.position.z;
                }
                if(y){
                    desiredPos = new Vector3(lastX, target.transform.position.y+heightFromPlayer, lastZ - distanceFromPlayer);
                }else{
                    desiredPos = new Vector3(lastX, lastY+height, lastZ - distanceFromPlayer);
                }
                
                if(movement > 0.1f){
                    //desiredPos = new Vector3(target.transform.position.x, height, target.transform.position.z - distanceFromPlayer);
                    //cam.position = Vector3.MoveTowards(cam.position, desiredPos, speed);
                    gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, desiredPos, ref velocity, smoothSpeed);
                }
                if(shaking){ //If the camera is currently shaking, behave differently
                    HandleShake();
                }else{
                    setRotation(); //Rotate to point at the player
                }
            }else{
                target = GameObject.Find("MainCamera_LookTarget_0");
                lookTarget = GameObject.Find("MainCamera_LookTarget_0");
            }
                    
            
        }else{ //IS LOCKED
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, lockPoint.transform.position, ref velocity, smoothSpeed);
            if(shaking){ //If the camera is currently shaking, behave differently
                HandleShake();
            }else{
                setRotation(); //Rotate to point at the player
            }
            
        }

        

        
    }


    //recenter the camera to the Target
    public override void centerCamera(){
        transform.LookAt(player.transform, Vector3.up);
    }

    public override void resetDistance(){
        distanceFromPlayer = defaultDistance;
        height = defaultHeight;
        cameraRotationFactor = DEFAULT_ROTATION_FACTOR;
    }   

    public void SetHeight(float h){
        height = h;
    }

    // void setRotation(){
    //     Vector3 targetPos = lookTarget.transform.position;
    //     Vector3 direction = targetPos - transform.position;
    //     Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
    //     transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
    // }
    void setRotation(){
        Vector3 targetPos = new Vector3(lookTarget.transform.position.x,transform.position.y,lookTarget.transform.position.z);
        Vector3 direction = targetPos - new Vector3(transform.position.x,cameraRotationFactor,transform.position.z);
        Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
    }

    void HandleShake(){
        Vector3 targetPos = new Vector3(lookTarget.transform.position.x+UnityEngine.Random.Range(-0.25f,0.25f)*shakeStrength,
                lookTarget.transform.position.y+UnityEngine.Random.Range(-1.0f,1.0f)*shakeStrength,
                lookTarget.transform.position.z+UnityEngine.Random.Range(-0.25f,0.25f)*shakeStrength);
        Vector3 direction = targetPos - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed*shakeStrength);
        shakeResponseTimeCounter += Time.deltaTime;
        if(shakeResponseTimeCounter > shakeResponseTime){
            shaking = false;
            shakeResponseTimeCounter = 0.0f;
        }
    }

    public override void Lock(GameObject lockPoint){
        locked = true;
        this.lockPoint = lockPoint;
    }

    public override void Unlock(){
        locked = false;
    }

    public override void SetCameraTarget(Transform target){
        this.target = target.gameObject;
    }

    public override void SetCameraLookTarget(Transform target){
        lookTarget = target.gameObject;
    }
    //Change what axis of movement this camera allows. It will follow the player in X, Y and Z axis when set to True respectively.
    public void SetMovementAxis(bool x, bool y, bool z){
        this.x = x;
        this.y = y;
        this.z = z;
    }

    //Change what axis of movement this camera allows. It will follow the player in X, Y and Z axis when set to True respectively.
    public void SetMovementAxis(bool x, bool y, bool z, GameObject alignPoint){
        this.x = x;
        this.y = y;
        this.z = z;
        axisAlignmentObject = alignPoint;
    }

    public void SetAlignmentPoint(GameObject alignPoint){
        axisAlignmentObject = alignPoint;
    }

    public void ResetMovementAxis(){
        x = true;
        y = true;
        z = true;
        axisAlignmentObject = startingAlignmentObject;
    }

    public override void ShakeCamera(float strength, float length, Transform position){
        shaking = true;
        if(position != null){
            shakeStrength = strength / Vector3.Distance(position.position, target.transform.position);
        }else{
            shakeStrength = strength;
            
        }
        shakeResponseTime = length;
    }

    public float GetRotationFactor(){
        return cameraRotationFactor;
    }

    public void SetRotationFactor(float factor){
        cameraRotationFactor = factor;
    }

}
