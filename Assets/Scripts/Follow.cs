using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
    }
}
