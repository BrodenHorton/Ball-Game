using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float force;
    
    private Vector2 input;


    private void Start() {
    }

    private void Update() {
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        if (input.magnitude > 0)
            rb.AddForce(Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * force);
    }

    public void AddForece(InputAction.CallbackContext context) {
        input = context.ReadValue<Vector2>();
    }
}
