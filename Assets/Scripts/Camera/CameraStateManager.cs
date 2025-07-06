using UnityEngine;
using UnityEngine.InputSystem;

public class CameraStateManager : MonoBehaviour
{
    [SerializeField] private Transform camPivot;

    private CameraState currentState;
    private CameraThirdPersonState thirdPersonState;
    private CameraFreeState freeCamState;
    private CameraDetachedState detachedState;

    private float mouseX;
    private float mouseY;


    private void Start()
    {
        thirdPersonState = new CameraThirdPersonState(camPivot);
        freeCamState = new CameraFreeState(camPivot);
        detachedState = new CameraDetachedState();
        currentState = thirdPersonState;

        currentState.EnterState(this);
    }

    private void LateUpdate()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(CameraState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void SetMouseValues(InputAction.CallbackContext mouseInput) {
        //Debug.Log("SetMouseValues Called");
        currentState.MouseMovementCallback(this, mouseInput.ReadValue<Vector2>());
    }

    public float getMouseX() {
        return mouseX;
    }

    public void setMouseX(float value) {
        mouseX = value;
    }

    public float getMouseY() {
        return mouseY;
    }

    public void setMouseY(float value) {
        mouseY = value;
    }

    public CameraState GetCurrentCameraState() { return currentState; }

    public CameraThirdPersonState GetThirdPersonState() {  return thirdPersonState; }

    public CameraFreeState GetFreeCamState() {  return freeCamState; }

    public CameraDetachedState GetDetachedState() {  return detachedState; }
}
