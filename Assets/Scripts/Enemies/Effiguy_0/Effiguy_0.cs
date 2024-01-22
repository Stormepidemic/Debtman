using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effiguy_0 : ActivatableEnemy
{
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material activedMaterial;
    [SerializeField] private Animator animator;
    [SerializeField] private Renderer renderer;
    [SerializeField] private GameObject[] effects;
    [SerializeField] private float speed;
    [SerializeField] private float lookSpeed;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] GameObject bodyPartsPrefab; //The dead body parts of this enemy that get thrown around when this enemy dies.
    [SerializeField] GameObject destroyLight;
    [SerializeField] GameObject deadModel; //The model used for this enemy when it is dead
    [SerializeField] GameObject enemyModel; //The hitboxes of this enemy; Both the KillEnemy and KillPlayer 
    private bool activated;
    private GameObject player;
    private bool isMove; //Is this enemy allowed to move?
    [SerializeField] private AudioSource audio;
    [SerializeField] private LookAt headRotator;
    // Start is called before the first frame update
    void Start()
    {
        alive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleActivatedState();
    }

    void OnTriggerEnter(Collider other){ //Player walks into the detection sphere
        if(other.gameObject.tag == "Player"){
            player = other.gameObject;
            Activate();
        }
    }

    public override void Activate(){
        if(animator != null){
                animator.SetBool("Activate", true);
        }
        renderer.material = activedMaterial;
        ToggleEffects(true);
        activated = true;
        isMove = true;
    }

    public override void Kill(string type){
        //deadModel.SetActive(true);
        isMove = false;
        alive = false;
        
        //Play particle effects
        //GameObject player = GameObject.Find("Player_Character");
        //Add some movement to the enemy's body parts
        // foreach(Rigidbody part in bodyParts){
        //     //part.AddExplosionForce(10.0f, part.gameObject.transform.position, 1.0f, 1.0f, ForceMode.Impulse);
        //     part.AddForceAtPosition((part.gameObject.transform.position - player.transform.position)*200, player.transform.position);
            
        // }
        Instantiate(bodyPartsPrefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent.parent);
        ToggleEffects(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().IncrementDestruction(destructionWeight);
        gameObject.SetActive(false);
        Destroy(gameObject.transform.parent.gameObject);
        //alive = false;
        //Destroy(destroyLight, 0.5f);

    }

    void HandleActivatedState(){
        if(activated && isMove){
            Vector3 toPlayerDirection = player.transform.position - gameObject.transform.position;
            rigidBody.AddForce(toPlayerDirection*speed, ForceMode.Impulse);
            setRotation();
            if(headRotator != null){
                Destroy(headRotator);
                headRotator.enabled = false;
            }
        }
    }

    //Rotate to face the
    void setRotation(){
        Vector3 targetPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 direction = targetPos - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation((direction).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
    }

    public void StepForward(){ //Called via an Animation Event to give the effect of the enemy stepping forward when the animation for walking is playing
        if(isMove){
            audio.Play();
            GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraBase>().ShakeCamera(5.0f,0.2f, gameObject.transform);
        }
    }

    //Turn on or turn off effects for this enemy. Used when the enemy gets activated or when it dies.
    //True -> Turn Effects on
    //False -> Turn Effects off
    private void ToggleEffects(bool toggle){
        foreach(GameObject effect in effects){
            effect.SetActive(toggle);
        }
    }

    public override void Disable(){
        //not used
    }

    
}
