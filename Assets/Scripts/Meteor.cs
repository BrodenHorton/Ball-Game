using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("These arent editable in inspector")]
    [SerializeField] float radius;
    [SerializeField] float damage;
    [SerializeField] LayerMask mask;
    public void Prepare(MeteorData data)
    {
        damage = data.meteorMaxDamage;
        radius = data.meteorDamageRadius;
        mask = data.hittables;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, mask);
        foreach (Collider hit in hits)
        {
            var h = hit.transform.GetParentOrSelf();
            if (h.TryGetComponent(out IDamageable damageable))
            {
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                float distancePercent = Mathf.Clamp01(distance / radius); // 0 at center, 1 at edge
                float dam = damage * Mathf.Pow(1f - distancePercent, 0.5f); // square falloff
                damageable.TakeDamage(dam);
            }
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
