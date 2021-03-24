using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    string Colour = "none";
    [SerializeField] private GameObject Boulder;
    GameObject BoulderManager;

    private void Start()
    {
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;
        
        if (Colour != "")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated += BoulderSpawner_OnActivated;
        }
        
        BoulderManager = Instantiate(Boulder, transform.position + new Vector3(0f, 1f, 0f), new Quaternion()) as GameObject;
    }

    private void BoulderSpawner_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void Activate()
    {
        Destroy(BoulderManager);
        BoulderManager = null;
        BoulderManager = Instantiate(Boulder, transform.position + new Vector3(0f, 1f, 0f), new Quaternion()) as GameObject;
    }
}
