using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    PlayerMovement movement;
    [SerializeField] List<AbilityData> startingAbilities;
    List<Ability> abilities = new List<Ability>();
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
                EventBus.DashedInto?.Invoke(collision);
                movement.StopDashing();
            }
        }
    }
    public float GetDashDamage() => baseDashDamage;

    public void OnActionUsed(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int action = (int)context.ReadValue<float>();
        if (action <= abilities.Count - 1)
        {
            Debug.Log("On Activate Ability");
            abilities[action].Activate();
        }
    }
    public bool AddAbility(AbilityData abilityData)
	{
        if (abilities.Count >= 3)
            return false;
        Ability ability = abilityData.CreateAbility();
        abilities.Add(ability);
		EventBus.AbilityAdded?.Invoke(abilityData, abilities.IndexOf(ability));
        return true;
    }
    public void RemoveAbility(Ability ability)
    {
		EventBus.AbilityRemoved?.Invoke(ability.GetAbilityData(), abilities.IndexOf(ability));
		abilities.Remove(ability);
    }

    public bool UpgradeAbility(Ability ability)
    {
        bool success = ability.Upgrade();
        if (success)
        {
            EventBus.AbilityUpgraded?.Invoke(ability.GetAbilityData(), abilities.IndexOf(ability));
            EventBus.OnClosedAbilityUpgradeMenu?.Invoke();
            Debug.Log("Succeeded Upgrading Ability " + ability);
            return true;
        }
        Debug.Log("Failed Upgrading Ability " + ability);
        return false;
    }
    public List<Ability> GetAbilities() { return abilities; }
}
