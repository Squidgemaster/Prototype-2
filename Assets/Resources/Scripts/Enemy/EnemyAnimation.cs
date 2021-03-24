using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public List<Collider> RagdollParts = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        GetRagdollParts();
        //ActivateRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Gets the collider parts of the ragdoll and sets them to trigger to prevent physics collisions
    private void GetRagdollParts()
    {
        Collider[] RagdollColliders = gameObject.GetComponentsInChildren<Collider>();

        foreach (Collider C in RagdollColliders)
        {
            if (C.gameObject != gameObject)
            {
                //C.isTrigger = true;
                RagdollParts.Add(C);
            }
        }
    }

    // Activates the physics of the ragdoll
    private void ActivateRagdoll()
    {
        foreach (Collider C in RagdollParts)
        {
            C.isTrigger = false;
            C.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}
