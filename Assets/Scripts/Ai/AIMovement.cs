using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] protected Transform movementTarget;
    private GameObject target;
    private void Awake()
    {
        movementTarget.SetParent(null);
    }
    private void Start()
    {
        target = GameManager.Instance.getPlayer();
    }
    private void Update()
    {
        if (target != null)
        {
            movementTarget.position = target.transform.position;
        }
    }
    public virtual Vector3 getTargetLocation()
    {
        return movementTarget.position;
    }
}
