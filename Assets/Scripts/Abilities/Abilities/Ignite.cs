using UnityEngine;
public class Ignite : Ability
{
    IgniteData abilityData => data as IgniteData;
    /*
     * 1. Ignite
    1. Upon activation, the Ball erupts with fire. Any Dashed into enemy catches on fire for 10 seconds and takes damage for the duration
    2. With an upgrade, when the ball makes contact after a dash and is currently on fire, an explosion happens.
    3. With another upgrade, the ball leaves a radius of fire that deals damage to any enemies in its radius while its burning.*/

    public override void Activate()
    {
        if (isActivated) return;
        isActivated = true;
        activationTimer = new Timer(abilityData.ActivatedLength, Deactivate);
        Debug.Log("Activating Ignite");
    }

    public override void Deactivate()
    {
        isActivated = false;
        Debug.Log("Deactivating Ignite");
    }

    private void Update()
    {
        if (!isActivated) return;

        Debug.Log("Ignite Active");
        activationTimer.Update();
    }

    public override void DashedIntoEventHandler(Collision enemy)
    {
        if(isActivated && enemy.transform.GetParentOrSelf().TryGetComponent(out StatusEffectRunner runner) && enemy.transform.GetParentOrSelf().TryGetComponent(out IDamageable damageable))
        {
            runner.ApplyEffect(new DamageEffect(abilityData.fireDamage, abilityData.fireEffectStatusDuration, abilityData.fireEffectDamageTickRate, damageable, EffectType.FIRE));
            Debug.Log("Applying Fire Effect");
            if (abilityData.explodeOnImpact)
            {
                Explosion explosion = Instantiate(abilityData.explosionObject, enemy.GetContact(0).point, Quaternion.identity);
                explosion.transform.up = enemy.GetContact(0).normal;
                explosion.SetHittables(abilityData.explosionHittables);
                explosion.SetDamage(abilityData.explosionDamage);
                explosion.SetRadius(abilityData.explosionRadius);
            }
        }
    }

    public void SetAbilityData(IgniteData abilityData) {
        data = abilityData;
    }
}
