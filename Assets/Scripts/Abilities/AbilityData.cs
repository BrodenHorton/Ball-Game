using UnityEngine;

public abstract class AbilityData : ScriptableObject {
    [SerializeField] protected float activatedLength;
    [SerializeField] protected Sprite icon;

    public abstract Ability CreateAbility();

    public abstract void Upgrade();

    public float ActivatedLength { get { return activatedLength; } }

    public Sprite Icon { get { return icon; } }
}
