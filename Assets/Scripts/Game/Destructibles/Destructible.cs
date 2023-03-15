using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destructible : MonoBehaviour
{
    public abstract void Break();
    public abstract void spawnCollectibles();
    public abstract void Reset();
}
