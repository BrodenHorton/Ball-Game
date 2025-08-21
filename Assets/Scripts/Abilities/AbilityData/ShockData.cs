using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/ShockData")]
public class ShockData : AbilityData {
    public float shockDamage;
    public float shockAreaRadius = 10;
    public LayerMask enemyMask;
}
