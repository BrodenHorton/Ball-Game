using UnityEngine;

public class Shock : Ability
{
    ShockData abilityData => data as ShockData;

    PlayerMovement playerMovement;
    bool hitWhileDashing;
    /*2. Shock
    1. When the player dashes, the ball shoots out a lightning towards the nearest monster dealing lightning damage.
    2. On upgrade, the ball shoots out lightning automatically every 2 seconds towards the nearest enemy.
    3. On upgrade again, the ball erupts with an electrical explosion when dashed into the ground or an enemy shocking everything in its radius
    */
    public override void Activate()
    {
        if (isActivated) return;
        isActivated = true;
        activationTimer = new Timer(data.ActivatedLength);
        playerMovement = GameManager.Instance.getPlayer().GetComponent<PlayerMovement>();
        hitWhileDashing = false;
        Debug.Log("Activating Shock");
    }

    public override void DashedIntoEventHandler(Collision enemy)
    {

    }

    public override void Deactivate()
    {
        isActivated = false;
        Debug.Log("Deactivating Shock");
    }

    private void Update()
    {
        if (!isActivated) return;

        Debug.Log("Shock Active");
        activationTimer.Update();
        if (activationTimer.IsFinished())
            Deactivate();
        if (!hitWhileDashing && playerMovement.IsDashing)
        {
            var hits = Physics.OverlapSphere(playerMovement.transform.position, abilityData.shockAreaRadius, abilityData.enemyMask);
            for (int i = 0; i < hits.Length; i++)
            {
                var trans = hits[i].transform.GetParentOrSelf();
                if (trans.CompareTag("Enemy") && trans.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(abilityData.shockDamage, EffectType.ELECTRIC);
                    hitWhileDashing = true;
                    break;
                }
            }
        } else if (!playerMovement.IsDashing)
        {
            hitWhileDashing = false;
        }
    }

}
