using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class EnemyBase : MonoBehaviour
{
    /*
    Written By: Joshua Rist
    Created: 11/30/2023

    This class serves as the base parent class for (hopefully) all enemies in the game.
    Some enemies can be stomped on to be killed, some can be spun, some can be both and some can be neither.
    Enemies that cannot be directly spun or stomped, likely have some kind of extra 'sweet spot' trigger.

    Every enemy should have some kind of destructionWeight assigned to them, and that value can be zero.
    */
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] protected int destructionWeight;
    protected bool alive;
    [SerializeField] protected float bounceForce;
    [SerializeField] protected bool stompable; //Can this enemy be stomped?
    [SerializeField] protected bool spinnable; //Can this enemy be spun?

    //Kill this enemy. 
    //type = "Stomp" -> Enemy was stomped on
    //type = "Spin" -> Enemy was spun
    public abstract void Kill(string type);

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