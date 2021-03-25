using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void OnDestroy()
    {
        if (Colour != "" && Colour != "none")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated -= CannonScript_OnActivated;
        }
    }

    private void CannonScript_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
        DelayedReset();
    }

    private void Activate()
    {
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null)
            {
                //GameObject BoulderInstance = Instantiate(Enemies[i], transform.position, new Quaternion(), transform) as GameObject;
                //Rigidbody RigidInstance = BoulderInstance.GetComponent<Rigidbody>();
                Enemies[i].SetActive(true);
                Rigidbody RigidInstance = Enemies[i].GetComponent<Rigidbody>();
                RigidInstance.AddForce((transform.forward + transform.up) * Power);
                Enemies[i] = null;
            }
        } 
    }

    private void Deactivate()
    {
        this.gameObject.GetComponent<SphereCollider>().enabled = true;
    }

    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(0.5f);
        Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Vector3 direction = transform.position - other.transform.position;
            //other.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * Power);

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
