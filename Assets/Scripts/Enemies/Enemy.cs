using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void kill();
    public abstract void Reset();
    public abstract void Disable();
}
