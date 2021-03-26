using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FloatingTextType
{
    Normal,
    DeathPoints,
}

public struct FloatingTextData
{
    public string Animation;
    public Color TextColor;
    public int FontSize;
}


public class FloatingTextManager : MonoBehaviour
{
    public GameObject FloatingTextPrefab;

    private Dictionary<FloatingTextType, FloatingTextData> FloatingTextDatas;

    private Camera MainCamera;
    private List<Transform> FloatingTexts;
    private Transform ParentFloatingTexts;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTextTypes();

        ParentFloatingTexts = new GameObject().transform;
        ParentFloatingTexts.name = "Floating Text";

        FloatingTexts = new List<Transform>();
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //TestCode();


        FloatingTexts.RemoveAll(item => item == null);

        Vector3 camLocation = MainCamera.transform.position;
        foreach (Transform text in FloatingTexts)
        {
            // Point towards the camera location
            text.rotation = Quaternion.LookRotation(text.position - camLocation);
        }
    }

    private void TestCode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateFloatingText(Vector3.zero, FloatingTextType.Normal, "Stuff");
        }
    }

    public void CreateFloatingText(Vector3 location, FloatingTextType type, string text)
    {
        // Get the data from the dictionary
        FloatingTextData data = FloatingTextDatas[type];

        // Create parent object (so that all animations are relative to this location)
        GameObject parent = new GameObject();
        parent.name = text;
        parent.transform.parent = ParentFloatingTexts;
        parent.transform.position = location;
        FloatingTexts.Add(parent.transform);

        // Instantiate a new floating text object
        GameObject floatingText = Instantiate(FloatingTextPrefab);
        floatingText.transform.parent = parent.transform;
        floatingText.transform.localPosition = Vector3.zero;

        // Rotate to face the camera
        Vector3 camLocation = MainCamera.transform.position;
        parent.transform.rotation = Quaternion.LookRotation(parent.transform.position - camLocation);

        // Update text component
        TextMesh textMesh = floatingText.GetComponent<TextMesh>();
        textMesh.color = data.TextColor;
        textMesh.text = text;
        textMesh.fontSize = data.FontSize;

        // Start playing animation
        Animator anim = floatingText.GetComponent<Animator>();
        anim.Play(data.Animation);

        // Destroy once animation is complete
        Destroy(parent, anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    private void GenerateTextTypes()
    {
        FloatingTextDatas = new Dictionary<FloatingTextType, FloatingTextData>();

        // Normal
        FloatingTextData normalType = new FloatingTextData() {
            Animation = "ScaleSmall",
            FontSize = 15,
            TextColor = Color.black };

        FloatingTextDatas.Add(FloatingTextType.Normal, normalType);


        // Death points
        FloatingTextData deathType = new FloatingTextData() {
            Animation = "FinalPoints",
            FontSize = 20,
            TextColor = Color.yellow };

        FloatingTextDatas.Add(FloatingTextType.DeathPoints, deathType);
    }
}
