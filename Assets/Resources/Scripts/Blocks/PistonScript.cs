using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonScript : MonoBehaviour
{
    public string Colour = "none";
    [SerializeField] private LayerMask Enemies;
    [SerializeField] private float FirePower = 100f;
    [SerializeField] private Animator Ani;

    private void Start()
    {
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;
        ColourEventManager.ColourEvents["Red"].OnActivated += PistonScript_OnActivated;
    }


    private void PistonScript_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(4.5f, 1.5f, 1.4f), 1.4f);
    }

    private void Activate()
    {
        Collider[] EnemiesInRange = Physics.OverlapSphere(transform.position + new Vector3(4.5f, 1.5f, 1.4f), 1.4f, Enemies);

        for (int i = 0; i < EnemiesInRange.Length; i++)
        {
            EnemiesInRange[i].gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * FirePower);
            EnemiesInRange[i].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        }

        Ani.SetTrigger("Attack");
    }
}
