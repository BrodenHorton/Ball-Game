using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    PlayerMovement movement;
    [SerializeField] List<Ability> abilities;
    [SerializeField] float dashDamage;
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
                damageable.TakeDamage(dashDamage);
                movement.CancelDash();
            }
        }
    }
    private void Update()
    {
        abilities.ForEach(ability => {
            if(!ability.needsPhysicsUpdate)
                ability.Update();
            });
    }
    private void FixedUpdate()
    {
        abilities.ForEach(ability => {
            if (ability.needsPhysicsUpdate)
                ability.Update();
        });
    }
    public void OnActionUsed(int action)
    {
        if (abilities.Count-1 <= action)
        {
            abilities[action].Activate();
        }
    }
}
