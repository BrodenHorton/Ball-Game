using UnityEngine;

public class BasicRangedMovement : AIMovement
{
    [SerializeField] float stoppingDistance;
    public override Vector3 getTargetLocation()
    {
        Vector3 toSelf = (transform.position - target.position).normalized;
        return target.position + toSelf * stoppingDistance;
    }
}
