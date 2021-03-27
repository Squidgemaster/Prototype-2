using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, IGridObject
{
    public string Colour { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boulder") && Colour != "" && Colour != null)
        {
            ColourEventManager.ColourEvents[Colour].TriggerActivated();

            Debug.Log(Colour + " Blocks Activated!");
        }
    }
}
