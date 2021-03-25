using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public string Colour = "";

    private void Start()
    {
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boulder" && Colour != "")
        {
            ColourEventManager.ColourEvents[Colour].TriggerActivated();

            Debug.Log(Colour + " Blocks Activated!");
        }
    }
}
