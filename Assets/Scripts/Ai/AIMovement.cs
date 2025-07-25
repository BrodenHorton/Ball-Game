using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] protected Transform movementTarget;
    [SerializeField] protected NavMeshAgent agent;
    private GameObject target;

    private void Awake()
    {
        movementTarget.SetParent(null);
    }
    private void Update()
    {
        if (target != null)
        {
            movementTarget.position = target.transform.position;
            agent.SetDestination(getTargetLocation());
        }
    }
    public virtual Vector3 getTargetLocation()
    {
        return movementTarget.position;
    }
    public virtual void StartMoving()
    {
        agent.isStopped = false;
    }
    public virtual void StopMoving()
    {
        agent.isStopped = true;
    }
    public GameObject getTarget() {  return target; }
    public void SetTarget(GameObject target) { this.target = target; }
}
