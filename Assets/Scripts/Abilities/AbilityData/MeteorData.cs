using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/MeteorData")]

public class MeteorData : AbilityData {
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
    public EffectData meteorEffectData;
    public bool spawnFireZone;
}
