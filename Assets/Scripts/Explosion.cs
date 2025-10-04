using UnityEngine;
using UnityEngine.UI;

public class Explosion : MonoBehaviour
{
    [SerializeField] LayerMask hittables;
    float damage;
    float radius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, hittables);
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
    }
    public void SetHittables(LayerMask hittables) => this.hittables = hittables;
    public void SetDamage(float damage) => this.damage = damage;
    public void SetRadius(float radius) => this.radius = radius;
}
