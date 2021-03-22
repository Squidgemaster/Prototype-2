using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ERayMode
{
    PlaceFromMouse,
    PlaceFromCenter,
}

public class GridPlacer : MonoBehaviour
{
    [Header("Script")]
    public bool CanBuild = true;
    public string CurrentObject = "";
    public RadialMenu RadialParent;

    [Header("Properties")]
    public float BuildRange = 5.0f;
    public ERayMode RayMode;

    [Header("Visuals")]
    public Material PlaceholderMaterial;

    // Don't touch
    private Camera MainCamera;
    private Grid3D[] Grids;

    // Placeholder data
    private Transform PlaceholderObject;
    private string PlaceholderName;
    private GridObjectData PlaceholderData;
    private EDirection PlaceholderDirection;

    // Start is called before the first frame update
    void Start()
    {
        // Find all the grids in the level
        Grids = FindObjectsOfType<Grid3D>();

        // Find the camera in the level
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (MainCamera == null) { Debug.LogError("Grid Error: Camera must have 'MainCamera' tag"); }
    }

    // Update is called once per frame
    void Update()
    {
        // Retrieve the building from the radial parent
        CurrentObject = RadialParent.SelectedSegment;
        CanBuild = !RadialParent.IsVisible;

        if (CanBuild && CurrentObject != "")
        {
            CheckForRotation();
            CheckForPlace();
            CheckForDestroy();
        }
    }

    private void CheckForRotation()
    {
        if (PlaceholderObject != null && Input.GetKeyDown(KeyCode.R))
        {
            // Loop through directions
            PlaceholderDirection = (EDirection)(((int)PlaceholderDirection + 1) % 4);

            // Update placeholder object
            PlaceholderObject.rotation = Quaternion.AngleAxis((int)PlaceholderDirection * 90, Vector3.up);
        }
    }

    private void CheckForPlace()
    {
        // Only try place an object if an object is selected
        if (CurrentObject != "")
        {
            Vector3Int tileLoc = new Vector3Int();
            bool foundValidLocation = false;

            // Loop through each grid
            foreach (Grid3D grid in Grids)
            {
                // Check if the mouse is hitting a tile object
                if (grid.GetMousePlaceTileLocation(PlaceholderData, PlaceholderDirection, MainCamera, GetRayLocation(), BuildRange, out tileLoc))
                {
                    foundValidLocation = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        // Place the selected object
                        grid.CreateGridObject(CurrentObject, tileLoc, PlaceholderDirection);
                        break;
                    }
                    else
                    {
                        UpdatePlaceholder(grid, tileLoc);
                    }

                    break;
                }
            }

            // If no location is found, hide the placeholder object
            if (!foundValidLocation && PlaceholderObject != null)
            {
                PlaceholderObject.gameObject.SetActive(false);
            }
        }
        // Stopped placing something
        else if (PlaceholderObject != null)
        {
            Destroy(PlaceholderObject);
        }
    }

    private void CheckForDestroy()
    {
        // Right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            Vector3Int tileLoc = new Vector3Int();

            // Loop through each grid
            foreach (Grid3D grid in Grids)
            {
                // Check if the mouse is hitting a tile object
                if (grid.GetMouseDestroyTileLocation(MainCamera, GetRayLocation(), BuildRange, out tileLoc))
                {
                    // Destroy the target object
                    grid.DestroyGridObject(tileLoc);
                }
            }
        }
    }

    private void UpdatePlaceholder(Grid3D grid, Vector3Int tileLocation)
    {
        // Update the placeholder if the object is changed
        if (PlaceholderName != CurrentObject && PlaceholderObject != null)
        {
            Destroy(PlaceholderObject.gameObject);
            PlaceholderDirection = EDirection.Right;
        }

        // Create a new placeholder object if it's null
        if (PlaceholderObject == null)
        {
            PlaceholderName = CurrentObject;
            PlaceholderData = Grid3D.GetObjectData(CurrentObject).Value;

            // Generate a copy of the object
            PlaceholderObject = GameObject.Instantiate(Grid3D.GetObjectData(CurrentObject).Key);
            PlaceholderObject.name = CurrentObject + " (Placeholder)";
            PlaceholderObject.parent = transform;

            // Make transparent
            UpdateAllMaterials(PlaceholderObject.gameObject, PlaceholderMaterial);
            CullUnnecessaryComponents(PlaceholderObject.gameObject);
        }

        bool isValid = grid.IsObjectValid(PlaceholderData, tileLocation, PlaceholderDirection);
        Color col = isValid ? Color.green : Color.red;
        PlaceholderMaterial.color = new Color(col.r, col.g, col.b, 0.5f);

        // Update location
        Vector3 worldLocation = new Vector3();
        grid.TileToWorld(tileLocation, out worldLocation);
        PlaceholderObject.transform.position = worldLocation;

        // Update rotation (TODO:)

        // Update visual
        PlaceholderObject.gameObject.SetActive(true);
    }

    private Vector2 GetRayLocation()
    {
        switch (RayMode)
        {
            case ERayMode.PlaceFromCenter:
                return new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

            case ERayMode.PlaceFromMouse:
                return Input.mousePosition;

            default:
                return Vector2.zero;
        }

    }

    private void UpdateAllMaterials(GameObject parent, Material material)
    {
        // Get all the renderer components in children
        Renderer[] children = parent.GetComponentsInChildren<Renderer>();

        // Loop through them
        foreach (Renderer rend in children)
        {
            // Update all the materials to the new one
            rend.materials = Enumerable.Repeat(material, rend.materials.Length).ToArray();
        }
    }

    private void CullUnnecessaryComponents(GameObject parent)
    {
        // Get EVERY component in the parent object
        Component[] components = parent.GetComponentsInChildren<Component>();

        foreach (Component component in components)
        {
            // Keep the mesh renderer and mesh filter components only
            if (!ReferenceEquals(component.GetType(), typeof(MeshRenderer)) &&
                !ReferenceEquals(component.GetType(), typeof(MeshFilter)) &&
                !ReferenceEquals(component.GetType(), typeof(Transform)))
            {
                Destroy(component);
            }
        }
    }
}
