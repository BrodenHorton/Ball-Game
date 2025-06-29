using UnityEngine;

public class BasicRangedMovement : AIMovement
{
    [SerializeField] float stoppingDistance;
    public override Vector3 getTargetLocation()
    {
        Vector3 toSelf = (transform.position - movementTarget.position).normalized;
        return movementTarget.position + toSelf * stoppingDistance;
    }
}
