using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    PlayerMovement movement;
    [SerializeField] List<Ability> startingAbilities;
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
                EventBus.DashedInto?.Invoke(collider.gameObject);
                movement.CancelDash();
            }
        }
    }
    public float GetDashDamage() => baseDashDamage;

    public void OnActionUsed(int action)
    {
        if (action <= abilities.Count - 1)
        {
            abilities[action].Activate();
        }
    }
    public void AddAbility(Ability ability)
	{
		Ability ab = Instantiate(ability, transform);
		abilities.Add(ab);
		EventBus.AbilityAdded?.Invoke(ab, abilities.IndexOf(ab));
    }
    public void RemoveAbility(Ability ability)
    {
        ability.Deactivate();
		EventBus.AbilityRemoved?.Invoke(ability, abilities.IndexOf(ability));
		abilities.Remove(ability);
        Destroy(ability.gameObject);
    }
}
