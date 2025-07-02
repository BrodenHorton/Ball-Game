public interface IDamageable
{
    public bool IsDead { get; }
    public void TakeDamage(float amt, EffectType effectType = EffectType.NORMAL);
    public void Heal(float amt);
}
