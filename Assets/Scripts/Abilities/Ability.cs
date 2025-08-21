using UnityEngine;


public abstract class Ability : MonoBehaviour {
    public bool isPassive;
    public bool isActivated;
    protected Timer activationTimer;
    [SerializeField] protected AbilityData data;


    public AbilityData GetAbilityData() => data;
    public void SetAbilityData(AbilityData abilityData) => this.data = abilityData;
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void DashedIntoEventHandler(GameObject enemy);

    public bool Upgrade()
    {
        AbilityData possibleUpgradeData = data.Upgrade();
        if (possibleUpgradeData != null)
        {
            data = possibleUpgradeData;
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        EventBus.DashedInto += DashedIntoEventHandler;
    }
    private void OnDisable()
    {
        EventBus.DashedInto -= DashedIntoEventHandler;
    }
}