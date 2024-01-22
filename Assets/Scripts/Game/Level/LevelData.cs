using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelData : MonoBehaviour
{
    //This script handles the data within the game for a given level.
    [SerializeField] private String levelName; //The name of this level
    [SerializeField] private int levelNumber; //The level number of the level
    [SerializeField] private String levelType; //VALID TYPES: Trail, Tracking, Chase, Boss, Hub
    [SerializeField] private Boolean fog; //Does this level have fog or not

    private List<GameObject> items; //The items that are collected, Gems, the Contract, etc
    [SerializeField] private Checkpoint[] checkpoints; //The checkpoints in the level
    //[SerializeField] private int EntryIndex; //The index at which we want the player to spawn at (checkpoint)
    private Checkpoint activeCheckpoint; //Whichever checkpoint the player last touched. If null, the default spawnpoint for the level should be used.
    private Boolean contractCollected;
    [SerializeField] private float cameraProjectedMovementOffset; //How much should the player's input vector be rotated along the Camera's Y axis? 100 = complete match, 0 = no match.

    private int destructableTotal; //The total of every object that contributes to this level's Destruction %

    private GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InitalizeLevelState();
        items = new List<GameObject>();
        SumLevelDestructableWeights();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Acceptable types:
    //trail
    //tracking
    //chase
    //boss
    //hub
    public String GetLevelType(){
        return levelType;
    }

    public float GetCameraProjectionMultiple(){
        return this.cameraProjectedMovementOffset;
    }

    public Boolean GetFogStatus(){
        return this.fog;
    }

    public void SetLevelType(String type){
        this.levelType = type;
    }

    public void SetFogStatus(Boolean status){
        this.fog = status;
    }

    private void InitalizeLevelState(){
        PersistentGameData gameData = GameObject.Find("PersistentGameData").GetComponent<PersistentGameData>();
        // LevelData state = gameData.GetLevelState(levelNumber);
        // this.levelType = state.GetLevelType();
        // this.fog = state.GetFogStatus();

    }

    public void SetContractCollected(Boolean collected){
        this.contractCollected = collected;
        //print("COLLECTED!!");
    }

    public void SetActiveCheckPoint(Checkpoint check){
        this.activeCheckpoint = check;
    }
    
    //This function is essential to what happens when the player loads into this level. It defines the 'method' the player enters the level, wheather that be from the Hub or from a Bonus Round
    //If the Player comes from the Bounus Round, special things need to happen such as spawning at a spesific checkpoint rather than the start of the level.
    //CheckpointIndex -> The index of which checkpoint the player should spawn at. If 0, the player should spawn at the start of the level.
    public void EnterLevel(int checkpointIndex){
        if(checkpointIndex > 0){
            activeCheckpoint = checkpoints[checkpointIndex];
            manager.GetComponent<GameManager>().HandleCheckpoint(activeCheckpoint.GetPlayerCheckPointSpawn(), activeCheckpoint.GetCameraCheckPointSpawn());
        }
    }

    //Sum together all Destruction Weights held by Destructables & Enemies in the level
    private void SumLevelDestructableWeights(){
        //Sum together the Destructables
        Destructable[] destructables = FindObjectsOfType<Destructable>();
        //Enemy[] enemies = FindObjectsOfType<Enemy>();
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        foreach(Destructable des in destructables){
            destructableTotal += des.GetDestructionWeight();
        }
        //foreach(Enemy eni in enemies){
        foreach(EnemyBase eni in enemies){
            destructableTotal += eni.GetDestructionWeight();
        }
    }

    //Get the amount of objects in the level which contain a Destruction Weight (Enemy or Destructable)
    public int GetDestructionTotal(){
        return destructableTotal;
    }

}
