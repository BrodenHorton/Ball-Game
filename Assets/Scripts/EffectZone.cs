using System.Collections.Generic;
using UnityEngine;

public class EffectZone : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> systems;
    EffectData effect;

    public void Setup(EffectData data)
    {
       this.effect = data;

        foreach (ParticleSystem p in systems)
        {
            ParticleSystem.ShapeModule shape = p.shape;
            shape.radius = data.radius;
        }
        Destroy(gameObject, data.lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var hit = other.transform.GetParentOrSelf();
        if (hit.TryGetComponent(out StatusEffectRunner runner) && hit.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Applying Damage Effect to " + hit.name);
            runner.ApplyEffect(new DamageEffect(effect.damage, effect.duration, effect.damageTick, damageable, effect.effectType));
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
