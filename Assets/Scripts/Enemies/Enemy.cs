using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] protected int destructionWeight;
    protected bool alive;
    [SerializeField] float bounceForce;

    //Kill this enemy. 
    //type = "Stomp" -> Enemy was stomped on
    //type = "Spin" -> Enemy was spun
    public abstract void kill(string type);

    public int GetDestructionWeight(){
        return destructionWeight;
    }

    public bool GetAlive(){
        return alive;
    }

    //How strong is this enemy's bounce force when the player stomps on them?
    public float GetBounceForce(){
        return bounceForce;
    }
}
