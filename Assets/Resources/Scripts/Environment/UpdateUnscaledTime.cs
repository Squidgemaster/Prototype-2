using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUnscaledTime : MonoBehaviour
{
    Material MainMaterial;

    // Start is called before the first frame update
    void Start()
    {
        MainMaterial = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        MainMaterial.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
