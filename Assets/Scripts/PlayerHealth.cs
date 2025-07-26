using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public bool IsDead => currentHealth <= 0;
    [SerializeField] float maxHealth;
    float currentHealth;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    void IDamageable.Heal(float amt)
    {
       currentHealth = Mathf.Clamp(currentHealth + amt, 0, maxHealth);
    }

    void IDamageable.TakeDamage(float amt, EffectType effectType)
    {
        currentHealth -= amt;
        Debug.Log("Player taking damage: " + amt + " of type " + effectType);
        if (IsDead)
            Destroy(gameObject);
    }
}
