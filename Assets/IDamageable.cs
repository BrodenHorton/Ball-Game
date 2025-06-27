public interface IDamageable
{
    public bool IsDead { get; }
    public void TakeDamage(float amt);
    public void Heal(float amt);
}
