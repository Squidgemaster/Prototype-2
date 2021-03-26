using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

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
        Collider[] EnemiesInRange = Physics.OverlapCapsule(transform.position, transform.position + transform.forward *3.5f, 1f, Enemies);

        for (int i = 0; i < EnemiesInRange.Length; i++)
        {
            if (EnemiesInRange[i].gameObject.tag == "Enemy")
            {
                EnemiesInRange[i].GetComponentInParent<EnemyAI>().ActivateRagdoll();
                EnemiesInRange[i].GetComponentInParent<NavMeshAgent>().enabled = false;
                EnemiesInRange[i].GetComponentInParent<EnemyAI>().ApplyForceToRagdoll(transform.forward * FirePower, ForceMode.Impulse);
                EnemiesInRange[i].GetComponentInParent<EnemyAI>().Score += 10;
                FTM.CreateFloatingText(EnemiesInRange[i].GetComponentInParent<EnemyAI>().transform.position, "Normal", "OH MY GOD DID YOU JUST SMASH THAT POOR LAD WITH A GOD DAMN PISTON I'LL GIVE YOU A THUNDEROUS APPLUD AND AWARD YOU WITH 10 POINTS!!!");
            }
            else if (EnemiesInRange[i].gameObject.tag == "Boulder")
            {
                EnemiesInRange[i].gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * FirePower);
            }
        }

        Ani.SetTrigger("Attack");
    }
}
