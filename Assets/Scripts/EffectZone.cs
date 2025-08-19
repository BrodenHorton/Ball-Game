using System.Collections.Generic;
using UnityEngine;

public class EffectZone : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> systems;
    [SerializeField] float radius;
    [SerializeField] EffectType damageType;
    [SerializeField] float damage;
    [SerializeField] float duration;
    [SerializeField] float damageTick;
    [SerializeField] float lifeTime;

    public void Setup(EffectData data)
    {
        this.lifeTime = data.lifeTime;
        this.radius = data.radius;
        this.damage = data.damage;
        this.duration = data.duration;
        this.damageTick = data.damageTick;
        this.damageType = data.effectType;

        foreach (ParticleSystem p in systems)
        {
            ParticleSystem.ShapeModule shape = p.shape;
            shape.radius = radius;
        }
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var hit = other.transform.GetParentOrSelf();
        if (hit.TryGetComponent(out StatusEffectRunner runner) && hit.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Applying Damage Effect to " + hit.name);
            runner.ApplyEffect(new DamageEffect(damage, duration, damageTick, damageable, damageType));
        }
    }
}
[System.Serializable]
public class EffectData
{
    public EffectType effectType;
    public float damage;
    public float duration;
    public float lifeTime;
    public float damageTick;
    public float radius;
}
