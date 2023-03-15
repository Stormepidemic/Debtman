using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
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
    [SerializeField] private GameObject manager;
    [SerializeField] private GameObject bodyParticles;
    //[SerializeField] private GameObject landFromJumpParticles;
    [SerializeField] private GameObject spinParticles;
    [SerializeField] private float manualDrag;
    [SerializeField] private int immunityTime; //How long the player should be invincible after taking damage
    [SerializeField] private int immunityCounter;
    private Vector3 previousDirection;
    private Boolean alive = true; //If the player is alive or not...
    [SerializeField] private int waitFrameCounter; //This counter is used in order to determine when a Waiting animation should play

    // Start is called before the first frame update
    void Awake()
    {
        StartingRotation = transform.rotation;
        animator.SetBool("Spawn",true);
        currentSpeed = speed; //Default speed
    }
    
    void Update(){
        manager = GameObject.Find("GameManager");
        if(Time.timeScale > 0){
            if(canMove){
            animator.SetBool("Spawn",false);
            HandleAnimator();
            HandleJump();
            HandleSpin();
            HandleWaiting();
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

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Explosion"){
            if(alive){
                manager.GetComponent<GameManager>().HandlePlayerDeath(); //KILL the player!
                alive = false;
            }
            
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
        if(!isGrounded){
            currentSpeed = speed / 1.1f;
        }else{
            currentSpeed = speed;
        }
        Vector3 MoveVector = PlayerMovementInput * currentSpeed;
        
        if(PlayerMovementInput == Vector3.zero){
            PlayerBody.velocity = Vector3.Lerp(PlayerBody.velocity, new Vector3(0.0f, PlayerBody.velocity.y, 0.0f), Time.deltaTime*manualDrag);
        }else{
            if(PlayerMovementInput.x == -previousDirection.x || PlayerMovementInput.z == -previousDirection.z){
                PlayerBody.velocity = Vector3.Lerp(new Vector3(previousDirection.x, PlayerBody.velocity.y, previousDirection.z), new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z), Time.deltaTime*currentSpeed);
            }else{
                PlayerBody.velocity = Vector3.Lerp(new Vector3(0.0f, PlayerBody.velocity.y, 0.0f), new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z), Time.deltaTime*currentSpeed);
            }
            PlayerBody.AddForce(new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z));
            
        }
        //PlayerBody.MovePosition(transform.position + MoveVector * Time.deltaTime);
        if(PlayerMovementInput != Vector3.zero){
            RotatePlayer();
        }

        previousDirection = PlayerMovementInput;
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
            if(isGrounded){
                animator.SetBool("Running", true);
            }else{
                animator.SetBool("Rising",true);
            }
        }else{
            animator.SetBool("Rising", false);
        }
        if(PlayerBody.velocity.y < -1.0f){ //FALLING
            if(isGrounded){
                animator.SetBool("Running", true);
            }else{
                animator.SetBool("Falling", true);
            }
        }else{
            animator.SetBool("Falling", false);
        }
    }
    
    //Handles what happens when the player jumps
    private void HandleJump(){
        RaycastHit hit;
        Vector3 adjPosition = new Vector3(transform.position.x - 0.1f, transform.position.y + 0.1f, transform.position.z);
        Physics.Raycast(adjPosition, -Vector3.up, out hit, Mathf.Infinity);
        if(Input.GetKeyDown(KeyCode.Space)){ //Jump
            if((hit.collider != null) && (hit.collider.tag == "Ground")){
                float distanceFromGround = Math.Abs(hit.point.y - transform.position.y);
                if(distanceFromGround < 0.12f){

                    isGrounded = true;
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
        alive = true;
    }

    //This function is used to count and handle when/if a waiting animation should play
    //A waiting animation should play when the Player hasn't moved for a considerable amount of time.
    void HandleWaiting(){
        int waitTime = 2400; //The amount of frames the Idle animation can play before the waiting animation will play
        Boolean hasRun = animator.GetBool("Running");
        Boolean hasJumped = animator.GetBool("Rising") || animator.GetBool("Falling");
        Boolean hasDied = animator.GetBool("Spawn") || animator.GetBool("Death");
        if((!hasRun && !hasJumped && !hasDied || (animator.GetInteger("Waiting") != 0))){
            if(waitFrameCounter > waitTime){
                waitFrameCounter = 0;
                //LIKELY TO REPLACE LATER WITH A RANDOM SELECTON SYSTEM
                animator.SetInteger("Waiting", 1);
            }else{
                waitFrameCounter++; //Increment the frame count
            }
        }else{
            animator.SetInteger("Waiting", 0);
            waitFrameCounter = 0; //Reset the timer
        }
    }
}
