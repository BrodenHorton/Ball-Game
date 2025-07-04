using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/DensityData")]
public class DensityData : AbilityData
{
    [Range(0, 1)] public float damageMultiplier;
}
