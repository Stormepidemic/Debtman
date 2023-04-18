using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int score;
    public static int lives = 10;

    private GameObject mainUI; //The main UI in the game in a normal level

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
    private List<GameObject>[] resetableObjects; //0 -> CollectiblesList, 1 -> DestructiblesList, 2 -> InteractablesList, 3 -> EnemiesList, 4 -> OtherList

    private List<ResettableElement> resettableElements;
    
    //LEVEL DATA
    private LevelData currentLevelData; //Data for the current level. Checkpoint coordinates, amount of collectibles, level information, etc
    private PersistentGameData gameData; //Data for the game itself. Which contracts have been collected, etc.

    private Boolean playerAlive = true; //Is the player currently alive?
    private const float DEATH_DELAY = 2.0f; //How long to wait before respawning the player
    private float deathTimer = 0.0f; //The timer counting until the player respawns 

    void Awake(){
        instance = this;
        //I'll have to figure this one out later. Basically, reloading a current scene causes issues and makes multiple GameManagers. This is a problem.
        //I think i could fix it by making a LevelManager handle most of this shit, and then GameManager be the singleton that runs the logic between levels and stuff...
        //DontDestroyOnLoad(this.gameObject); //Keeps this object from ever being deleted between Scene changes
    }
    // Start is called before the first frame update
    void Start()
    {
        if(gameData == null){
            gameData = GameObject.Find("PersistentGameData").GetComponent<PersistentGameData>();
            //gameData.LoadDefaultState(); //Load the inital state of the game before any saves get loaded.
            print("HELLO");
        }

        //TESTING!!
        resettableElements = new List<ResettableElement>();


        resetableObjects = new List<GameObject>[6];
        //Set up our list of objects
        for(int i = 0; i < resetableObjects.Length; i++){
            resetableObjects[i] = new List<GameObject>();
        }

        deathBarriers = GameObject.FindGameObjectsWithTag("Death_Barrier");
    }

    // Update is called once per frame
    void Update()
    {
       HandlePause();
       if(currentLevelData == null){
        currentLevelData = GameObject.Find("LevelData").GetComponent<LevelData>();
       }
        if(resettableElements == null){
            resettableElements = new List<ResettableElement>();
        }
       //TESTING!!
       if(resettableElements.Count == 0){
            GameObject[] resettables = GameObject.FindGameObjectsWithTag("Resettable");
            foreach(GameObject obj in resettables){
                resettableElements.Add(obj.GetComponent<ResettableElement>());
            }
       }

       if(mainUI == null){
            mainUI = GameObject.Find("Main_UI");
       }

       if(!playerAlive){ //Respawn after DEATH_DELAY
            deathTimer += Time.deltaTime;
            if(deathTimer > DEATH_DELAY){
                deathTimer = 0;
                playerAlive = true;
                StartCoroutine(HandleDeathFade("in"));
                Invoke("HandlePlayerRespawn", DEATH_DELAY); //Respawn the player
            }
       }

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

        mainUI.GetComponent<MainUI>().ShowCounters(); //Show the amount of scrap & lives in the UI

        
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

        mainUI.GetComponent<MainUI>().ShowCounters(); //Show the amount of scrap & lives in the UI
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
    public void HandleCheckpoint(GameObject PlayerCheckpoint, GameObject CameraCheckpoint){
        playerSpawn = PlayerCheckpoint;
        cameraSpawn = CameraCheckpoint;
        UpdateResetableObjects();
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
        playerAlive = false;
        

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
        //DestroyAll(cams);
        
        Vector3 playerRespawnPos = playerSpawn.transform.position;
        Vector3 camRespawnPos = cameraSpawn.transform.position;
        Transform camRotation = cameraSpawn.transform;
        //WAIT
        //Instantiate(playerPrefab, playerRespawnPos, Quaternion.identity);
        //Instantiate(cameraPrefab, camRotation, true);
        players[0].GetComponent<PlayerMovement>().HandleRespawn();
        //cams = GameObject.FindGameObjectsWithTag("MainCamera");
        //cams[0].GetComponent<CameraMove>().centerCamera(); //recenters the main camera
        switch(currentLevelData.GetLevelType()){
            case("trail"):
                GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = Instantiate(playerPrefab, playerRespawnPos, Quaternion.identity).transform;
                GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().m_LookAt = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow;
                cams[0].transform.LookAt(GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow);
            break;
            case("tracking"):
                Instantiate(playerPrefab, playerRespawnPos, Quaternion.identity);
            break;
            case("chase"):
            break;
            case("boss"):
            break;
            case("hub"):
            break;
        }
        
        Reinstate();
        ResetResetableObjects(); //Turn back on/reset all objects not saved upon a checkpoint
        StartCoroutine(HandleDeathFade("out"));

        //Show the sliding elements to the player upon respawn
        mainUI.GetComponent<MainUI>().ShowCounters();
        mainUI.GetComponent<MainUI>().ShowInventory();
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
    private void SetPlayerSpawn(GameObject spawn, GameObject cameraPosition){
        playerSpawn = spawn;
        cameraSpawn = cameraPosition;
    }

    void HandlePause(){
        if(pauseScreen == null){
            pauseScreen = GameObject.Find("Pause");
        }
        if(Input.GetButtonDown("Pause") && !paused){
            Time.timeScale = 0; //Pauses the game
            paused = true;
            pauseScreen.SetActive(true); //Displays "Paused Game" graphic
            
        }else if(Input.GetButtonDown("Pause") && paused){
            Time.timeScale = 1; //Unpauses the game
            paused = false;
            pauseScreen.SetActive(false); //Removes "Paused Game" graphic
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
            for (float i = 0; i <= 2; i += Time.deltaTime)
            {
                // set color with i as alpha
                graphic.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }

    

    /*
    This function more or less handles populating the resetableObjects 2d array. These objects
    are then either marked as non-resetable when the player reaches a checkpoint, or are reset upon player death.
    Basically, things that the player interacts with BEFORE a checkpoint should stay broken when a checkpoint happens,
    but reset if they occur AFTER a checkpoint has been reached.
    */
    public void PopulateDisabledObjects(String type, GameObject obj){
        //0 -> Collectibles[]
        //1 -> Destructibles[]
        //2 -> Interactables[]
        //3 -> Enemies[]
        //4 -> Explosive[]
        //5 -> Other[]
        switch(type){
            case "collectible":
                resetableObjects[0].Add(obj);
                
            break;

            case "destructible":
                resetableObjects[1].Add(obj);
                obj.transform.parent.gameObject.SetActive(false);
            break;

            case "interactible":
                resetableObjects[2].Add(obj);
            break;

            case "enemy":
                resetableObjects[3].Add(obj);
                obj.transform.parent.gameObject.SetActive(false);
            break;

            case "explosive":
                resetableObjects[4].Add(obj);
                obj.SetActive(false);
            break;

            case "other":
            break;

            default:
            //Ill just leave this doing nothing for now
            break;
        }
    }

    //Handles re-enabling all of the objects that were messed with before the player died
    private void ResetResetableObjects(){

        foreach(GameObject item in resetableObjects[0]){ //COLLECTIBLE LIST
            //Reset collectibles
            
        }
        //Reset spawned collectibles
        GameObject[] scrap = GameObject.FindGameObjectsWithTag("Collectable");
        foreach(GameObject spawnedItem in scrap){
            if(spawnedItem.GetComponent<Scrap>() != null){ //If it is scrap...
                if(spawnedItem.GetComponent<Scrap>().GetSpawned()){
                    Destroy(spawnedItem); //If the scrap was spawned by another object
                }else if(spawnedItem.GetComponent<Scrap>().GetCollected()){
                    Destroy(spawnedItem); //If the scrap was touched, but may not have been collected
                }
            }
        }

        
        foreach(GameObject item in resetableObjects[2]){ //INTERACTiBLE
            //Reset interactibles
            item.GetComponent<Interactible>().Deactivate();
        }

        foreach(ResettableElement element in resettableElements){
            element.ResetElement();
        }
    }

    //When the player reaches a checkpoint, all resetable objects that have been activated since the last checkpoint should still be activated.
    private void UpdateResetableObjects(){
        List<ResettableElement> destroyedElements = new List<ResettableElement>();
        //For now I'm just going to empty the resetableObjects structure so they never get reset in the level.
        for(int i = 0; i < resetableObjects.Length; i++){
            resetableObjects[i] = new List<GameObject>();
        }

        foreach(ResettableElement element in resettableElements){
            if(!element.GetElementStatus()){
                //resettableElements.Remove(element);
                destroyedElements.Add(element);
                //element.DestroyElement();
            }
            
            print(resettableElements.Count);
        }
        foreach(ResettableElement destroyed in destroyedElements){
            resettableElements.Remove(destroyed);
            destroyed.DestroyElement();
            print(resettableElements.Count);
        }
        //resettableElements = new List<ResettableElement>(); //Pretty much just delete the reset points

    }

    public void CollectInventoryCollectable(String type){ //Handle special collectables that go into the inventory rather than lives/scrap counter
        switch(type){
            case("contract"):
                mainUI.GetComponent<MainUI>().ShowContract();
                mainUI.GetComponent<MainUI>().ShowInventory();
                currentLevelData.SetContractCollected(true);
            break;
            default:
            break;
        }
    }

    

    
}
