public abstract class StatusEffect
{
    protected Timer effectTimer;
    protected Timer damageTickTimer;
    protected IDamageable target;

    public bool IsFinished => effectTimer.IsFinished();

    public StatusEffect(float duration, float damageTick, IDamageable target)
    {
        effectTimer = new Timer(duration);
        damageTickTimer = new Timer(damageTick);
        this.target = target;
    }

    public void Tick(float deltaTime)
    {
        if(target == null)
        {
            effectTimer.SetFinished();
            damageTickTimer.SetFinished();
            return;
        }
        effectTimer.Update();
        damageTickTimer.Update();
        OnUpdate(deltaTime);
    }

    protected abstract void OnUpdate(float deltaTime);
}
public enum EffectType
{
    FIRE,
    ICE,
    ELECTRIC,
    WATER
}
