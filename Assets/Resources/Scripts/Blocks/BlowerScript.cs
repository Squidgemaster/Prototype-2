using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowerScript : MonoBehaviour
{
    string Colour = "none";
    [SerializeField] private LayerMask Enemies;
    float Power = 100f;
    GameObject[] EnemyArray = new GameObject[20];

    private void Start()
    {
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;
        ColourEventManager.ColourEvents[Colour].OnActivated += BlowerScript_OnActivated;
    }

    private void BlowerScript_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void Activate()
    {
        if (EnemyArray.Length > 0)
        {
            for (int i = 0; i < EnemyArray.Length; i++)
            {
                if (EnemyArray[i] != null)
                {
                    float distance = Vector3.Distance(transform.position, EnemyArray[i].transform.position);
                    EnemyArray[i].gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Power * 1 / distance);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            
                bool isInArray = false;

                for (int i = 0; i < EnemyArray.Length; i++)
                {
                    if (EnemyArray[i] == other.gameObject)
                    {
                        isInArray = true;
                    }
                }

                if (!isInArray)
                {
                    for (int i = 0; i < EnemyArray.Length; i++)
                    {
                        if (EnemyArray[i] == null)
                        {
                            EnemyArray[i] = other.gameObject;
                            break;
                        }
                    }
                }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            for (int i = 0; i < EnemyArray.Length; i++)
            {
                if (EnemyArray[i] != null && EnemyArray[i].gameObject == other.gameObject)
                {
                    EnemyArray[i] = null;
                }
            }
        }
    }
}
