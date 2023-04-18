using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scrap : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private float offset;
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

    private Boolean spawned; //If this scrap was spawned by another object or was present at the beginning

    private bool objectDestroyed; //This is used so the gameObject isn't destroyed before the audio clip can finish
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Target")[0];
        material = model.GetComponent<Renderer>().material;
        startingColor = material.color;
        manager = GameObject.Find("GameManager");
    }

    void Awake(){
        player = GameObject.FindGameObjectsWithTag("Target")[0];
        material = model.GetComponent<Renderer>().material;
        startingColor = material.color;
        manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectsWithTag("Target")[0];
        //print(player);
        if(!objectDestroyed){
            player = GameObject.FindGameObjectsWithTag("Target")[0];
            Vector3 adjustedPlayerPosition = new Vector3(player.transform.position.x, player.transform.position.y + offset, player.transform.position.z);
            if(collected && !manager.GetComponent<GameManager>().GetPaused()){
                model.GetComponent<Renderer>().material = collectedMaterial;
                trail.SetActive(true);
                gameObject.transform.position = Vector3.MoveTowards(transform.position, adjustedPlayerPosition, speed);
                if(Vector3.Distance(gameObject.transform.position, adjustedPlayerPosition) < 0.1f){
                    //Destroy(shadow); //destroys the shadow
                    manager.GetComponent<GameManager>().IncrementScore(1); //Increments the score by 1
                    Destroy(trail);
                    Destroy(model);
                    Destroy(shadow);
                    Destroy(gameObject, sound.clip.length);
                    objectDestroyed = true;
                
                }   
            }
        }
        
    }

    void OnTriggerEnter(){
        sound.pitch = UnityEngine.Random.Range(0.75f, 1.0f);
        sound.Play();

        collected = true;
        Destroy(GetComponent<BoxCollider>()); //removes the collider from the object
        trail.SetActive(true);
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
