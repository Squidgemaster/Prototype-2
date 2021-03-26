using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CannonScript : MonoBehaviour
{
    public string Colour = "none";
    [SerializeField] private float FirePower;
    List<GameObject> Enemies = new List<GameObject>();

    private FloatingTextManager FTM;

    private void Start()
    {
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;

        if (Colour != "")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated += CannonScript_OnActivated;
        }

        LevelManager.LevelRestartEvent += LevelManager_LevelRestartEvent;
        FTM = FindObjectOfType<FloatingTextManager>();
    }

    private void LevelManager_LevelRestartEvent(object sender, System.EventArgs e)
    {
        this.gameObject.GetComponent<SphereCollider>().enabled = true;
        Enemies.Clear();
    }

    private void OnDestroy()
    {
        if (Colour != "" && Colour != "none")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated -= CannonScript_OnActivated;
        }
        LevelManager.LevelRestartEvent -= LevelManager_LevelRestartEvent;
        Enemies.Clear();
    }

    private void CannonScript_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
        StartCoroutine(DelayedReset());
    }

    private void Activate()
    {
        //diable sphere collider while firing
        this.gameObject.GetComponent<SphereCollider>().enabled = false;

        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].tag == "Enemy")
            {
                Enemies[i].transform.parent.gameObject.SetActive(true);

                //Put object infront of this object
                Enemies[i].transform.position = transform.position + transform.forward * 2f;

                // Add score
                Enemies[i].GetComponentInParent<EnemyAI>().Score += 25;
                FTM.CreateFloatingText(Enemies[i].GetComponentInParent<EnemyAI>().RagdollRigidbodies[0].position, FloatingTextType.Normal, "+ 25");

                Enemies.Remove(Enemies[i]);
            }
            else if (Enemies[i] != null && Enemies[i].gameObject.tag == "Boulder")
            {
                Rigidbody[] bodies = Enemies[i].GetComponentsInChildren<Rigidbody>();

                //Add a force to all attached rigidbodies
                for (int j = 0; j < bodies.Length; j++)
                {
                    bodies[j].velocity = Vector3.zero;
                    bodies[j].AddForce((transform.forward + transform.up) * FirePower, ForceMode.Impulse);
                }

                Enemies.Remove(Enemies[i]);
            }
        }
    }

    private void Deactivate()
    {
        this.gameObject.GetComponent<SphereCollider>().enabled = true;
    }

    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(1f);
        Deactivate();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boulder")
        {
            //Add enemy to list
            Enemies.Add(other.gameObject);
            //Turn Enemy into ragdolls
            Enemies[0].GetComponentInParent<EnemyAI>().ActivateRagdoll();
            //Diable NavMesh
            Enemies[0].gameObject.GetComponentInParent<NavMeshAgent>().enabled = false;
            //diable parent object
            Enemies[0].gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
