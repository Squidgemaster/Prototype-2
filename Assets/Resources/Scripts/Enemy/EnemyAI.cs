using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public List<GameObject> AnimationObjects = new List<GameObject>();
    public List<Quaternion> JointInitialRotation = new List<Quaternion>();
    public List<ConfigurableJoint> RagdollJoints = new List<ConfigurableJoint>();

    private NavMeshAgent Agent;
    private NavMeshManager MeshManager;
    private Animator Anim;

    // Pathfinding
    //[Header("Pathfinding Properties")]
    private Rigidbody RB;
    private GameObject SpawnLocation;

    private Vector3 StartLocation;
    private Vector3 FinishLocation;

    // Debugging things
    //[Header("Debugging")]

    // Start is called before the first frame update
    void Start()
    {
        //Agent = GetComponent<NavMeshAgent>();
        //RB = GetComponent<Rigidbody>();
        //Anim = GetComponent<Animator>();
        SetupRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug tools
        #region Debug
        if (Input.GetMouseButtonDown(0))
        {

        }

        // Reset
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Agent.ResetPath();
        }
        #endregion

    }

    private void FixedUpdate()
    {
        UpdateJointTargetRotation();
    }

    // Gets the ragdoll objects and their animation counterparts
    private void SetupRagdoll()
    {
        // Get the two bodies of the ragdoll
        GameObject AnimationObject = transform.Find("Animated").gameObject;
        GameObject PhysicalObject = transform.Find("Physical").gameObject;

        // Get ragdoll colliders
        foreach (ConfigurableJoint CJ in PhysicalObject.GetComponentsInChildren<ConfigurableJoint>())
        {
            // Ignore the parent game object colliders
            if (CJ.gameObject != gameObject)
            {
                //C.isTrigger = true;
                RagdollJoints.Add(CJ);
            }
        }

        // Get animation object corresponding to the physical ragdoll part
        foreach (Transform Child in AnimationObject.GetComponentsInChildren<Transform>())
        {
            foreach (ConfigurableJoint CJ in RagdollJoints)
            {
                if (CJ.gameObject.name == Child.name)
                {
                    AnimationObjects.Add(Child.gameObject);
                }
            }
        }

        // Save the inital rotation of the joints
        foreach(ConfigurableJoint CJ in RagdollJoints)
        {
            JointInitialRotation.Add(CJ.transform.rotation);
        }

    }

    // Does what the name says
    private void UpdateJointTargetRotation()
    {
        for (int i = 0; i < RagdollJoints.Count; i++)
        {
            ConfigurableJointExtensions.SetTargetRotationLocal(RagdollJoints[i], AnimationObjects[i].transform.rotation, JointInitialRotation[i]);
        }
    }

    // Shimmie along to a given position
    private void MoveToPosition(Vector3 Pos)
    {
        Agent.SetDestination(Pos);

        // Purple line for current chasing target
        Debug.DrawLine(transform.position, Pos, Color.magenta);
    }
}
