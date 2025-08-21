using UnityEngine;

public abstract class AbilityGeneric<T> : MonoBehaviour where T : AbilityData
{
    [SerializeField] protected T abilityData;

    public abstract void Activate();

    public abstract void Deactivate();
}
