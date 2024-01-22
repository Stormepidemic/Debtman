using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AnimationModule;
using System;
//using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    public int playerFrameCounter = 0;
    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private Collider collider;  
    [SerializeField] private float speed;
    private float currentSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject armature;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Animator animator;
    private Quaternion StartingRotation;
    public Boolean canMove = false;
    [SerializeField] private GameObject mainModel;
    //Camera targets
    [SerializeField] private GameObject mainCameraTarget;
    [SerializeField] private GameObject rollCameraTarget;

    //SPIN STUFF:
    [SerializeField] private GameObject spinModel;
    private int spinValue = 0; //Starts at 0, handles how often the player can spin
    private int lastSpinFrame = 0;
    [SerializeField] private int spinDelay;
    [SerializeField] private int spinStrength; //How 'strong' a spin is. How many frames will it decrement from the counter every time the player spins. Couldn't think of a better name for this.

    public Boolean enemyImmunity = false;
    [SerializeField] private Boolean isGrounded;
    [SerializeField] private GameObject groundSensor; //A parent gameObject which's children's positions are used to determine if the player is on the ground or not. 
    [SerializeField] private GameObject manager;
    //Some graphical stuff
    [SerializeField] private GameObject bodyParticles;
    [SerializeField] private GameObject endLevelParticles;
    [SerializeField] private GameObject playerShadow;
    //[SerializeField] private GameObject landFromJumpParticles;
    [SerializeField] private GameObject spinParticles;
    [SerializeField] private float manualDrag;
    [SerializeField] private int immunityTime; //How long the player should be invincible after taking damage
    [SerializeField] private int immunityCounter;
    private Vector3 previousDirection;
    private Boolean alive = true; //If the player is alive or not...
    [SerializeField] private int waitFrameCounter; //This counter is used in order to determine when a Waiting animation should play
    [SerializeField] private GameObject camera;

    [SerializeField] private GameObject deadModel; //The model represeting the Dead player
    [SerializeField] private GameObject characterObject;
    public float cameraProjectedMovementOffset;

    //Roll
    [SerializeField] private Collider RollCollider;
    [SerializeField] private GameObject RollCenterOfMass;
    [SerializeField] private GameObject RollForcePoint;
    [SerializeField] private Vector3 MovementForcePoint;
    private Quaternion lastDirectionBeforeRoll;
    private bool rolling;
    private Quaternion defaultRBRotation; //The default starting rotation of the rigidbody.

    //Double Jump
    private int jumpCounter; //When the player has no double jump, this is 0. When the player can double jump, it is greater than zero representing the amount of extra jumps they have.
    [SerializeField] private bool rotateToMovementDirection = true;


    //The following ParticleSystems are used to apply a effect to the Player's feet while they are sliding along the ground if there is no movement input, a sliding feet effect.
    [SerializeField] private ParticleSystem leftFootSlideEffect;
    [SerializeField] private ParticleSystem rightFootSlideEffect;

    [SerializeField] private bool[] ABILITIES = {true, true, true, true}; //Spin, Spin Hover, Double Jump, Roll,

    // Start is called before the first frame update
    void Awake()
    {
        MovementForcePoint = gameObject.transform.position;
        StartingRotation = transform.rotation;
        lastDirectionBeforeRoll = StartingRotation;
        animator.SetBool("Spawn",true);
        currentSpeed = speed; //Default speed
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        defaultRBRotation = PlayerBody.rotation;
        
    }

   
    
    void Update(){
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];

        manager = GameObject.Find("GameManager");
        this.ApplyLevelSettings();
        if(Time.timeScale > 0){
            if(canMove){
                //Animations & Movement
                animator.SetBool("Spawn",false);
                HandleAnimator();
                //Handle the player's special powers, only being able to use them when they actually have them...
                if(ABILITIES[0]){ //Spin & Spin w/ Hover Power
                    HandleSpin();
                }
                HandleJump(); //Double Jump is covered here, too
                //Only allow the player to Roll if they have it unlocked.
                

                HandleWaiting();

                //Effects
                FootSlideEffect();

                
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
        PlayerMovementInput = Quaternion.AngleAxis(camera.transform.rotation.y*cameraProjectedMovementOffset, Vector3.up) * PlayerMovementInput;
        
        if(Mathf.Abs(PlayerMovementInput.x) > 0.1f && Mathf.Abs(PlayerMovementInput.z) > 0.1f){ //Prevents diagonal movement from being drastically faster
            PlayerMovementInput = PlayerMovementInput.normalized;
        }
        //PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if(Time.timeScale > 0){ //Only run if the game is not paused
            //Movement Logic
            if(canMove){
                animator.SetBool("Spawn",false);
                MovePlayer();
                if(ABILITIES[3]){
                    //HandleRoll();
                    HandleRollV2();
                }
            }
        }
    }
    //Switches the player's parent to make their movement relative to the ground
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Ground"){
            gameObject.transform.SetParent(collision.gameObject.transform, true);
            //isGrounded = true;
        }
        if(collision.gameObject.tag == "GroundNoParent"){
            isGrounded = true;
        }
    }

    void OnCollisionStay(Collision collision){
        if(collision.gameObject.tag == "Ground"){
            gameObject.transform.SetParent(collision.gameObject.transform, true);
            //isGrounded = true;
        }
        if(collision.gameObject.tag == "GroundNoParent"){
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
        Vector3 scale = new Vector3(0.8f,0.8f,0.8f); //Resets the scale because it gets messed up due to floating point precision
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
            if(!animator.GetBool("Roll")){
                PlayerBody.velocity = Vector3.Lerp(PlayerBody.velocity, new Vector3(0.0f, PlayerBody.velocity.y, 0.0f), Time.deltaTime*manualDrag);
            }
        }else{
            if(PlayerMovementInput.x == -previousDirection.x || PlayerMovementInput.z == -previousDirection.z){
                if(!animator.GetBool("Roll")){
                    PlayerBody.velocity = Vector3.Lerp(new Vector3(previousDirection.x, PlayerBody.velocity.y, previousDirection.z), new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z), Time.deltaTime*currentSpeed);
                }
                
            }else{
                if(!animator.GetBool("Roll")){
                    PlayerBody.velocity = Vector3.Lerp(new Vector3(0.0f, PlayerBody.velocity.y, 0.0f), new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z), Time.deltaTime*currentSpeed);
                }
                
            }
            if(!animator.GetBool("Roll")){
                PlayerBody.AddForceAtPosition(new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z),MovementForcePoint);
            }else{
                PlayerBody.AddForce((new Vector3(MoveVector.x, 0.0f, MoveVector.z).normalized*currentSpeed*25));
            }
            
            
        }
        //PlayerBody.MovePosition(transform.position + MoveVector * Time.deltaTime);
        if(PlayerMovementInput != Vector3.zero){
            
            lastDirectionBeforeRoll = gameObject.transform.rotation;
            RotatePlayer();
        }
        
    }

    //Handles how the player rotates to face the direction in which the player is moving
    private void RotatePlayer(){
        if(rotateToMovementDirection){
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(-PlayerMovementInput.x, PlayerMovementInput.y, -PlayerMovementInput.z),Vector3.up);
            Quaternion adj = new Quaternion(0.0f, gameObject.transform.rotation.y, 0.0f, 1);
            Quaternion newRotation = Quaternion.RotateTowards(adj, toRotation, rotationSpeed);
            //newRotation *= Quaternion.Euler(-90, 0, 90);
            gameObject.transform.rotation = newRotation;
        }
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
            if(DetectGround()){
                //animator.SetBool("Running", true);
                animator.SetBool("Rising",false);
                animator.SetBool("Falling",false);
            }else{
                animator.SetBool("Running", false);
                animator.SetBool("Rising",true);
            }
        }else{
            animator.SetBool("Rising", false);
        }
        if(PlayerBody.velocity.y < -1.0f){ //FALLING
            if(DetectGround()){
                //animator.SetBool("Running", true);
                animator.SetBool("Falling", false);
                animator.SetBool("Rising",false);
            }else{
                animator.SetBool("Running", false);
                animator.SetBool("Falling", true);
            }
        }else{
            animator.SetBool("Falling", false);
        }
    }
    
    //Handles what happens when the player jumps
    private void HandleJump(){
        bool groundedJump = false;
        //RaycastHit hit;
        //Vector3 adjPosition = new Vector3(transform.position.x - 0.1f, transform.position.y + 0.1f, transform.position.z);
        //Physics.Raycast(adjPosition, -Vector3.up, out hit, Mathf.Infinity);
        DetectGround();
        if(Input.GetButtonDown("Jump")){ //Jump
            // if((hit.collider != null) && (hit.collider.tag == "Ground")){
            //     float distanceFromGround = Math.Abs(hit.point.y - transform.position.y);
            //     if(distanceFromGround < 0.1f){
            //         jumpCounter = 1;
            //         isGrounded = true;
            //         Jump(jumpForce);
            //         groundedJump = true;
            //         animator.SetBool("Double Jump", false);
            //     }
            // }
            if(isGrounded){
            //     float distanceFromGround = Math.Abs(hit.point.y - transform.position.y);
            //     if(distanceFromGround < 0.1f){
            //         jumpCounter = 1;
            //         isGrounded = true;
            //         Jump(jumpForce);
            //         groundedJump = true;
            //         animator.SetBool("Double Jump", false);
            //     }
            jumpCounter = 1;
            groundedJump = true;
            animator.SetBool("Double Jump", false);
            isGrounded = false;
            Jump(jumpForce);
            }
        }
        //Only allow the player to perform a double jump if they have the ability unlocked.
        if(ABILITIES[2]){
            if(Input.GetButtonDown("Jump") && !groundedJump){ //Jump
                if(jumpCounter > 0){ //Double Jump
                    Jump(jumpForce/2);
                    jumpCounter = jumpCounter - 1;
                    animator.SetBool("Double Jump", true);
                }
            }
        }  
    }

    private void Jump(float force){
        PlayerBody.AddForce(Vector3.up * force, ForceMode.Impulse);
        isGrounded = false;
    }

    //Handles what happens when the player spawns in
    public void HandlePlayerSpawn(){
        canMove = true;
    }

    //Handles the player using the spin move
    public void HandleSpin(){
        if(Input.GetButtonDown("Spin") && ((spinValue-1) > 0)){
            spinValue = spinValue - spinStrength;
            lastSpinFrame = playerFrameCounter;
            mainModel.SetActive(false);
            spinModel.SetActive(true);
            spinParticles.GetComponent<ParticleSystem>().Play();
            //Only allow the player to use the Spin Hover ability if they have it unlocked.
            if(ABILITIES[1]){
                SpinHover();
            }
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
        //animator.SetBool("Death",true);
        PlayerBody.useGravity = false;
        PlayerBody.velocity = Vector3.zero;
        collider.enabled = false;
        characterObject.SetActive(false);
        mainModel.SetActive(false);
        deadModel.SetActive(true);
        GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraBase>().ShakeCamera(5.0f,0.1f, gameObject.transform);
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

    public void SetCanMove(Boolean cm){
        this.canMove = cm;
    }

    public void HandleExitLevel(){
        Destroy(PlayerBody); //Destroy the physics handler
        Destroy(mainModel); //Destroy the visual model
        endLevelParticles.GetComponent<ParticleSystem>().Play();
        playerShadow.SetActive(false);
    }

    //Overload to allow the Player to be rotated towards a gameObject.
    public void HandleExitLevel(GameObject facePoint){
        Destroy(PlayerBody); //Destroy the physics handler
        //Destroy(mainModel); //Destroy the visual model
        //Reset all animator params to their defaults.
        foreach(AnimatorControllerParameter param in animator.parameters){
            switch(param.type){
                case(AnimatorControllerParameterType.Bool):
                    animator.SetBool(param.name, param.defaultBool);
                break;
                case(AnimatorControllerParameterType.Float):
                    animator.SetFloat(param.name, param.defaultFloat);
                break;
            }
        }
        animator.SetBool("Exit_0",true);
        canMove = false;
        armature.transform.LookAt(facePoint.transform, Vector3.up);
        spinModel.SetActive(false);
        endLevelParticles.GetComponent<ParticleSystem>().Play();
        playerShadow.SetActive(false);
        endLevelParticles.transform.position = facePoint.transform.position;
    }

    private Boolean DetectGround(){
        Boolean returnVal = false;
        RaycastHit hit;
        //Vector3 adjPosition = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        Physics.Raycast(groundSensor.transform.position, -Vector3.up, out hit, Mathf.Infinity);
        bool result = false;
        foreach(Transform sensor in groundSensor.transform){
            if(!result){
                result = Physics.Raycast(groundSensor.transform.position, -Vector3.up, out hit, Mathf.Infinity);
            }else{
                
                if((hit.collider != null) && ((hit.collider.tag == "Ground") || (hit.collider.tag == "GroundNoParent"))){
                    float distanceFromGround = Math.Abs(hit.point.y - sensor.transform.position.y);
                    if(distanceFromGround < 0.2f){
                        isGrounded = true;
                        returnVal = true;
                        return returnVal;
                    }
                }
            }
            

        }
        
        return returnVal;
        }

        public void BouncePlayer(float force){
            PlayerBody.velocity = Vector3.zero;
            PlayerBody.AddForce(Vector3.up * jumpForce*force, ForceMode.Impulse);
        }

        public void ApplyLevelSettings(){
            manager.GetComponent<GameManager>().SetUpLevelSettings(this);
        }


        //SPECIAL ABILITIES
        public void HandleRoll(){
            if (Input.GetButton("Roll"))
            {
                if(!animator.GetBool("Roll")){
                    lastDirectionBeforeRoll = gameObject.transform.rotation;
                    gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    RollCollider.enabled = true;
                    animator.SetBool("Roll", true);
                    PlayerBody.constraints = RigidbodyConstraints.None;
                    rotateToMovementDirection = false;
                    //PlayerBody.centerOfMass = PlayerBody.centerOfMass + new Vector3(0.0f, 0.3f, 0.0f);
                    MovementForcePoint = RollForcePoint.transform.position;
                    currentSpeed = speed * 20;
                    
                }
            }else{
                if(animator.GetBool("Roll")){
                    gameObject.transform.rotation = lastDirectionBeforeRoll;
                    rotateToMovementDirection = true;
                    rotationSpeed = 100.0f;
                    collider.enabled= true;
                    RollCollider.enabled = false;
                    animator.SetBool("Roll", false);
                    PlayerBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
                    //gameObject.transform.rotation = lastDirectionBeforeRoll;
                    //PlayerBody.centerOfMass = new Vector3(0,0,0);
                    //MovementForcePoint = gameObject.transform.position;
                    RotatePlayer();
                    //gameObject.transform.rotation = lastDirectionBeforeRoll;
                    currentSpeed = speed / 2;
                    rolling = true;
                }else{
                        //rotateToMovementDirection = true;
                        //if(rolling){
                             //Adjust the player's rotation when they come out of a roll
                            PlayerBody.rotation = defaultRBRotation;
                            gameObject.transform.rotation = lastDirectionBeforeRoll;
                            //RotatePlayer();
                            //Debug.Log("ROTATED!");
                            //rolling = false;
                        //}
                    }
                }
        }

        int rollState = 0;
        public void HandleRollV2(){
            switch(rollState){
                case(0):
                    if (Input.GetButton("Roll")){
                        //lastDirectionBeforeRoll = gameObject.transform.rotation;
                        gameObject.GetComponent<CapsuleCollider>().enabled = false;
                        RollCollider.enabled = true;
                        animator.SetBool("Roll", true);
                        PlayerBody.constraints = RigidbodyConstraints.None;
                        rotateToMovementDirection = false;
                        //PlayerBody.centerOfMass = PlayerBody.centerOfMass + new Vector3(0.0f, 0.3f, 0.0f);
                        MovementForcePoint = RollForcePoint.transform.position;
                        currentSpeed = speed * 20;

                        //gameObject.transform.rotation.Set(lastDirectionBeforeRoll.x, 0.0f, lastDirectionBeforeRoll.z,1);
                        rollState++; //Player is now rolling. Move to next state.
                        camera.GetComponent<CameraBase>().SetCameraTarget(rollCameraTarget.transform);
                        camera.GetComponent<CameraBase>().SetCameraLookTarget(rollCameraTarget.transform);
                    }
                break;
                case(1):
                    if(!Input.GetButton("Roll")){ //user is no longer pressing the roll button
                        rotationSpeed = 500.0f;
                        
                        RollCollider.enabled = false;
                        animator.SetBool("Roll", false);
                        gameObject.transform.rotation = lastDirectionBeforeRoll;
                        PlayerBody.rotation = gameObject.transform.rotation;
                        currentSpeed = speed / 2;
                        rolling = true;

                        rollState++; //Player is no longer rolling. Move to next state.
                    }
                break;
                    case(2):
                        //Do anything we need to do to reset the player when the exit the roll
                        rotateToMovementDirection = true;
                        PlayerBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

                        gameObject.transform.rotation = defaultRBRotation;
                        PlayerBody.rotation = defaultRBRotation;
                        gameObject.GetComponent<CapsuleCollider>().enabled = true;
                        camera.GetComponent<CameraBase>().SetCameraTarget(mainCameraTarget.transform);
                        camera.GetComponent<CameraBase>().SetCameraLookTarget(mainCameraTarget.transform);
                        rollState++;
                    break;
                default:
                    rollState = 0;
                break;
            }
        }

        private void SpinHover(){
            //Play special spin hovering effects here...
            PlayerBody.velocity = new Vector3(PlayerBody.velocity.x, 2.0f, PlayerBody.velocity.z); //Gives the player a 'floating' effect while spinning.

        }

        public void ApplyExternalForce(Vector3 force, float strength){
            PlayerBody.AddForce(force*strength, ForceMode.Impulse);
        }

        private void FootSlideEffect(){
            if(PlayerMovementInput == Vector3.zero){
                if((PlayerBody.velocity.x > 0.0f && PlayerBody.velocity.z > 0.0f) || (PlayerBody.velocity.x < 0.0f && PlayerBody.velocity.z < 0.0f)){
                    leftFootSlideEffect.Play();
                    rightFootSlideEffect.Play();
                }else{
                    leftFootSlideEffect.Stop();
                    rightFootSlideEffect.Stop();
                }
            }else{
                leftFootSlideEffect.Stop();
                rightFootSlideEffect.Stop();
            }
        }

        public void SetAnimatorBoolean(string paramName, bool value){
            animator.SetBool(paramName, value);
        }
        
}
