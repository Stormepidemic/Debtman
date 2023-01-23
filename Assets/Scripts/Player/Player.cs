using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    public int playerFrameCounter = 0;
    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private Collider collider;  
    [SerializeField] private float speed;
    //[SerializeField] private float inAirSpeed;
    private float currentSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject armature;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Animator animator;
    private Quaternion StartingRotation;
    public Boolean canMove = false;
    [SerializeField] private GameObject mainModel;
    [SerializeField] private GameObject spinModel;
    private int spinValue = 0; //Starts at 0, handles how often the player can spin
    private int lastSpinFrame = 0;
    [SerializeField] private int spinDelay = 30;
    public Boolean enemyImmunity = false;
    private Boolean isGrounded;
    private Boolean sprinting;
    [SerializeField] private GameObject manager;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject spinParticles;
    [SerializeField] private float manualDrag;
    [SerializeField] private Material[] damageStateMaterials;
    private int damageState;
    [SerializeField] private int immunityTime; //How long the player should be invincible after taking damage
    [SerializeField] private int immunityCounter;
    // Start is called before the first frame update
    void Awake()
    {
        StartingRotation = transform.rotation;
        animator.SetBool("Spawn",true);
        currentSpeed = speed; //Default speed
    }
    
    void Update(){
        if(Time.timeScale > 0){
            if(canMove){
            animator.SetBool("Spawn",false);
            HandleAnimator();
            HandleJump();
            HandleSpin();
            }

            //Spin Logic
            playerFrameCounter = playerFrameCounter + 1; //Counts the amount of frames the player has been active for
            if(spinValue < 101){
                spinValue = spinValue + 1; //regen spin
            }
        }
        //decrementImmunityTimer();
    }

    void FixedUpdate(){
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if(Mathf.Abs(PlayerMovementInput.x) > 0.0f && Mathf.Abs(PlayerMovementInput.z) > 0.0f){ //Prevents diagonal movement from being drastically faster
            PlayerMovementInput = PlayerMovementInput.normalized;
        }
        //PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if(Time.timeScale > 0){ //Only run if the game is not paused
            //Movement Logic
            if(canMove){
            animator.SetBool("Spawn",false);
            MovePlayer();
            }
        }
    }
    //Switches the player's parent to make their movement relative to the ground
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Ground"){
            gameObject.transform.SetParent(collision.gameObject.transform);
            isGrounded = true;
        }
    }

    void OnCollisionStay(Collision collision){
        if(collision.gameObject.tag == "Ground"){
            gameObject.transform.SetParent(collision.gameObject.transform);
            isGrounded = true;
        }
    }
    
    //Switches the player's parent to make their movement relative to the ground
    void OnCollisionExit(Collision collision){
        gameObject.transform.SetParent(null);
        isGrounded = false;
        Vector3 scale = new Vector3(1,1,1); //Resets the scale because it gets messed up due to floating point precision
        transform.localScale = scale; 
    }
    
    //Moves the player based on the inputted direction, calls other functions needed to handle input logic
    private void MovePlayer(){
        //Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * speed;
        Vector3 MoveVector = PlayerMovementInput * currentSpeed;
        //Stops the player from "overstepping", when the movement vector is 0 it just stops the player from moving in all directions except falling
        
        if(PlayerMovementInput == Vector3.zero){
            PlayerBody.velocity = Vector3.Lerp(PlayerBody.velocity, new Vector3(0.0f, PlayerBody.velocity.y, 0.0f), Time.deltaTime*manualDrag);
        }else{
            //PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
            PlayerBody.velocity = Vector3.Lerp(new Vector3(0.0f, PlayerBody.velocity.y, 0.0f), new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z), Time.deltaTime*speed);
        }
        if(PlayerMovementInput != Vector3.zero){
            RotatePlayer();
        }
    }

    //Handles how the player rotates to face the direction in which the player is moving
    private void RotatePlayer(){
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(-PlayerMovementInput.x, PlayerMovementInput.y, -PlayerMovementInput.z),Vector3.up);
        Quaternion newRotation = Quaternion.RotateTowards(gameObject.transform.rotation, toRotation, rotationSpeed);
        //newRotation *= Quaternion.Euler(-90, 0, 90);
        gameObject.transform.rotation = newRotation;
    }

    //Handles interacting with the Animator and setting the needed values
    private void HandleAnimator(){
        if(PlayerMovementInput != Vector3.zero){
            //Running
            animator.SetBool("Running", true);
        }else{
            animator.SetBool("Running", false);
        }
        if(PlayerBody.velocity.y > 1.0f){ //RISING
            animator.SetBool("Rising",true);
        }else{
            animator.SetBool("Rising", false);
        }
        if(PlayerBody.velocity.y < -1.0f){ //FALLING
            animator.SetBool("Falling", true);
        }else{
            animator.SetBool("Falling", false);
        }
    }
    
    //Handles what happens when the player jumps
    private void HandleJump(){
        RaycastHit hit;
        Vector3 adjPosition = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        Physics.Raycast(adjPosition, -Vector3.up, out hit, Mathf.Infinity);
        if(Input.GetKeyDown(KeyCode.Space)){ //Jump
            if((hit.collider != null) && (hit.collider.tag == "Ground")){
                float distanceFromGround = Math.Abs(hit.point.y - transform.position.y);
                if(distanceFromGround < 0.05f){
                    PlayerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }
        }
    }

    //Handles what happens when the player spawns in
    public void HandlePlayerSpawn(){
        canMove = true;
    }

    //Handles the player using the spin move
    public void HandleSpin(){
        if(Input.GetKeyDown(KeyCode.LeftShift) && ((spinValue-30) > 0)){
            spinValue = spinValue - 60;
            lastSpinFrame = playerFrameCounter;
            mainModel.SetActive(false);
            spinModel.SetActive(true);
            spinParticles.GetComponent<ParticleSystem>().Play();
            
            //Disables the main hitbox
            enemyImmunity = true;
        }else{
            if((playerFrameCounter - lastSpinFrame) > spinDelay){ //Handles the delay such that the player can spin longer than 1 frame
                mainModel.SetActive(true);
                spinModel.SetActive(false);
                spinParticles.GetComponent<ParticleSystem>().Stop();
                //Reenables the main hitbox
                enemyImmunity = false;
            }
        } 
    }

    public void HandleDeath(){
        animator.SetBool("Death",true);
        PlayerBody.useGravity = false;
        collider.enabled = false;

    }
    public void HandleRespawn(){
        animator.SetBool("Death",false);
        animator.SetBool("Spawn",true);
        PlayerBody.useGravity = true;
        collider.enabled = true;
    }
    // public void HandleSprint(){
    //     if(Input.GetKey(KeyCode.LeftControl)){
    //         if(manager.GetComponent<GameManager>().GetScore() > 0 && isGrounded){
    //             manager.GetComponent<GameManager>().DecrementScore(1);
    //             sprinting = true;
    //             currentSpeed = sprintSpeed;
    //         }
    //     }else{
    //         currentSpeed = speed;
    //         sprinting = false;
    //     }
    // }

    // private void SetDamageMaterial(){
    //     mainModel.GetComponent<Renderer>().material = damageStateMaterials[damageState];
    // }
    // //Returns 0 if the player is in the base damage state, returns 1 if it can successfully move down a damage state (take damage without dying)
    // public int DealDamage(){
    //     int returnValue = 0;
    //     if(immunityCounter == 0){
    //         if(damageState > 0){
    //             damageState = damageState - 1;
    //             SetDamageMaterial();
    //             returnValue = 1;
    //             immunityCounter = immunityTime;
    //     }else if(damageState == 0){
    //         damageState = 0;
    //         returnValue = 0;
    //         print("damageState = " + damageState);
    //         }
    //     }
        
    //     return returnValue;
    // }

    // public int GetDamageState(){
    //     return damageState;
    // }

    // public void SetDamageState(int state){
    //     damageState = state;
    //     SetDamageMaterial();
    // }

    // private void decrementImmunityTimer(){
    //     if(immunityCounter > 0){
    //         immunityCounter -= 1;
    //     }else{
    //         immunityCounter = 0;
    //     }
    // }
}
