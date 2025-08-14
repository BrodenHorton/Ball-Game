using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    PlayerMovement movement;
    [SerializeField] List<AbilityData> startingAbilities;
    List<AbilityData> abilities = new List<AbilityData>();
    [SerializeField] float baseDashDamage;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        foreach(var ability in startingAbilities)
        {
            AddAbility(ability);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (movement.IsDashing)
        {
            Transform collider = collision.transform.GetParentOrSelf();
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log("Dealing damage to " + damageable);
                damageable.TakeDamage(baseDashDamage, EffectType.NORMAL);
                EventBus.DashedInto?.Invoke(collider.gameObject);
                movement.StopDashing();
            }
        }
    }
    public float GetDashDamage() => baseDashDamage;

    public void OnActionUsed(int action)
    {
        if (action <= abilities.Count - 1)
        {
            Ability ability = abilities[action].CreateAbility();
            Debug.Log("On Activate Ability");
            ability.Activate();
        }
    }
    public bool AddAbility(AbilityData abilityData)
	{
        if (abilities.Count >= 3)
            return false;
		abilities.Add(abilityData);
		EventBus.AbilityAdded?.Invoke(abilityData, abilities.IndexOf(abilityData));
        return true;
    }
    public void RemoveAbility(AbilityData abilityData)
    {
		EventBus.AbilityRemoved?.Invoke(abilityData, abilities.IndexOf(abilityData));
		abilities.Remove(abilityData);
    }
}
