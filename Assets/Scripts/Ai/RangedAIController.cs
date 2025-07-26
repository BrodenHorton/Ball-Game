using UnityEngine;

public class RangedAIController : AIController
{
    [SerializeField] Transform firePoint;
    protected override void Attack()
    {
        isAttacking = true;
        attackTimer.Update();
        if(target != null)
            transform.LookAt(target.transform.position);
        if (attackTimer.IsFinished())
        {
            isAttacking = false;
            movement.StartMoving();
            attackTimer.Reset();
            if (isWithinAttackRange)
            {
                Instantiate(data.projectile, firePoint.position, firePoint.rotation);
                Debug.Log("Attacking " + target);
            }
        }
        else
        {
            movement.StopMoving();
        }
    }
}
