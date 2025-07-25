using UnityEngine;


public abstract class Ability : MonoBehaviour
{
    public bool isPassive;
    public bool isActivated;
    protected Timer activationTimer;
    [SerializeField] protected AbilityData abilityData;
    public abstract void Activate();
    public abstract void Upgrade();
    public abstract void Deactivate();
    public abstract void DashedIntoEventHandler(GameObject enemy);

    private void OnEnable()
    {
        EventBus.DashedInto += DashedIntoEventHandler;
    }
    private void OnDisable()
    {
        EventBus.DashedInto -= DashedIntoEventHandler;
    }
    public AbilityData GetAbilityData() => abilityData;
}
