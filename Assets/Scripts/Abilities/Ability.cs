using UnityEngine;


public abstract class Ability : MonoBehaviour {
    public bool isPassive;
    public bool isActivated;
    public int level = 1;
    protected Timer activationTimer;

    public abstract AbilityData GetAbilityData();
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void DashedIntoEventHandler(GameObject enemy);

    public abstract bool Upgrade();

    private void OnEnable()
    {
        EventBus.DashedInto += DashedIntoEventHandler;
    }
    private void OnDisable()
    {
        EventBus.DashedInto -= DashedIntoEventHandler;
    }
}
