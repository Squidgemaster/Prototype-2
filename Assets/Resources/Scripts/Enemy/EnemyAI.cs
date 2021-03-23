using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
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
        Agent = GetComponent<NavMeshAgent>();
        RB = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
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

        // Set the animation speed param
        Anim.SetFloat("Speed", RB.velocity.magnitude);
    }

    // Shimmie along to a given position
    private void MoveToPosition(Vector3 Pos)
    {
        Agent.SetDestination(Pos);

        // Purple line for current chasing target
        Debug.DrawLine(transform.position, Pos, Color.magenta);
    }
}
