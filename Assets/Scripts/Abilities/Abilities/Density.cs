using System;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/Density")]
public class Density : Ability
{
    [NonSerialized] GameObject player;
    [NonSerialized] Rigidbody rb;
    [NonSerialized] float originalMass;
    [SerializeField][Range(0, 1)] float damageMultiplier;
    [NonSerialized] float damageToApply;
    
    /*
     * 4. Density
    1. Upon activation, your balls mass and gravity skyrockets. So does your damage upon dashing into an enemy.
    2. On upgrade, your dashing physical damage is tripled.
    3. On upgrade if you used Density and fell a short distance, an eruption of physical damage hurts anything nearby.
     */
    public override void Activate()
    {
        if (isActivated) return;
        ConnectToEvents();
        activationTimer = new Timer(activatedLength);
        this.player = GameManager.Instance.getPlayer();
        isActivated = true;
        rb = player.GetComponent<Rigidbody>();
        originalMass = rb.mass;
        rb.mass *= 3;
        var abilitiesComponent = player.GetComponent<PlayerAbilities>();
        var damage = abilitiesComponent.GetDashDamage();
        damageToApply = damageMultiplier * damage;
    }

    public override void Upgrade()
    {

    }
    public override void Update()
    {
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
        DisconnectEvents();
        rb.mass = originalMass;
    }

    public override void DashedIntoEventHandler(GameObject enemy)
    {
        if(enemy.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Dealing " + damageToApply + " to " + enemy);
            damageable.TakeDamage(damageToApply, EffectType.NORMAL);
        }
    }
}
