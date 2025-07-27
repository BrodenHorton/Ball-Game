using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class BallMovement : MonoBehaviour {
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed;

    private CharacterController controller;
    private SphereCollider SphereCollider;
    private Vector2 input;

    private void Awake() {
        controller = GetComponent<CharacterController>();
        SphereCollider = GetComponent<SphereCollider>();
    }

    private void FixedUpdate() {
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        if (input.magnitude > 0) {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.y = 0f;
            controller.Move(moveDir * speed);
            Vector3 right = -Vector3.Cross(moveDir, Vector3.up);
            Debug.DrawRay(transform.position, right * 10, Color.red);
            transform.RotateAround(transform.position, right, speed / (float)(2 * SphereCollider.radius * Mathf.PI) * 360);
        }
    }

    public void SetMouseInput(InputAction.CallbackContext context) {
        input = context.ReadValue<Vector2>();
    }
}
