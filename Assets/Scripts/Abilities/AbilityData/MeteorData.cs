using UnityEngine;

public class MeteorData : AbilityData
{
    public GameObject meteorPrefab;
    public float spawnRadius;
    public float spawnRate;
    public float heightOffsetOfMeteor;
    [Header("Meteor Data")]
    [Header("TODO: Maybe make the data below a scriptable object?")]
    public float meteorMaxDamage;
    public float meteorDamageRadius;
}
