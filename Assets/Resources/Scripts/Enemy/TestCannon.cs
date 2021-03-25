using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCannon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Ground")
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(25.0f, 0.0f), ForceMode.Impulse);
        }
    }
}
