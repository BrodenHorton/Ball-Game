using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThirdPersonState : CameraState {
    private const float DEFAULT_NATURAL_CAM_DISTANCE = 6.0f;
    private const float DEFAULT_CAM_ROTATION_SPEED = 0.2f;
    private const int CAM_UPPER_PITCH_CLAMP = 60;
    private const int CAM_LOWER_PITCH_CLAMP = -40;
    private const float OBSTACLE_PADDING = 0.5f;

    private Transform camPivot;
    private Vector3 camStartingVector;
    private float naturalCamDistance;
    private float rotationSpeed;
    private LayerMask layerMask;

    private float mouseX;
    private float mouseY;
    private Vector3 camPosition;

    public CameraThirdPersonState(Transform camPivot) {
        this.camPivot = camPivot;
        camStartingVector = new Vector3(0, 1, -3);
        naturalCamDistance = DEFAULT_NATURAL_CAM_DISTANCE;
        rotationSpeed = DEFAULT_CAM_ROTATION_SPEED;
        layerMask = LayerMask.GetMask("Ground");
    }

    public override void EnterState(CameraStateManager camManager) {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Setting initial camera position
        camManager.setMouseX(camPivot.transform.eulerAngles.y);
        camManager.setMouseY(0);
    }

    public override void UpdateState(CameraStateManager camManager) {
        mouseY = Mathf.Clamp(mouseY, CAM_LOWER_PITCH_CLAMP, CAM_UPPER_PITCH_CLAMP);
        camPosition = camPivot.position + (Quaternion.Euler(mouseY, mouseX, 0f) * -Vector3.forward.normalized * naturalCamDistance);
        camManager.transform.position = camPosition;
        camManager.transform.LookAt(camPivot);

        CameraCollision(camManager);
    }

    private void CameraCollision(CameraStateManager camManager) {
        Vector3 baseToCamVector = (camManager.transform.position - camPivot.transform.position).normalized;

        if (Physics.Raycast(camPivot.transform.position, baseToCamVector, out RaycastHit hitInfo, naturalCamDistance, layerMask)) {
            Vector3 paddedVector = hitInfo.point + ((camPivot.transform.position - hitInfo.point).normalized * OBSTACLE_PADDING);
            camManager.transform.position = paddedVector;
        }
        else
            camManager.transform.position = camPivot.transform.position + (baseToCamVector * naturalCamDistance);
    }

    public override void MouseMovementCallback(CameraStateManager camManager, Vector2 mouseInput) {
        mouseX += mouseInput.x * rotationSpeed;
        mouseY -= mouseInput.y * rotationSpeed;
    }
}
