using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    private NavMeshSurface Surface;

    // Start is called before the first frame update
    void Start()
    {
        Surface = GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateNavmesh()
    {
        Surface.BuildNavMesh();
    }
}
