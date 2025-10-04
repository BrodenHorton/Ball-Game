using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/IgniteData")]
public class IgniteData : AbilityData {
    public float fireEffectStatusDuration;
    public float fireEffectDamageTickRate;
    public float fireDamage = 5;
    public bool explodeOnImpact;
    public Explosion explosionObject;
    public LayerMask explosionHittables;
    public float explosionDamage;
    public float explosionRadius;
}
