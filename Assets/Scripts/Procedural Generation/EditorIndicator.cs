using UnityEngine;

public class EditorIndicator : MonoBehaviour {
    [SerializeField] private Color color;

    private void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
