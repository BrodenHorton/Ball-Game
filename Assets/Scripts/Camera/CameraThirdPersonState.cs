using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThirdPersonState : CameraState {
    private static readonly float DEFAULT_NATURAL_CAM_DISTANCE = 10.0f;
    private static readonly float DEFAULT_CAM_ROTATION_SPEED = 0.2f;
    private static readonly float CAM_UPPER_PITCH_CLAMP = 120;
    private static readonly float CAM_LOWER_PITCH_CLAMP = 30f;
    private static readonly float OBSTACLE_PADDING = 0.5f;

    private Transform camPivot;
    private Vector3 camStartingVector;
    private float naturalCamDistance;
    private float rotationSpeed;
    private LayerMask layerMask;

    private float mouseX;
    private float mouseY;
    private bool hasMousePositionUpdated;

    public CameraThirdPersonState(Transform camPivot) {
        this.camPivot = camPivot;
        camStartingVector = new Vector3(0f, 1f, -4f);
        naturalCamDistance = DEFAULT_NATURAL_CAM_DISTANCE;
        rotationSpeed = DEFAULT_CAM_ROTATION_SPEED;
        layerMask = LayerMask.GetMask("Ground");
    }

    public override void EnterState(CameraStateManager camManager) {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camManager.transform.position = camPivot.position + camStartingVector.normalized * naturalCamDistance;
        camManager.transform.LookAt(camPivot);
    }

    public override void UpdateState(CameraStateManager camManager) {
        Vector3 camPosition;
        if (hasMousePositionUpdated) {
            float xRotation = mouseY;
            float xAngle = Vector3.Angle(Vector3.up, camManager.transform.position - camPivot.transform.position);
            if (xAngle + mouseY > CAM_UPPER_PITCH_CLAMP)
                xRotation = CAM_UPPER_PITCH_CLAMP - xAngle;
            else if (xAngle + mouseY < CAM_LOWER_PITCH_CLAMP)
                xRotation = -(xAngle - CAM_LOWER_PITCH_CLAMP);

            camPosition = camPivot.position + Quaternion.Euler(0f, mouseX, 0f) * Quaternion.AngleAxis(xRotation, Vector3.Cross(camManager.transform.forward.normalized, Vector3.up)) * -camManager.transform.forward.normalized * naturalCamDistance;

            mouseX = 0f;
            mouseY = 0f;
            hasMousePositionUpdated = false;
        }
        else
            camPosition = camPivot.position + -camManager.transform.forward.normalized * naturalCamDistance;

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
        mouseX = mouseInput.x * rotationSpeed;
        mouseY = mouseInput.y * rotationSpeed;
        hasMousePositionUpdated = true;
    }
}
