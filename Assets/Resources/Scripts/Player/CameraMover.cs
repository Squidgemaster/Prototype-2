using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    [Space(10)]
    public float RotateSpeed = 5f;
    public float ZoomSpeed = 3f;
    public float MoveSpeed = 10f;
    public float Smooth = 0.3f;

    [Space(10)]
    public float MaxRotation = 90f;
    public float MinRotation = 30f;

    [Space(10)]
    public float MaxZoom = 30f;
    public float MinZoom = 5f;

    private Vector3 LastMouseLocation;

    private Vector3 TargetFocusPosition;
    private float TargetDistance;
    private float TargetHorizontal;
    private float TargetVertical;

    private Vector3 CurrentFocusPosition;
    private float CurrentDistance;
    private float CurrentHorizontal;
    private float CurrentVertical;

    private void Start()
    {
        LastMouseLocation = Input.mousePosition;

        TargetFocusPosition = Vector3.zero;
        TargetDistance = 75.0f;
        TargetHorizontal = 0.0f;
        TargetVertical = 45.0f;

        CurrentFocusPosition = TargetFocusPosition;
        CurrentDistance = TargetDistance;
        CurrentHorizontal = TargetHorizontal;
        CurrentVertical = TargetVertical;
    }


    // Update is called once per frame
    void Update()
    {
        HandleInput();
        RotateCamera();
        MoveCamera();
    }

    void MoveCamera()
    {
        // Smooth lerp to target values
        CurrentDistance = Mathf.Lerp(CurrentDistance, TargetDistance, Smooth);
        CurrentHorizontal = Mathf.Lerp(CurrentHorizontal, TargetHorizontal, Smooth);
        CurrentVertical = Mathf.Lerp(CurrentVertical, TargetVertical, Smooth);
        CurrentFocusPosition = Vector3.Lerp(CurrentFocusPosition, TargetFocusPosition, Smooth);

        Vector3 worldPosition = (Vector3.forward * -CurrentDistance);
        Vector3 RotatedVec = Quaternion.AngleAxis(CurrentHorizontal, Vector3.up) * Quaternion.AngleAxis(CurrentVertical, Vector3.right) * worldPosition;
         
        transform.position = CurrentFocusPosition + RotatedVec;
        transform.LookAt(CurrentFocusPosition);
    }

    private void HandleInput()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(hor, 0f, ver).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + CurrentHorizontal;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            TargetFocusPosition += moveDir.normalized * MoveSpeed * Time.unscaledDeltaTime;
        }
    }

    void RotateCamera()
    {

        if (Input.GetMouseButton(2))
        {
            Cursor.lockState = CursorLockMode.Locked;

            Vector2 deltaPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            // Update horizontal component
            float targetHorizontal = TargetHorizontal + deltaPosition.x * Time.unscaledDeltaTime * RotateSpeed;
            TargetHorizontal = Mathf.Lerp(TargetHorizontal, targetHorizontal, Smooth);

            // Update vertical component
            float targetVertical = TargetVertical + -deltaPosition.y * Time.unscaledDeltaTime * RotateSpeed;
            targetVertical = Mathf.Clamp(targetVertical, MinRotation, MaxRotation);
            TargetVertical = Mathf.Lerp(TargetVertical, targetVertical, Smooth);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
        // Update distance
        TargetDistance *= (1.0f - (Input.mouseScrollDelta.y * ZoomSpeed));
        TargetDistance = Mathf.Clamp(TargetDistance, MinZoom, MaxZoom);
    }
}
