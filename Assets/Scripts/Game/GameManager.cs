using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int score;
    public static int lives = 10;
    [SerializeField] private GameObject playerSpawn;
    [SerializeField] private GameObject cameraSpawn;
    private static GameObject[] collected;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;
    private const int MAX_SCRAP = 99; //The max amount of scrap that can be collected before earning a life
    private const int MAX_LIVES = 99; //The maximum amount of lives a player can have
    private GameObject[] deathBarriers;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private bool paused;
    private int playerDamageState;
    

    void Awake(){
        instance = this;
        DontDestroyOnLoad(this.gameObject); //Keeps this object from ever being deleted between Scene changes
    }
    // Start is called before the first frame update
    void Start()
    {
        deathBarriers = GameObject.FindGameObjectsWithTag("Death_Barrier");
    }

    // Update is called once per frame
    void Update()
    {
       HandlePause();
    }

    public void IncrementScore(int amount){
        int newScore = score + amount;
        if(newScore > 99){
            score = 0;
            IncrementLives(1);
            score += amount-1;
        }else{
            score += amount;
            HandlePlayerDamageState();
        }
        
    }
    public void DecrementScore(int amount){
        if(score - amount < 0){
            score = 0;
        }else{
            score = score - amount;
        }
    }

    public void IncrementLives(int amount){
        if(lives + amount >= MAX_LIVES){
            lives = MAX_LIVES;
        }else{
            lives += amount;
        }
    }

    public void DecrementLives(int amount){
        
        if(lives - amount < 0){
            lives = 0; //for safety reasons, this shouldn't ever matter
            HandleGameOver(); //If lives is below 0, a game over occurs
        }else{
            lives -= amount;
        }
    }

    //Handles what happens when the player reaches a checkpoint
    void HandleCheckpoint(GameObject PlayerCheckpoint, GameObject CameraCheckpoint){
        playerSpawn = PlayerCheckpoint;
        cameraSpawn = CameraCheckpoint;
    }

    //Get the current score / scrap total
    public int GetScore(){
        return score;
    }

    //Get the current lives total
    public int GetLives(){
        return lives;
    }

    //Handles what happens when the player dies
    public void HandlePlayerDeath(){
        DecrementLives(1); //lives--;
        score = 0; //Reset the score to 0
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        players[0].GetComponent<PlayerMovement>().canMove = false;
        players[0].GetComponent<PlayerMovement>().HandleDeath();
        StartCoroutine(HandleDeathFade("in"));
    }

    // public void HandlePlayerDamage(){
    //     GameObject player = GameObject.Find("Player");
    //     int dealtDamage = player.GetComponent<PlayerMovement>().DealDamage();
    //     if(dealtDamage == 0){
    //         playerDamageState = 0;
    //         HandlePlayerDeath(); //Player couldn't move down a damage state, kill the player
    //     }else{
    //         playerDamageState = player.GetComponent<PlayerMovement>().GetDamageState();
    //     }
    // }

    public void HandlePlayerRespawn(){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] cams = GameObject.FindGameObjectsWithTag("MainCamera");
        DestroyAll(players);
        DestroyAll(cams);
        
        Vector3 playerRespawnPos = playerSpawn.transform.position;
        Vector3 camRespawnPos = cameraSpawn.transform.position;
        Transform camRotation = cameraSpawn.transform;
        //WAIT
        Instantiate(playerPrefab, playerRespawnPos, Quaternion.identity);
        Instantiate(cameraPrefab, camRotation, true);
        players[0].GetComponent<PlayerMovement>().HandleRespawn();
        cams = GameObject.FindGameObjectsWithTag("MainCamera");
        cams[0].GetComponent<CameraMove>().centerCamera(); //recenters the main camera
        Reinstate();
        StartCoroutine(HandleDeathFade("out"));
    }

    //Handles what happens when a gameover occurs
    public void HandleGameOver(){
        
    }

    //Reinstate (sets back to Active or Inactive) game objects during runtime
    void Reinstate(){
        // for(int i = 0; i < deathBarriers.Length; i++){
        //     deathBarriers[i].GetComponent<Death_Barrier>().reinstate();
        // }
    }


    //Destroys all gameobjects in a list of gameobjects
    void DestroyAll(GameObject[] list){
        foreach(GameObject obj in list){
            Destroy(obj);
        }
    }

    //Set the player's spawn as well as a camera position for when the player respawns. 
    public void SetPlayerSpawn(GameObject spawn, GameObject cameraPosition){
        playerSpawn = spawn;
        cameraSpawn = cameraPosition;
    }

    void HandlePause(){
        if(Input.GetKeyDown("escape") && !paused){
            Time.timeScale = 0; //Pauses the game
            paused = true;
            pauseScreen.SetActive(true); //Displays "Paused Game" graphic
            print("PAUSED");
        }else if(Input.GetKeyDown("escape") && paused){
            Time.timeScale = 1; //Unpauses the game
            paused = false;
            pauseScreen.SetActive(false); //Removes "Paused Game" graphic
            print("UNPAUSED");
        }
    }

    public bool GetPaused(){ //Get the value of paused
        return paused;
    }

    private void HandlePlayerDamageState(){
        // if(playerDamageState == 0){
        //     if(score > 10){
        //         playerDamageState = 1;
        //         GameObject player = GameObject.Find("Player");
        //         player.GetComponent<PlayerMovement>().SetDamageState(2);
        //         print("setState = " + playerDamageState + 1);
        //     }
        // }else if(playerDamageState == 1){
        //     if(score == 99){
        //         GameObject player = GameObject.Find("Player");
        //         player.GetComponent<PlayerMovement>().SetDamageState(2);
        //         print("setState = " + playerDamageState + 1);
        //     }
        // }
        
    }

    //in -> scale the image up to fill the screen
    //out -> scale the image down so you can see the screen
    IEnumerator HandleDeathFade(string type){
        GameObject fade = GameObject.Find("Death_Fade");
        Image graphic = fade.GetComponent<Image>();
        // fade from opaque to transparent
        if(type == "out"){
            // loop over 1 second backwards
            for (float i = 2; i >= 0; i -= Time.deltaTime*2)
            {
                // set color with i as alpha
                graphic.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else if(type == "in")
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                graphic.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }

    
}
