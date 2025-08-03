using UnityEngine;

public class CollisionTrap : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] EffectType effectType;
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(damage, effectType);
        }
    }
}
