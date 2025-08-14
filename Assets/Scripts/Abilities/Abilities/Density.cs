using UnityEngine;
public class Density : Ability
{
    [SerializeField] private DensityData abilityData;
    GameObject player;
    Rigidbody rb;
    float originalMass;
    float damageToApply;
    
    /*
     * 4. Density
    1. Upon activation, your balls mass and gravity skyrockets. So does your damage upon dashing into an enemy.
    2. On upgrade, your dashing physical damage is tripled.
    3. On upgrade if you used Density and fell a short distance, an eruption of physical damage hurts anything nearby.
     */
    public override void Activate()
    {
        if (isActivated) return;
        activationTimer = new Timer(abilityData.ActivatedLength);
        this.player = GameManager.Instance.getPlayer();
        isActivated = true;
        rb = player.GetComponent<Rigidbody>();
        originalMass = rb.mass;
        rb.mass *= 3;
        var abilitiesComponent = player.GetComponent<PlayerAbilities>();
        var damage = abilitiesComponent.GetDashDamage();
        damageToApply = abilityData.damageMultiplier * damage;
    }

    private void FixedUpdate()
    {
        if (!isActivated) return;

        Debug.Log("Density Active");
        rb.AddForce(Physics.gravity * 2);
        activationTimer.Update();
        if (activationTimer.IsFinished())
            Deactivate();
    }
    public override void Deactivate()
    {
        isActivated = false;
        Debug.Log("Deactivating Density");
        rb.mass = originalMass;
        Destroy(gameObject);
    }

    public override void DashedIntoEventHandler(GameObject enemy)
    {
        if(isActivated && enemy.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Dealing " + damageToApply + " to " + enemy);
            damageable.TakeDamage(damageToApply, EffectType.NORMAL);
        }
    }

    public DensityData AbilityData { get { return abilityData; } set { abilityData = value; } }
}
