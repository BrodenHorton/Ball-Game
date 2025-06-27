using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] protected Transform target;
    private void Awake()
    {
        target.SetParent(null);
    }
    public virtual Vector3 getTargetLocation()
    {
        return target.position;
    }
}
