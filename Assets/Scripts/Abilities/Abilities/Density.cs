using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/Density")]
public class Density : Ability
{
    GameObject player;
    Rigidbody rb;
    float originalMass;
    [SerializeField][Range(0, 1)] float damageMultiplier;
    
    /*
     * 4. Density
    1. Upon activation, your balls mass and gravity skyrockets. So does your damage upon dashing into an enemy.
    2. On upgrade, your dashing physical damage is tripled.
    3. On upgrade if you used Density and fell a short distance, an eruption of physical damage hurts anything nearby.
     */
    public override void Activate()
    {
        if (isActivated) return;
        activationTimer = new Timer(activatedLength);
        this.player = GameManager.Instance.getPlayer();
        isActivated = true;
        rb = player.GetComponent<Rigidbody>();
        originalMass = rb.mass;
        rb.mass *= 3;
        var abilitiesComponent = player.GetComponent<PlayerAbilities>();
        var damage = abilitiesComponent.GetDashDamage();
        abilitiesComponent.ChangeDamageModifier(damage * damageMultiplier);
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
        rb.mass = originalMass;
        var abilitiesComponent = player.GetComponent<PlayerAbilities>();
        var damage = abilitiesComponent.GetDashDamage();
        abilitiesComponent.ChangeDamageModifier(-damage * damageMultiplier);
    }
}
