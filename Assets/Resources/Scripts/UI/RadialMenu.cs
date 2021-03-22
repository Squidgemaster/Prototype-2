using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [System.Serializable]
    public struct Item
    {
        public string Name;
        public Material Material;
    }

    [Header("Function")]
    public List<Item> Items;
    public KeyCode MenuVisibilityToggle;

    [Header("Visuals")]
    public float InnerRadius = 10.0f;
    public float OuterRadius = 50.0f;
    public float SelectDistance = 5.0f;
    public float AlphaMultiplier = 2.0f;

    [Header("Script Use")]
    public bool IsVisible = false;
    public string SelectedSegment = "";


    private GameObject[] Segments;
    private float Alpha = 0.0f;
    private bool IsExited;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        Segments = new GameObject[Items.Count];

        // Generate the mesh
        float segmentSize = 360.0f / Items.Count;
        Mesh segmentMesh = GenerateSlice(segmentSize, InnerRadius, OuterRadius);

        // Generate all the segment objects
        for (int i = 0; i < Items.Count; ++i)
        {
            GameObject segmentObject = new GameObject();
            Segments[i] = segmentObject;

            // Update transform
            segmentObject.transform.parent = transform;
            segmentObject.transform.localRotation = Quaternion.AngleAxis(i * segmentSize, new Vector3(0.0f, 1.0f, 0.0f));
            segmentObject.transform.localPosition = Vector3.zero;
            segmentObject.layer = LayerMask.NameToLayer("UI");
            segmentObject.name = "Segment " + i;

            // Add mesh rendering components
            MeshFilter filter = segmentObject.AddComponent<MeshFilter>();
            MeshRenderer renderer = segmentObject.AddComponent<MeshRenderer>();

            // Update rendering values
            filter.mesh = segmentMesh;
            renderer.material = Items[i].Material;
        }
    }

    // Update view
    public void Update()
    {
        UpdateInput();
        UpdateSegments();
        UpdateAlpha();
    }

    private void UpdateInput()
    {
        if (IsVisible && Input.GetKeyDown(KeyCode.Mouse0))
        {
            IsVisible = false;
            IsExited = true;
        }
        else if (!IsExited)
        {
            IsVisible = Input.GetKey(MenuVisibilityToggle);
        }
        else if (!Input.GetKey(MenuVisibilityToggle))
        {
            IsExited = false;
        }

        if (IsVisible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Temporary
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void UpdateAlpha()
    {
        // Update alpha value
        Alpha += Time.deltaTime * AlphaMultiplier * (IsVisible ? 1.0f : -1.0f);
        Alpha = Mathf.Clamp(Alpha, 0.0f, 0.5f);

        // Update material
        foreach (Item item in Items)
        {
            Color color = item.Material.color;
            item.Material.color = new Color(color.r, color.g, color.b, Alpha);
        }
    }

    private void UpdateSegments()
    {
        int currentNum = -1;

        if (IsVisible)
        {
            // Get the angle of the mouse from the center
            Vector2 centeredMousePosition = Input.mousePosition - new Vector3(Screen.width, Screen.height, 0.0f) * 0.5f;
            float dist = centeredMousePosition.magnitude;

            // Only detect for segments if out of the inner radius
            if (dist > InnerRadius * 5.0f)
            {
                // Retrieve angle and convert to 0-360
                float angle = Mathf.Atan2(centeredMousePosition.y, centeredMousePosition.x) * Mathf.Rad2Deg;
                angle = -angle + (360.0f / Items.Count) * 0.5f;
                angle = (angle + 360.0f) % 360.0f;

                // Divide by the total number of items to retrieve the current one
                currentNum = Mathf.Clamp(Mathf.FloorToInt((angle / (360.0f / Items.Count))), 0, Items.Count - 1);

                // Get the angle of the segment
                float segmentAngle = -Segments[currentNum].transform.localEulerAngles.y * Mathf.Deg2Rad;

                // Target is pushed out from the center
                Vector3 target = new Vector3(Mathf.Cos(segmentAngle), 0.0f, Mathf.Sin(segmentAngle)) * SelectDistance;
                Vector3 current = Segments[currentNum].transform.localPosition;

                // Move position towards the target
                Segments[currentNum].transform.localPosition += (target - current) * Time.deltaTime * 10.0f;

                // Update the selected material
                SelectedSegment = Items[currentNum].Name;
            }
            else
            {
                // Cancel selected
                SelectedSegment = "";
            }
        }


        // Reset scales
        for (int i = 0; i < Items.Count; ++i)
        {
            if (i != currentNum)
            {
                // Bring back to the center
                Segments[i].transform.localPosition = Segments[i].transform.localPosition * (1.0f - Time.deltaTime * 10.0f);
            }
        }
    }

    // Update is called once per frame
    private Mesh GenerateSlice(float angle, float innerRadius, float outerRadius)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        // The size of an angle of each triangle
        float splitAngle = Mathf.Ceil(angle) / angle;

        for (int i = 0; i <= Mathf.Ceil(angle); i++)
        {
            // Calculate the angle of the current ray
            float current = (i * splitAngle) - Mathf.Ceil(angle) * 0.5f;
            current *= Mathf.Deg2Rad;

            // Create a unit vector based on the angle
            float x = Mathf.Cos(current);
            float z = Mathf.Sin(current);

            // Update the direction in the array
            vertices.Add(new Vector3(x, 0.0f, z) * innerRadius);
            vertices.Add(new Vector3(x, 0.0f, z) * outerRadius);
        }

        for (int i = 0; i < vertices.Count - 2; i += 2)
        {
            // Create a triangle between the starting point and the two end vertices
            indices.Add(i);
            indices.Add(i + 2);
            indices.Add(i + 1);

            indices.Add(i + 1);
            indices.Add(i + 2);
            indices.Add(i + 3);
        }

        // Assign the triangles
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();

        return mesh;
    }
}
