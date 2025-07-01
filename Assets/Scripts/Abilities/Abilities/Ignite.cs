using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Abilities/Ignite")]
public class Ignite : Ability
{
    [SerializeField] float fireEffectStatusDuration;
    [SerializeField] float fireEffectDamageTickRate;
    /*
     * 1. Ignite
    1. Upon activation, the Ball erupts with fire. Any Dashed into enemy catches on fire for 10 seconds and takes damage for the duration
    2. With an upgrade, when the ball makes contact after a dash and is currently on fire, an explosion happens.
    3. With another upgrade, the ball leaves a radius of fire that deals damage to any enemies in its radius while its burning.*/

    public override void Activate()
    {
        if (isActivated) return;
        isActivated = true;
        activationTimer = new Timer(activatedLength);
        ConnectToEvents();
        Debug.Log("Activating Ignite");
    }

    public override void Deactivate()
    {
        DisconnectEvents();
    }

    public override void Update()
    {
        Debug.Log("Ignite Active");
        activationTimer.Update();
        if (activationTimer.IsFinished())
            Deactivate();
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }
    public override void DashedIntoEventHandler(GameObject enemy)
    {
        if(enemy.TryGetComponent(out StatusEffectRunner runner) && enemy.TryGetComponent(out IDamageable damageable))
        {
            runner.ApplyEffect(new FireDamageEffect(fireEffectStatusDuration, fireEffectDamageTickRate, damageable));
            Debug.Log("Applying Fire Effect");
        }
    }
}
