using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour
{
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Transform MainCam;
    [SerializeField] private float Speed = 6f;

    public bool IsActive = false;

    // Update is called once per frame
    void Update()
    {
        //if (IsActive)
        //{
        //    if (transform.position.y > 30f)
        //    {
        //        transform.position = new Vector3(transform.position.x, 30f, transform.position.z);
        //    }
        //}
    }

    private void FixedUpdate()
    {
        if (IsActive)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(hor, 0f, ver).normalized;

        if (direction.magnitude >= 0.1f)
        {
            
            Controller.Move(direction * Speed * Time.deltaTime);
        }
    }

    public void ActivateBat()
    {
        IsActive = true;
    }
}
