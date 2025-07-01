public class FireDamageEffect : StatusEffect
{
    public float damage = 5;
    public FireDamageEffect(float duration, float damageTick, IDamageable target) : base(duration, damageTick, target){ }

    protected override void OnUpdate(float deltaTime)
    {
       if (damageTickTimer.IsFinished())
        {
            damageTickTimer.Reset();
            target.TakeDamage(damage);
        }
    }
}
