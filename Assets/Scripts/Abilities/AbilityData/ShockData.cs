using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/ShockData")]
public class ShockData : AbilityData {
    [SerializeField] private Shock shockPrefab;
    public float shockDamage;
    public float shockAreaRadius = 10;
    public LayerMask enemyMask;

    public override Ability CreateAbility() {
        Shock shock = Instantiate(shockPrefab);
        shock.AbilityData = this;
        return shock;
    }

    public override void Upgrade() {

    }
}
