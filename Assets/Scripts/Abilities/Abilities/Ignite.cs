using UnityEngine;
public class Ignite : Ability
{
    [SerializeField] private IgniteData abilityData;

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

    public override void DashedIntoEventHandler(GameObject enemy)
    {
        if(isActivated && enemy.TryGetComponent(out StatusEffectRunner runner) && enemy.TryGetComponent(out IDamageable damageable))
        {
            var data = abilityData;
            runner.ApplyEffect(new DamageEffect(data.fireDamage, data.fireEffectStatusDuration, data.fireEffectDamageTickRate, damageable, EffectType.FIRE));
            Debug.Log("Applying Fire Effect");
        }
    }

    public override bool Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public override AbilityData GetAbilityData()
    {
        return abilityData;
    }
    public void SetAbilityData(IgniteData abilityData) => this.abilityData = abilityData;

}
