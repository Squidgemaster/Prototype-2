using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.SceneManagement;

public class PistonScript : MonoBehaviour
{
    public string Colour = "";
    [SerializeField] private LayerMask Enemies;
    [SerializeField] private float FirePower = 100f;
    [SerializeField] private Animator Ani;

    private FloatingTextManager FTM;

    private void Start()
    {
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;

        if (Colour != "")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated += PistonScript_OnActivated;
        }

        FTM = FindObjectOfType<FloatingTextManager>();
    }

    private void OnDestroy()
    {
        if (Colour != "" && Colour != "none")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated -= PistonScript_OnActivated;
        }
    }

    private void PistonScript_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * 3.5f, 1f);
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    private void Activate()
    {
        // Find all nearby colliders
        Collider[] colliders = Physics.OverlapCapsule(transform.position, transform.position + transform.forward * 3.5f, 1f, Enemies);

        foreach (var hit in colliders)
        {
            if (hit.gameObject.tag == "Enemy")
            {
                //Turn Enemy into ragdolls
                hit.gameObject.GetComponentInParent<EnemyAI>().ActivateRagdoll();
                //Diable NavMesh
                hit.gameObject.GetComponentInParent<NavMeshAgent>().enabled = false;
                //Put object infront of this object
                hit.gameObject.transform.position = transform.position + transform.forward * 3.5f;
                //Add a force to all attached rigidbodies
                if (hit.gameObject.GetComponent<Rigidbody>() != null)
                {
                    hit.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    hit.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * FirePower, ForceMode.Impulse);
                }

                // Add score
                hit.GetComponentInParent<EnemyAI>().Score += 10;
                FTM.CreateFloatingText(hit.GetComponentInParent<EnemyAI>().RagdollRigidbodies[0].position, FloatingTextType.Normal, "+ 10");
            }
            else if (hit.gameObject.tag == "Boulder")
            {
                //Add a force to all attached rigidbodies
                if (hit.gameObject.GetComponent<Rigidbody>() != null)
                {
                    hit.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * FirePower / 20, ForceMode.Impulse);
                }
            }
        }
        
        Ani.SetTrigger("Attack");
    }
}
