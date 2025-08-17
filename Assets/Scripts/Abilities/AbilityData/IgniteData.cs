using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/IgniteData")]
public class IgniteData : AbilityData {
    [SerializeField] private Ignite ignitePrefab;
    public float fireEffectStatusDuration;
    public float fireEffectDamageTickRate;
    public float fireDamage = 5;

    public override Ability CreateAbility() {
        Ignite ignite = Instantiate(ignitePrefab);
        ignite.AbilityData = this;
        return ignite;
    }

    public override void Upgrade() {

    }
}
