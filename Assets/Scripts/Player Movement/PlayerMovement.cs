using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float force;
    [SerializeField] float jumpCooldown = 3;
    [SerializeField] float dashCooldown = 3;
    [SerializeField] float jumpForce;
    [SerializeField] float dashVelocity;
    [SerializeField] float dashLength;
    [SerializeField] float maxLinearVelocity;

    private bool jumpPressed;
    private bool dashPressed;
    private Vector2 input;
    private Timer jumpTimer;
    private Timer dashTimer;
    private Timer dashLengthTimer; 

    private void Awake()
    {
        jumpTimer = new Timer(jumpCooldown);
        dashTimer = new Timer(dashCooldown);
        dashLengthTimer = new Timer(dashLength);
        dashTimer.SetFinished();
        dashLengthTimer.SetFinished();
        jumpTimer.SetFinished();
        rb.maxLinearVelocity = maxLinearVelocity;
    }
    private void FixedUpdate() {
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        float tempForce = force;



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
            rb.maxLinearVelocity = maxLinearVelocity + dashVelocity;
            rb.linearVelocity = cameraTransform.forward * dashVelocity;
            dashTimer.Reset();
            dashLengthTimer.Reset();
        }
        if (dashLengthTimer.IsFinished())
        {
            rb.maxLinearVelocity = maxLinearVelocity;
        }
        else
        {
            dashLengthTimer.Update();
        }
        //This should be last to do
        if (dashLengthTimer.IsFinished() && input.magnitude > 0)
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.y = 0f;
            rb.AddForce(moveDir.normalized * tempForce);
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
