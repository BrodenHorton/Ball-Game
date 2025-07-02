public class DamageEffect : StatusEffect
{
    protected float damage;
    public DamageEffect(float damage, float duration, float damageTick, IDamageable target, EffectType effectType) : base(duration, damageTick, target, effectType){
        this.damage = damage;
    }

    protected override void OnUpdate(float deltaTime)
    {
       if (damageTickTimer.IsFinished())
        {
            damageTickTimer.Reset();
            target.TakeDamage(damage, EffectType.FIRE);
        }
    }
}
