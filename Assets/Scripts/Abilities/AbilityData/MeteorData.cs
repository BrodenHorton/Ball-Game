using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/MeteorData")]

public class MeteorData : AbilityData {
    [SerializeField] private MeteorShower meteorShowerPrefab;
    public GameObject meteorPrefab;
    public float spawnRadius;
    public float spawnRate;
    public float heightOffsetOfMeteor;
    [Header("Meteor Data")]
    [Header("TODO: Maybe make the data below a scriptable object?")]
    public float meteorMaxDamage;
    public float meteorDamageRadius;
    public float meteorSpeed;
    public float meteorDownwardMaxAngle;
    public LayerMask hittables;

    public override Ability CreateAbility() {
        MeteorShower meteorShower = Instantiate(meteorShowerPrefab.gameObject).GetComponent<MeteorShower>();
        meteorShower.AbilityData = this;
        return meteorShower;
    }

    public override void Upgrade() {

    }
}
