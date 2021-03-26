using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CannonScript : MonoBehaviour
{
    public string Colour = "none";
    [SerializeField] private GameObject[] Enemies = new GameObject[20];
    [SerializeField] private float Power;

    private void Start()
    {
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;
        
        if (Colour != "")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated += CannonScript_OnActivated;
        }

        LevelManager.LevelRestartEvent += LevelManager_LevelRestartEvent;
    }

    private void LevelManager_LevelRestartEvent(object sender, System.EventArgs e)
    {
        this.gameObject.GetComponent<SphereCollider>().enabled = true;
    }

    private void OnDestroy()
    {
        if (Colour != "" && Colour != "none")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated -= CannonScript_OnActivated;
        }
        LevelManager.LevelRestartEvent -= LevelManager_LevelRestartEvent;
    }

    private void CannonScript_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
        StartCoroutine(DelayedReset());
    }

    private void Activate()
    {
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null && Enemies[i].gameObject.GetComponentInParent<EnemyAI>() != null)
            {
                Enemies[i].transform.parent.gameObject.SetActive(true);
                Enemies[i].gameObject.GetComponentInParent<EnemyAI>().ActivateRagdoll();
                Enemies[i].gameObject.GetComponentInParent<NavMeshAgent>().enabled = false;
                Enemies[i].gameObject.GetComponentInParent<EnemyAI>().ApplyForceToRagdoll((transform.forward + transform.up) * Power, ForceMode.Impulse);

                Enemies[i] = null;
            }
            else if (Enemies[i].gameObject.tag == "Boulder" && Enemies[i] != null)
            {
                Enemies[i].gameObject.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * Power);
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
        if (other.gameObject.tag == "Enemy")
        {
            //add gameobject to array of objects  to store in the cannon
            bool isInArray = false;
            //check if this object already exists in the array
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (Enemies[i] == other.gameObject)
                {
                    isInArray = true;
                }
            }
            //if not add it to the array
            if (!isInArray)
            {
                for (int i = 0; i < Enemies.Length; i++)
                {
                    if (Enemies[i] == null)
                    {
                        Enemies[i] = other.gameObject as GameObject;
                        other.gameObject.transform.parent.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }

        else if (other.gameObject.tag == "Boulder")
        {

            //add gameobject to array of objects  to store in the cannon
            bool isInArray = false;
            //check if this object already exists in the array
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (Enemies[i] == other.gameObject)
                {
                    isInArray = true;
                }
            }
            //if not add it to the array
            if (!isInArray)
            {
                for (int i = 0; i < Enemies.Length; i++)
                {
                    if (Enemies[i] == null)
                    {
                        Enemies[i] = other.gameObject as GameObject;
                        other.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}
