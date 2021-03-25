using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public List<Quaternion> JointInitialRotation = new List<Quaternion>();
    public List<GameObject> AnimationObjects = new List<GameObject>();
    public List<Rigidbody> RagdollRigidbodies = new List<Rigidbody>();
    public List<ConfigurableJoint> RagdollJoints = new List<ConfigurableJoint>();
    public Transform EnemyTarget;

    private NavMeshAgent Agent;

    [SerializeField] private GameObject AnimationObject;
    [SerializeField] private GameObject PhysicalObject;
    [SerializeField] private Rigidbody RagdollRoot;

    private Vector3 FinishLocation;

    private bool RagdollActive;

    public enum RagdollPart
    {
        Pelvis,
        Head,
        Torso,
        LShoulder,
        LArm,
        LLeg,
        LFoot,
        RShoulder,
        RArm,
        RLeg,
        RFoot
    }

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();

        RagdollActive = false;
        FinishLocation = EnemyTarget.position;

        SetupRagdoll();

        // Make enemy move to specified location
        MoveToPosition(FinishLocation);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug tools
        #region Debug
        if (Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().name == "Rui")
        {
            //ActivateRagdoll();
            ApplyForceToRagdoll(new Vector3(20.0f, 10.0f), ForceMode.Impulse, RagdollPart.LLeg);
            Agent.ResetPath();
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (!RagdollActive)
        {
            UpdateJointTargetRotation();
        }
    }

    // Activates the ragdolling
    public void ActivateRagdoll()
    {
        RagdollActive = true;
        RagdollRoot.isKinematic = false;

        JointDrive JD = new JointDrive();

        foreach (ConfigurableJoint CJ in RagdollJoints)
        {
            CJ.angularXDrive = JD;
        }
    }

    // Apply an impulse force to ragdoll (Pelvis by default)
    public void ApplyForceToRagdoll(Vector3 Force, ForceMode Type)
    {
        ActivateRagdoll();
        RagdollRigidbodies[0].AddForce(Force, Type);
    }

    // Apply an impulse force to provided part
    public void ApplyForceToRagdoll(Vector3 Force, ForceMode Type, RagdollPart Part)
    {
        ActivateRagdoll();

        // Determine part to apply force to
        switch (Part)
        {
            case RagdollPart.Head:
                RagdollRigidbodies[8].AddForce(Force, Type);
                break;

            case RagdollPart.Torso:
                RagdollRigidbodies[5].AddForce(Force, Type);
                break;

            case RagdollPart.Pelvis:
                RagdollRigidbodies[0].AddForce(Force, Type);
                break;

            case RagdollPart.LShoulder:
                RagdollRigidbodies[6].AddForce(Force, Type);
                break;

            case RagdollPart.LArm:
                RagdollRigidbodies[7].AddForce(Force, Type);
                break;

            case RagdollPart.LLeg:
                RagdollRigidbodies[1].AddForce(Force, Type);
                break;

            case RagdollPart.LFoot:
                RagdollRigidbodies[2].AddForce(Force, Type);
                break;

            case RagdollPart.RShoulder:
                RagdollRigidbodies[9].AddForce(Force, Type);
                break;

            case RagdollPart.RArm:
                RagdollRigidbodies[10].AddForce(Force, Type);
                break;

            case RagdollPart.RLeg:
                RagdollRigidbodies[3].AddForce(Force, Type);
                break;

            case RagdollPart.RFoot:
                RagdollRigidbodies[4].AddForce(Force, Type);
                break;

            default:
                Debug.Log("Something has gone wrong with applying force to the ragdoll :c");
                break;
        }
    }

    // Gets the ragdoll objects and their animation counterparts
    private void SetupRagdoll()
    {
        // Get the two bodies of the ragdoll if it isn't specified already
        AnimationObject = AnimationObject == null ? transform.Find("Animated").gameObject : AnimationObject;
        PhysicalObject = PhysicalObject == null ? transform.Find("Physical").gameObject : PhysicalObject;

        // Add the pelvis to rigidbodies
        RagdollRigidbodies.Add(RagdollRoot);

        // Get ragdoll joints
        foreach (ConfigurableJoint CJ in PhysicalObject.GetComponentsInChildren<ConfigurableJoint>())
        {
            // Ignore the parent game object colliders
            if (CJ.gameObject != gameObject)
            {
                //C.isTrigger = true;
                RagdollJoints.Add(CJ);
                RagdollRigidbodies.Add(CJ.GetComponent<Rigidbody>());
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
