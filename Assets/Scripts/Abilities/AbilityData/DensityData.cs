using System;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/DensityData")]
public class DensityData : AbilityData {
    [SerializeField] private Density densityPrefab;
    [Range(0, 1)] public float damageMultiplier;

    public override Ability CreateAbility() {
        Density density = Instantiate(densityPrefab.gameObject).GetComponent<Density>();
        density.AbilityData = this;
        return density;
    }

    public override void Upgrade() {
        
    }
}
