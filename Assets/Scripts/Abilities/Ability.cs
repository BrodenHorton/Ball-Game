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
    public abstract void DashedIntoEventHandler(GameObject enemy);
    protected virtual void ConnectToEvents()
    {
        EventBus.DashedInto += DashedIntoEventHandler;
    }
    protected virtual void DisconnectEvents()
    {
        EventBus.DashedInto -= DashedIntoEventHandler;
    }

}
