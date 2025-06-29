using UnityEngine;

public class PlaneResize : MonoBehaviour
{
    [SerializeField] Vector2 planeDimensions;

    private void Start() {
        transform.localScale = new Vector3(planeDimensions.x, transform.localScale.y, planeDimensions.y);
    }
}
