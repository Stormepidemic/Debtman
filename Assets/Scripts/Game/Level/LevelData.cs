using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelData : MonoBehaviour
{
    //This script handles the data within the game for a given level.
    [SerializeField] private String levelName; //The name of this level
    [SerializeField] private int levelNumber; //The level number of the level
    [SerializeField] private String levelType; //VALID TYPES: Trail, Tracking
    [SerializeField] private Boolean fog; //Does this level have fog or not

    private List<GameObject> items; 

    private Boolean contractCollected;
    // Start is called before the first frame update
    void Start()
    {
        InitalizeLevelState();
        items = new List<GameObject>();
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
        print("COLLECTED!!");
    }


}
