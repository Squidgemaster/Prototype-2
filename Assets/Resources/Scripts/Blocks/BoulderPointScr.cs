using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoulderPointScr : MonoBehaviour
{
    FloatingTextManager FTM;

    public void Start()
    {
        FTM = FindObjectOfType<FloatingTextManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponentInParent<EnemyAI>();

        if (enemy != null)
        {
            enemy.Score += 5;
            FTM.CreateFloatingText(enemy.RagdollRigidbodies[0].position, FloatingTextType.Normal, "+ 5");

            enemy.ActivateRagdoll();
            enemy.GetComponentInParent<NavMeshAgent>().enabled = false;
        }
    }
}
