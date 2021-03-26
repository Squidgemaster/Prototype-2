using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlowerScript : MonoBehaviour
{
    string Colour = "";
    [SerializeField] private LayerMask Enemies;
    public float Power = 500f;
    private Animator BlowerAni;

    private void Start()
    {
        BlowerAni = this.gameObject.GetComponentInChildren<Animator>();
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;

        if (Colour != "")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated += BlowerScript_OnActivated;
        }
    }

    private void OnDestroy()
    {
        if (Colour != "")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated -= BlowerScript_OnActivated;
        }
    }

    private void BlowerScript_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void Activate()
    {
        Collider[] EnemiesInRange = Physics.OverlapCapsule(transform.position, transform.position + transform.forward * 9f, 1.2f, Enemies);

        if (EnemiesInRange.Length > 0)
        {
            for (int i = 0; i < EnemiesInRange.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, EnemiesInRange[i].transform.position);
                if (EnemiesInRange[i].gameObject.GetComponentInParent<EnemyAI>() != null)
                {
                    EnemiesInRange[i].gameObject.GetComponentInParent<EnemyAI>().ActivateRagdoll();
                    EnemiesInRange[i].gameObject.GetComponentInParent<NavMeshAgent>().enabled = false;
                    EnemiesInRange[i].gameObject.GetComponentInParent<EnemyAI>().ApplyForceToRagdoll(transform.forward * Power * 1 / distance, ForceMode.Impulse);
                }
                else if (EnemiesInRange[i].gameObject.tag == "Boulder")
                {
                    EnemiesInRange[i].gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Power * 1 / distance);
                }
            }
        }

        BlowerAni.SetTrigger("IsActive");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * 3f, 1.1f);
        Gizmos.DrawWireSphere(transform.position + transform.forward * 6f, 1.1f);
        Gizmos.DrawWireSphere(transform.position + transform.forward * 9f, 1.1f);
    }

}
