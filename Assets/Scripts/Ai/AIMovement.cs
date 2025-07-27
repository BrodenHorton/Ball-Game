using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] protected Transform movementTarget;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected float aiUpdateTick = 2;
    private GameObject target;
    Timer updateDestinationTimer;
    private void Awake()
    {
        movementTarget.SetParent(null);
        updateDestinationTimer = new Timer(aiUpdateTick);
    }
    private void Update()
    {
        updateDestinationTimer.Update();
        if (target != null && updateDestinationTimer.IsFinished())
        {
            updateDestinationTimer.Reset();
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
