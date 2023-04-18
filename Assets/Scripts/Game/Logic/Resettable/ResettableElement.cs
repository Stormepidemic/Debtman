using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ResettableElement : MonoBehaviour
{
    [SerializeField] protected GameObject prefab; //This is the prefab that gets respawned when ResetElement() is called

    public abstract void ResetElement(); //Reset/respawn the given prefab object

    public abstract void DestroyElement(); //Delete the respawn/reset point so this object will not respawn when called. This should occur whenever the player reaches a checkpoint

    public abstract Boolean GetElementStatus(); //Get if this element has been destroyed or not, if it currently exists regardless of reset or not
}
