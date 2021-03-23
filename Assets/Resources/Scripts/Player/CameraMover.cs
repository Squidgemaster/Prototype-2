using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    public Transform Target;

    [Space(10)]
    public float RotateSpeed = 5f;
    public float ZoomSpeed = 3f;
    public float Smooth = 0.3f;

    [Space(10)]
    public float MaxRotation = 90f;
    public float MinRotation = 30f;

    [Space(10)]
    public float MaxZoom = 30f;
    public float MinZoom = 5f;

    private Vector3 LastMouseLocation;

    private float TargetDistance = 10f;
    private float TargetHorizontal;
    private float TargetVertical;

    private float CurrentDistance;
    private float CurrentHorizontal;
    private float CurrentVertical;

    private void Start()
    {
        LastMouseLocation = Input.mousePosition;

        TargetHorizontal = 0.0f;
        TargetVertical = 0.0f;

        CurrentDistance = TargetDistance;
        CurrentHorizontal = TargetHorizontal;
        CurrentVertical = TargetVertical;
    }


    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        RotateCamera();
    }

    void MoveCamera()
    {
        if (!Target) { return; }

        // Smooth lerp to target values
        CurrentDistance = Mathf.Lerp(CurrentDistance, TargetDistance, Smooth);
        CurrentHorizontal = Mathf.Lerp(CurrentHorizontal, TargetHorizontal, Smooth);
        CurrentVertical = Mathf.Lerp(CurrentVertical, TargetVertical, Smooth);

        Vector3 worldPosition = (Vector3.forward * -CurrentDistance);
        Vector3 RotatedVec = Quaternion.AngleAxis(CurrentHorizontal, Vector3.up) * Quaternion.AngleAxis(CurrentVertical, Vector3.right) * worldPosition;
         
        transform.position = Target.position + RotatedVec;
        transform.LookAt(Target.position);
    }

    void RotateCamera()
    {
        // Get delta position of mouse (avoids stiff movement)
        Vector3 currentPositon = Input.mousePosition;
        Vector3 deltaPositon = currentPositon - LastMouseLocation;
        LastMouseLocation = currentPositon;

        if (Input.GetMouseButton(2))
        {
            // Update horizontal component
            float targetHorizontal = TargetHorizontal + deltaPositon.x * Time.deltaTime * RotateSpeed;
            TargetHorizontal = Mathf.Lerp(TargetHorizontal, targetHorizontal, Smooth);

            // Update vertical component
            float targetVertical = TargetVertical + -deltaPositon.y * Time.deltaTime * RotateSpeed;
            targetVertical = Mathf.Clamp(targetVertical, MinRotation, MaxRotation);
            TargetVertical = Mathf.Lerp(TargetVertical, targetVertical, Smooth);
        }
        
        // Update distance
        TargetDistance *= (1.0f - (Input.mouseScrollDelta.y * ZoomSpeed));
        TargetDistance = Mathf.Clamp(TargetDistance, MinZoom, MaxZoom);
    }
}
