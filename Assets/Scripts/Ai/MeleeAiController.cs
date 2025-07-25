using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AIMovement))]
public class MeleeAiController : AIController
{
    protected override void Attack()
    {
        isAttacking = true;
        attackTimer.Update();
        if (attackTimer.IsFinished())
        {
            isAttacking = false;
            movement.StartMoving();
            attackTimer.Reset();
            if(isWithinAttackRange && target.transform.GetParentOrSelf().TryGetComponent(out IDamageable damageable))
            {
                float depthPercentage = 1;
                damageable.TakeDamage(data.baseDamage * (1 + depthPercentage));
                Debug.Log("Attacking " + damageable);
            }
        }
        else
        {
            movement.StopMoving();
        }
    }
}
