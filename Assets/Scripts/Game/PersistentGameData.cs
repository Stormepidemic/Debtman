using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameData : MonoBehaviour
{
    public static PersistentGameData instance;

    private List<LevelData> levels;
    private List<LevelData> initalData; //Default game state with 0% completion. Used when creating a new game.
    private TextAsset defaultState;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        defaultState = Resources.Load<TextAsset>("Game/LevelDetails");
        levels = new List<LevelData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Load(){ //Load save data from file

    }

    void Save(int saveNumber){ //Save data to a save file

    }

    public void LoadDefaultState(){
        if(defaultState == null){
            this.defaultState = Resources.Load<TextAsset>("Game/LevelDetails");
        }
    }

    public LevelData GetLevelState(int levelNumber){
        //return levels[levelNumber];
        return null;
    }
}
