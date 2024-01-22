using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract Boolean GetActive();
}
