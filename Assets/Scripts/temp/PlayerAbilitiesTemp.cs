using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilitiesTemp : MonoBehaviour {
    PlayerMovement movement;
    [SerializeReference, SubclassSelector] List<AbilityTemp> startingAbilities;
    List<AbilityTemp> abilities = new List<AbilityTemp>();
    [SerializeField] float baseDashDamage;

    private void Awake() {
        movement = GetComponent<PlayerMovement>();
    }

    private void Start() {
        foreach (var ability in startingAbilities) {
            AddAbility(ability);
        }
    }

    private void Update() {
        foreach(AbilityTemp ability in abilities)
            ability.Tick();
    }

    private void OnCollisionEnter(Collision collision) {
        if (movement.IsDashing) {
            Transform collider = collision.transform.GetParentOrSelf();
            if (collider.TryGetComponent(out IDamageable damageable)) {
                Debug.Log("Dealing damage to " + damageable);
                damageable.TakeDamage(baseDashDamage, EffectType.NORMAL);
                EventBus.DashedInto?.Invoke(collider.gameObject);
                movement.StopDashing();
            }
        }
    }

    public float GetDashDamage() => baseDashDamage;

    public void OnActionUsed(InputAction.CallbackContext context) {
        if (!context.performed)
            return;

        int action = (int)context.ReadValue<float>();
        if (action <= abilities.Count - 1) {
            Debug.Log("On Activate Ability");
            abilities[action].Activate(this.gameObject);
        }
    }

    public bool AddAbility(AbilityTemp ability) {
        if (abilities.Count >= 3)
            return false;

        abilities.Add(ability);
        //EventBus.AbilityAdded?.Invoke(ability, abilities.IndexOf(ability));
        return true;
    }

    public void RemoveAbility(AbilityTemp ability) {
        //EventBus.AbilityRemoved?.Invoke(ability.GetAbilityData(), abilities.IndexOf(ability));
        abilities.Remove(ability);
    }

    public bool UpgradeAbility(Ability ability) {
        bool success = ability.Upgrade();
        if (success) {
            //EventBus.AbilityUpgraded?.Invoke(ability.GetAbilityData(), abilities.IndexOf(ability));
            return true;
        }
        return false;
    }

    public List<AbilityTemp> GetAbilities() { return abilities; }
}
