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

        Vector3 worldPosition = (Vector3.forward * -Distance) + (Vector3.up * Height);

        Vector3 RotatedVec = Quaternion.AngleAxis(Angle, Vector3.up) * worldPosition;

        Vector3 TargetPos = Target.position;
        TargetPos.y = 0f;

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
                Angle += Time.deltaTime * RotateSpeed * 100f;
            }
            else if (Input.GetAxis("Mouse X") > 0)
            {
                Angle -= Time.deltaTime * RotateSpeed * 100f;
            }


        }
    }
}