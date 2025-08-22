using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Abilities Temp/DensityDataTemp")]
public class DensityDataTemp : AbilityDataTemp {
    [Range(0, 1)] public float damageMultiplier;
    [SerializeField] private DensityEntity densityEntity;

    public DensityTemp CreateAbility() {
        return new DensityTemp(this, false);
    }

    public DensityEntity DensityEntity { get { return densityEntity; } }
}
