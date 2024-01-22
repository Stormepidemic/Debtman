using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* This class defines a 'Clearable' object/element. 
* A clearable element is one in which is deleted/Destroyed upon a certain event occuring.
* The event in which this element is deleted is determined and called from various sources.
* The clearable elements have an event defined by an enum, and the clearables that have that set enum are collected
* and then destroyed when the desired event occurs. 
*/


public enum ClearableCondition
{
    PLAYER_DEATH,
    LEVEL_START,
    LEVEL_END,
    ALL
}

public abstract class ClearableElement : MonoBehaviour
{
    protected ClearableCondition clearType {get;set;} //The type of event in which this element will be cleared.
    public abstract void ClearElement(); //Handle what happens when this element is called to be 'cleared'
    public abstract void ClearElement(ClearableCondition condition); //Clear this element if the provided type matches this element's clear condition type

}
