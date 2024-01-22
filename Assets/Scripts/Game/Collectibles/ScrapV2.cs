using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScrapV2 : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private float offset;
    [SerializeField] private int value; //The amount of 'scrap' this Scrap is worth
    private Boolean collected = false;
    [SerializeField] private GameObject manager;
    private Material material;
    [SerializeField] Material collectedMaterial;
    private Color startingColor;
    private Color endColor = new Color(255, 12, 28, 1.0f);
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject trail;
    [SerializeField] private AudioSource sound;
    [SerializeField] private GameObject shadow;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Animator anim;
    private int collectionSpeedIncrease;

    private Boolean spawned; //If this scrap was spawned by another object or was present at the beginning

    private bool objectDestroyed; //This is used so the gameObject isn't destroyed before the audio clip can finish
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Target")[0];
        material = model.GetComponent<Renderer>().material;
        startingColor = material.color;
        manager = GameObject.Find("GameManager");
        anim.Play("Rotate",-1,UnityEngine.Random.Range(0.0f,anim.GetCurrentAnimatorStateInfo(0).length));
    }

    void Awake(){
        player = GameObject.FindGameObjectsWithTag("Target")[0];
        material = model.GetComponent<Renderer>().material;
        startingColor = material.color;
        manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        //print(player);
        if(!objectDestroyed && collected){
            player = GameObject.FindGameObjectsWithTag("Target")[0];
            Vector3 adjustedPlayerPosition = new Vector3(player.transform.position.x, player.transform.position.y + offset, player.transform.position.z);
            if(collected && !manager.GetComponent<GameManager>().GetPaused()){
                model.GetComponent<Renderer>().material = collectedMaterial;
                trail.SetActive(true);
                //gameObject.transform.position = Vector3.MoveTowards(transform.position, adjustedPlayerPosition, speed);
                body.AddForce(-(transform.position-adjustedPlayerPosition)*(speed+collectionSpeedIncrease));
                collectionSpeedIncrease = collectionSpeedIncrease + 2; //Increase the speed over time
                if(Vector3.Distance(gameObject.transform.position, adjustedPlayerPosition) < 0.25f){
                    //Destroy(shadow); //destroys the shadow
                    manager.GetComponent<GameManager>().IncrementScore(value); //Increments the score by the value of this scrap
                    Destroy(trail);
                    Destroy(model);
                    Destroy(shadow);
                    Destroy(gameObject, sound.clip.length);
                    objectDestroyed = true;
                
                }   
            }
        }
        
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag.Contains("Player")){
            if(player == null){
                player = GameObject.FindGameObjectsWithTag("Target")[0];
            }

            sound.pitch = UnityEngine.Random.Range(0.75f, 1.0f);
            sound.Play();

            collected = true;
            trail.SetActive(true);
            Destroy(GetComponent<BoxCollider>()); //removes the collider from the object
        }
    }

    public void SetSpawned(Boolean spawned){
        this.spawned = spawned;
    }

    public void SetCollected(Boolean collected){
        this.collected = collected;
    }

    public Boolean GetSpawned(){
        return this.spawned;
    }

    public Boolean GetCollected(){
        return this.collected;
    }
}
