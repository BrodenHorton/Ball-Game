using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThirdPersonState : CameraState {
    private Transform camPivot;
    private Vector3 camStartingVector;
    private float naturalCamDistance;
    private float rotationSpeed;
    private int camUpperPitchClamp;
    private int camLowerPitchClamp;
    private float obstaclePadding;
    private LayerMask layerMask;

    private float mouseX;
    private float mouseY;

    public CameraThirdPersonState(Transform camPivot) {
        this.camPivot = camPivot;
        camStartingVector = new Vector3(0, 1, -3);
        naturalCamDistance = 6f;
        rotationSpeed = 2.75f;
        camUpperPitchClamp = 50;
        camLowerPitchClamp = -50;
        obstaclePadding = 0.5f;
        layerMask = LayerMask.GetMask("Ground");
    }

    public override void EnterState(CameraStateManager camManager) {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Setting initial camera position
        mouseX = camPivot.parent.transform.eulerAngles.y;
        mouseY = 0;
    }

    public override void UpdateState(CameraStateManager camManager) {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, camLowerPitchClamp, camUpperPitchClamp);

        Vector3 CamPosition = camPivot.position + (Quaternion.Euler(mouseY, mouseX, 0f) * camStartingVector.normalized * naturalCamDistance);
        camManager.transform.position = CamPosition;

        camManager.transform.LookAt(camPivot);

        CameraCollision(camManager);
    }

    private void CameraCollision(CameraStateManager camManager) {
        Vector3 baseToCamVector = (camManager.transform.position - camPivot.transform.position).normalized;

        if (Physics.Raycast(camPivot.transform.position, baseToCamVector, out RaycastHit hitInfo, naturalCamDistance, layerMask)) {
            Vector3 paddedVector = hitInfo.point + ((camPivot.transform.position - hitInfo.point).normalized * obstaclePadding);
            camManager.transform.position = paddedVector;
        }
        else
            camManager.transform.position = camPivot.transform.position + (baseToCamVector * naturalCamDistance);
    }
}
