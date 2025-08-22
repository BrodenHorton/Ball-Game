using UnityEngine;

public abstract class AbilityDataTemp : ScriptableObject {
    [SerializeField] protected float activatedLength;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected AbilityData nextUpgrade;

    public AbilityData Upgrade() {
        return nextUpgrade;
    }

    public float ActivatedLength { get { return activatedLength; } }

    public Sprite Icon { get { return icon; } }
}
