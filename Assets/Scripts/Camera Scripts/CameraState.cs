using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CameraState
{
    public abstract void EnterState(CameraStateManager camManager);
    public abstract void UpdateState(CameraStateManager camManager);

    public abstract void MouseMovementCallback(CameraStateManager camManager, Vector2 mouseInput);
}
