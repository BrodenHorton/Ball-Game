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
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] GameObject landingEffect, groundPoundEffect;
    private bool isGrounded;
    public bool IsDashing => !dashLengthTimer.IsFinished();
    private bool jumpPressed;
    private bool dashPressed;
    private Vector2 input;
    private Timer jumpTimer;
    private Timer dashCooldownTimer;
    private Timer dashLengthTimer; 

    private void Awake()
    {
        jumpTimer = new Timer(jumpCooldown);
        dashCooldownTimer = new Timer(dashCooldown);
        dashLengthTimer = new Timer(dashLength);
        dashCooldownTimer.SetFinished();
        dashLengthTimer.SetFinished();
        jumpTimer.SetFinished();
        rb.maxLinearVelocity = maxLinearVelocity;
    }

    private void Update()
    {
        dashLengthTimer.Update();
        jumpTimer.Update();
        dashCooldownTimer.Update();
    }
    private void FixedUpdate() {
        Debug.Log("Dash Value: " + IsDashing);
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, groundDistance, groundMask);
        if (!isGrounded && hits.Length > 0) {
            isGrounded = true;
            SpawnImpactParticles(hits[0].normal, hits[0].point);
        }
        else if (hits.Length == 0)
            isGrounded = false;

        if (isGrounded)
            jumpTimer.SetFinished();

        if (jumpPressed && jumpTimer.IsFinished())
            Jump();
        else if (dashCooldownTimer.IsFinished() && dashPressed)
            Dash();

        if (dashLengthTimer.IsFinished())
            StopDashing();

        //This should be last to do
        if (dashLengthTimer.IsFinished() && input.magnitude > 0)
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.y = 0f;
            float airControlFactor = isGrounded ? 1f : 0.2f; // 0.2 is orginal value = weak air control and used to refuce floatyness. One way to solve this is to have a "move timer" in the air and disable this method when it expires
            rb.AddForce(airControlFactor * (force * moveDir.normalized) + Physics.gravity, ForceMode.Force);
        }
    }

    public void Jump()
    {
        Debug.Log("Player Jumping");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpTimer.Reset();
    }

    public void StopDashing()
    {
        rb.maxLinearVelocity = maxLinearVelocity;
        EventBus.Dashing?.Invoke(false); //This gets called too often, TODO
        dashCooldownTimer.SetFinished();
        dashLengthTimer.SetFinished();
    }

    public void Dash()
    {
        Debug.Log("Player Dashing");
        rb.maxLinearVelocity = maxLinearVelocity + dashVelocity;
        rb.linearVelocity = cameraTransform.forward * dashVelocity;
        EventBus.Dashing?.Invoke(true);
        dashCooldownTimer.Reset();
        dashLengthTimer.Reset();
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
    public void CancelDash()
    {
        dashCooldownTimer.SetFinished();
        dashLengthTimer.SetFinished();
    }

    void SpawnImpactParticles(Vector3 normal, Vector3 point)
    {

        GameObject objToSpawn = IsDashing ? groundPoundEffect : landingEffect;

        // Align the effect with the surface normal
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        // Instantiate at the contact point, facing out from the surface
        Instantiate(objToSpawn, point, rotation);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded && !collision.collider.CompareTag("Enemy"))
        {
            SpawnImpactParticles(collision.contacts[0].normal, collision.contacts[0].point);
        }
    }
}
