using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessorUpdater : MonoBehaviour
{
    public Material mat;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }


    // Update is called once per frame
    [ExecuteInEditMode]
    void Update()
    {
        mat.SetMatrix("InverseProjection", Camera.main.projectionMatrix.inverse);
    }
}
