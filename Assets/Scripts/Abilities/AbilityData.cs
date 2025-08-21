using UnityEngine;

public class AbilityData : ScriptableObject {
    [SerializeField] protected float activatedLength;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected AbilityData nextUpgrade;

    [SerializeField] private Ability abilityPrefab;

    public AbilityData Upgrade()
    {
        return nextUpgrade;
    }
    public Ability CreateAbility()
    {
        Ability ability = Instantiate(abilityPrefab);
        ability.SetAbilityData(this);
        return ability;
    }
    public float ActivatedLength { get { return activatedLength; } }

    public Sprite Icon { get { return icon; } }
}
