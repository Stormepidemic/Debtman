using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destructable : MonoBehaviour
{
    public abstract void Break();
    public abstract void spawnCollectibles();
    public abstract void Ignite(); //May or may not have any implementation!
}
