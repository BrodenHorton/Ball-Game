using UnityEngine;

[System.Serializable]
public class DensityTemp : AbilityTemp {
    [SerializeField] private DensityDataTemp abilityData;

    // Defualt constructor required for SerializeReferenceExtensions
    public DensityTemp() : base(false) {

    }

    public DensityTemp(DensityDataTemp abilityData, bool isPassive) : base(isPassive) {
        this.abilityData = abilityData;
        cooldownTimer = new Timer(abilityData.ActivatedLength, ResetActivation);
    }

    public override void Activate(GameObject parent) {
        if (isActivated)
            return;

        isActivated = true;
        DensityEntity densityEntity = Object.Instantiate(abilityData.DensityEntity);
        densityEntity.Activate(this, parent);
    }

    public void ResetActivation() {
        isActivated = false;
    }

    public DensityDataTemp AbilityData => abilityData;
}
