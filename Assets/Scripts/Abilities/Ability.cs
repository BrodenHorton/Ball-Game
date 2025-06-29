using System;
using UnityEngine;


public abstract class Ability : ScriptableObject
{
    public bool isPassive;
    [NonSerialized] public bool isActivated;
    public bool needsPhysicsUpdate;
    public float activatedLength;
    [NonSerialized] protected Timer activationTimer;

    public abstract void Activate();
    public abstract void Upgrade();
    public abstract void Update();
    public abstract void Deactivate();
}
