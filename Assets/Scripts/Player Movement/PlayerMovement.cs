using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float force;
    [SerializeField] float jumpCooldown = 3;
    [SerializeField] float dashCooldown = 3;
    [SerializeField] float jumpForce;
    [SerializeField] float additionalDashForce;
    [SerializeField] float maxLinearVelocity;

    private bool jumpPressed;
    private bool dashPressed;
    private Vector2 input;
    private Timer jumpTimer;
    private Timer dashTimer;

    private void Awake()
    {
        jumpTimer = new Timer(jumpCooldown);
        dashTimer = new Timer(dashCooldown);
        dashTimer.SetFinished();
        jumpTimer.SetFinished();
        rb.maxLinearVelocity = maxLinearVelocity;
    }
    private void Update() {
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        float tempForce = dashPressed && dashTimer.IsFinished() ? force + additionalDashForce : force;
        if (input.magnitude > 0)
        {
            rb.AddForce(Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * tempForce);
        }


        if (jumpPressed && jumpTimer.IsFinished())
        {
            Debug.Log("Player Jumping");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpTimer.Reset();
        }
        else
        {
            jumpTimer.Update();
        }
        if (!dashTimer.IsFinished())
        {
            dashTimer.Update();
        }
        else if (dashTimer.IsFinished() && dashPressed)
        {
            Debug.Log("Player Dashing");
            dashTimer.Reset();
        }
    }

    public void AddForce(InputAction.CallbackContext context) {
        input = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }else if (context.canceled)
        {
            jumpPressed = false;
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            dashPressed = true;
        }
        else if (context.canceled)
        {
            dashPressed = false;
        }
    }
}
