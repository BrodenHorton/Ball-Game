using UnityEngine;

public class Density : Ability
{
    GameObject player;
    Rigidbody rb;
    float originalMass;
    
    /*
     * 4. Density
    1. Upon activation, your balls mass and gravity skyrockets. So does your damage upon dashing into an enemy.
    2. On upgrade, your dashing physical damage is tripled.
    3. On upgrade if you used Density and fell a short distance, an eruption of physical damage hurts anything nearby.
     */
    public override void Activate()
    {
        if (isActivated) return;
        this.player = GameManager.Instance.getPlayer();
        isActivated = true;
        rb = player.GetComponent<Rigidbody>();
        originalMass = rb.mass;
    }

    public override void Upgrade()
    {

    }
    public override void Update()
    {
        rb.AddForce(Physics.gravity * 2);
    }
}
