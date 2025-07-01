using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    PlayerMovement movement;
    [SerializeField] List<Ability> abilities;
    [SerializeField] float baseDashDamage;
    [SerializeField] float abilityDamageModifier = 0;
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (movement.IsDashing)
        {
            Transform collider = collision.transform.GetParentOrSelf();
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log("Dealing damage to " + damageable);
                damageable.TakeDamage(baseDashDamage);
                EventBus.EnemyHit.Invoke(collider.gameObject);
                movement.CancelDash();
            }
        }
    }
    public float GetDashDamage() => baseDashDamage;
    private void Update()
    {
        abilities.ForEach(ability => {
            if((ability.isPassive || ability.isActivated) && !ability.needsPhysicsUpdate)
                ability.Update();
            });
    }
    private void FixedUpdate()
    {
        abilities.ForEach(ability => {
            if ((ability.isPassive || ability.isActivated) && ability.needsPhysicsUpdate)
                ability.Update();
        });
    }
    public void OnActionUsed(int action)
    {
        Debug.Log("Activating " + action);
        if (abilities.Count-1 <= action)
        {
            abilities[action].Activate();
        }
    }
    public void RemoveAbility(Ability ability)
    {
        ability.Deactivate();
        abilities.Remove(ability);
    }
}
