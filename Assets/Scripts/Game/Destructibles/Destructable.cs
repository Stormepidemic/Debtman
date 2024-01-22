using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destructable : MonoBehaviour
{
    [SerializeField] protected int scrapValue; //How many scrap collectables are spawned by this destructable
    [SerializeField] protected int destructionWeight; //The 'size' of this destructable- how much of the level's Destruction % is this object worth?
    public abstract void Break();
    public abstract void spawnCollectibles(bool autoCollect);
    public abstract void Ignite(); //May or may not have any implementation!

    public int GetScrapValue(){
        return scrapValue;
    }

    public int GetDestructionWeight(){
        return destructionWeight;
    }
}
