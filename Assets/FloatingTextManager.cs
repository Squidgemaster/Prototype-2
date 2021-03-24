using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct FloatingTextData
{
    public string Animation;

    public string Text;
    public Color TextColor;
    public float Size;
}


public class FloatingTextManager : MonoBehaviour
{
    public GameObject FloatingTextPrefab;

    private Camera MainCamera;
    private List<Transform> FloatingTexts;
    private Transform ParentFloatingTexts;

    // Start is called before the first frame update
    void Start()
    {
        ParentFloatingTexts = new GameObject().transform;
        ParentFloatingTexts.name = "Floating Text";

        FloatingTexts = new List<Transform>();
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camLocation = MainCamera.transform.position;

        foreach (Transform text in FloatingTexts)
        {
            // Point towards the camera location
            text.rotation = Quaternion.LookRotation(text.position - camLocation);
        }
    }

    private void TestCode()
    {
       // Screen.ray
    }

    public void CreateFloatingText(Vector3 location, FloatingTextData data)
    {
        // Create parent object (so that all animations are relative to this location)
        GameObject parent = new GameObject();
        parent.transform.parent = ParentFloatingTexts;
        parent.transform.position = location;

        // Instantiate a new floating text object
        GameObject floatingText = Instantiate(FloatingTextPrefab);
        floatingText.transform.parent = parent.transform;

        // Rotate to face the camera
        Vector3 camLocation = MainCamera.transform.position;
        floatingText.transform.rotation = Quaternion.LookRotation(floatingText.transform.position - camLocation);

        // Update text component
        TMPro.TMP_Text text = floatingText.GetComponent<TMPro.TMP_Text>();
        text.color = data.TextColor;
        text.text = data.Text;
        text.fontSize = data.Size;

        // Start playing animation
        floatingText.GetComponent<Animator>().Play(data.Animation);
    }
}
