using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateManager : MonoBehaviour
{
    [SerializeField] private Transform camPivot;

    private CameraState currentState;
    private CameraThirdPersonState thirdPersonState;
    private CameraFreeState freeCamState;
    private CameraDetachedState detachedState;


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

    public CameraState GetCurrentCameraState() { return currentState; }

    public CameraThirdPersonState GetThirdPersonState() {  return thirdPersonState; }

    public CameraFreeState GetFreeCamState() {  return freeCamState; }

    public CameraDetachedState GetDetachedState() {  return detachedState; }
}
