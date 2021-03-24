using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public string Colour = "none";
    [SerializeField] private GameObject Boulder;
    [SerializeField] private float Power;

    private void Start()
    {
        Colour = GameObject.Find("Radial Menu - Colours").gameObject.GetComponent<RadialMenu>().SelectedSegment;
        
        if (Colour != "")
        {
            ColourEventManager.ColourEvents[Colour].OnActivated += PistonScript_OnActivated;
        }
    }

    private void PistonScript_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void Activate()
    {
        GameObject BoulderInstance = Instantiate(Boulder, transform.position, new Quaternion(), transform) as GameObject;
        Rigidbody RigidInstance = BoulderInstance.GetComponent<Rigidbody>();

        RigidInstance.AddForce((transform.forward + transform.up) * Power);
    }
}
