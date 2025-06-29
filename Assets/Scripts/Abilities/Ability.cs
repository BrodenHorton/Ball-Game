public abstract class Ability
{
    public bool isPassive;
    public bool isActivated;
    public bool needsPhysicsUpdate;
    public float activatedLength;
    protected Timer activationTimer;

    public abstract void Activate();
    public abstract void Upgrade();
    public abstract void Update();
}
