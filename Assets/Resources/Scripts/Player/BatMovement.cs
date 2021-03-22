using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour
{
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Transform MainCam;
    [SerializeField] private float Speed = 6f;

    public bool IsActive = false;

    float turnSmoothTime = 0f;
    float turnSmoothVel;

    private void FixedUpdate()
    {
        if (IsActive)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        //float hor = Input.GetAxisRaw("Horizontal");
        //float ver = Input.GetAxisRaw("Vertical");
        //Vector3 direction = new Vector3(hor, 0f, ver).normalized;

        //if (direction.magnitude >= 0.1f)
        //{

        //    Controller.Move(direction * Speed * Time.deltaTime);
        //}

        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(hor, 0f, ver).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + MainCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            Controller.Move(moveDir.normalized * Speed * Time.deltaTime);
        }


    }

    public void ActivateBat()
    {
        IsActive = true;
    }
}
