using UnityEngine;

public class DensityEntity : MonoBehaviour {
    [SerializeField] private DensityTemp density;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Timer activationTimer;
    [SerializeField] private float originalMass;
    [SerializeField] private float damageToApply;
    [SerializeField] private bool isActivated;

    private void OnEnable() {
        EventBus.DashedInto += DashedIntoEventHandler;
    }

    private void OnDisable() {
        EventBus.DashedInto -= DashedIntoEventHandler;
    }

    private void FixedUpdate() {
        if (!density.isActivated) return;

        rb.AddForce(Physics.gravity * 2);
        activationTimer.Update();
    }

    public void Activate(DensityTemp density, GameObject player) {
        if (isActivated)
            return;

        isActivated = true;
        this.density = density;
        activationTimer = new Timer(density.AbilityData.ActivatedLength, Deactivate);
        rb = player.GetComponent<Rigidbody>();
        originalMass = rb.mass;
        rb.mass *= 3;
        var abilitiesComponent = player.GetComponent<PlayerAbilities>();
        var damage = abilitiesComponent.GetDashDamage();
        damageToApply = density.AbilityData.damageMultiplier * damage;
        Debug.Log("Activated Density");
    }

    public void Deactivate() {
        Debug.Log("Deactivating Density");
        rb.mass = originalMass;
        Destroy(gameObject);
    }

    public void DashedIntoEventHandler(Collision enemy) {
        if (isActivated && enemy.transform.GetParentOrSelf().TryGetComponent(out IDamageable damageable)) {
            Debug.Log("Dealing " + damageToApply + " to " + enemy);
            damageable.TakeDamage(damageToApply, EffectType.NORMAL);
        }
    }
}