using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] Vector3 offset;

    private void Update()
    {
        if(target != null)
            transform.position = target.position + offset;
    }
}
