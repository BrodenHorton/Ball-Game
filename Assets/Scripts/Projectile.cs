using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float lifeTime;
    [SerializeField] bool canStickInWall;
    bool isStuckInWall;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if(!isStuckInWall)
            transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetParentOrSelf().TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(canStickInWall)
        {
            isStuckInWall = true;
        }
    }
}
