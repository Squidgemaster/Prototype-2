using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    [SerializeField] private Transform Target;
    [SerializeField] private float Distance = 10f;
    [SerializeField] private float Height = 30f;
    [SerializeField] private float Angle = 0f;
    [SerializeField] private float RotateSpeed = 5f;
    [SerializeField] private float ZoomSpeed = 3f;
    [SerializeField] private float MaxRotation = 90f;
    [SerializeField] private float MinRotation = 30f;
    [SerializeField] private float MaxZoom = 30f;
    [SerializeField] private float MinZoom = 5f;

    float newXAngle;
    float newYAngle;


    private void Start()
    {
        newXAngle = Angle;
        newYAngle = Height;
    }


    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        RotateCamera();
    }

    void MoveCamera()
    {
        if (!Target)
        {
            return;
        }

        Vector3 worldPosition = (Vector3.forward * -Distance);

        Vector3 RotatedVec = Quaternion.AngleAxis(Angle, Vector3.up) * Quaternion.AngleAxis(Height, Vector3.right) * worldPosition;
   


        Vector3 TargetPos = Target.position;
        //TargetPos.y = 0f;

        Vector3 finalPos = TargetPos + RotatedVec;
 
        transform.position = finalPos;
        transform.LookAt(TargetPos);

    }

    void RotateCamera()
    {
        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse X") < 0)
            {

                newXAngle -= Time.deltaTime * RotateSpeed;
                Angle = Mathf.Lerp(Angle, newXAngle, 0.2f);
            }
            else if (Input.GetAxis("Mouse X") > 0)
            {
                newXAngle += Time.deltaTime * RotateSpeed;
                Angle = Mathf.Lerp(Angle, newXAngle, 0.2f);
            }



            if (Input.GetAxis("Mouse Y") < 0)
            {
                newYAngle += Time.deltaTime * RotateSpeed;
                Height = Mathf.Lerp(Height, newYAngle, 0.2f);
                if (Height > MaxRotation)
                {
                    Height = MaxRotation;
                    newYAngle = Height;
                }
            }
            else if (Input.GetAxis("Mouse Y") > 0)
            {
                newYAngle -= Time.deltaTime * RotateSpeed;
                Height = Mathf.Lerp(Height, newYAngle, 0.2f);
                if (Height < -MinRotation)
                {
                    Height = -MinRotation;
                    newYAngle = Height;
                }
            }

        }
        
        Distance *= (1.0f - (Input.mouseScrollDelta.y * ZoomSpeed));

        if (Distance > MaxZoom)
        {
            Distance = MaxZoom;
        }
        else if (Distance < MinZoom)
        {
            Distance = MinZoom;
        }
    }
}
