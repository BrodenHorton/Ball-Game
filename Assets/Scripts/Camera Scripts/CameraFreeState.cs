using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeState : CameraState
{
    private const float STANDARD_SPEED = 30f;
    private const float SPRINT_SPEED = 60f;
    private const float VERTICAL_SPEED = 25f;

    private Transform camPivot;
    private Vector3 camStartingVector;
    private float camStartingDistance;
    private float rotationSpeed;
    private int camUpperPitchClamp;
    private int camLowerPitchClamp;

    private float mouseX;
    private float mouseY;

    private bool isSprinting;

    public CameraFreeState(Transform camPivot)
    {
        this.camPivot = camPivot;
        camStartingVector = new Vector3(0, 0.5f, 1);
        camStartingDistance = 1.5f;
        rotationSpeed = 2.75f;
        camUpperPitchClamp = 80;
        camLowerPitchClamp = -80;
    }

    public override void EnterState(CameraStateManager camManager)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camManager.transform.position = camPivot.position + (Quaternion.Euler(0, camPivot.parent.transform.eulerAngles.y, 0) * camStartingVector.normalized * camStartingDistance);
        mouseX = camPivot.parent.transform.eulerAngles.y;
        mouseY = 0;
        isSprinting = false;
    }

    public override void UpdateState(CameraStateManager camManager)
    {
        //Camera Rotation
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, camLowerPitchClamp, camUpperPitchClamp);

        camManager.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        //Camera Movement
        float xAxisRaw = Input.GetAxisRaw("Horizontal");
        float zAxisRaw = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown("q"))
            isSprinting = !isSprinting;

        float moveSpeed = (isSprinting) ? SPRINT_SPEED : STANDARD_SPEED;

        float verticalVector = 0f;
        if (Input.GetKey(KeyCode.Space))
            verticalVector = VERTICAL_SPEED;
        else if(Input.GetKey(KeyCode.LeftAlt))
            verticalVector = -VERTICAL_SPEED;

        Vector3 horizontalVector = new Vector3(xAxisRaw, 0, zAxisRaw).normalized * moveSpeed;
        Vector3 moveVector = new Vector3(horizontalVector.x, verticalVector, horizontalVector.z);
        camManager.transform.position = camManager.transform.position + (Quaternion.Euler(0, camManager.transform.eulerAngles.y, 0) * moveVector * Time.deltaTime);
    }
}
