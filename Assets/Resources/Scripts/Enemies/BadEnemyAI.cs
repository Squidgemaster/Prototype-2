using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEnemyAI : MonoBehaviour
{
    private Rigidbody RB;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RB.position += new Vector3(10.0f, 0.0f, 0.0f) * Time.deltaTime;
    }
}
