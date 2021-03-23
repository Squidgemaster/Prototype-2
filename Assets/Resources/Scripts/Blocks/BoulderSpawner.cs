using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    string Colour = "none";
    [SerializeField] private GameObject Boulder;

    private void Start()
    {
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;
        ColourEventManager.ColourEvents[Colour].OnActivated += BoulderSpawner_OnActivated;
    }

    private void BoulderSpawner_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void Activate()
    {
        Instantiate(Boulder, transform.position + new Vector3(0f, 1f, 0f), new Quaternion());
    }
}
