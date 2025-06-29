using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    PlayerMovement movement;
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
}
