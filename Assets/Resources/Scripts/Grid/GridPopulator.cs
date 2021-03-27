using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPopulator : MonoBehaviour
{
    private Grid3D grid;
    private RadialMenu colourMenu;
    private GameObject tempObject;

    // Start is called before the first frame update
    void Awake()
    {
        grid = GetComponent<Grid3D>();
        colourMenu = GameObject.Find("Radial Menu - Colours").GetComponent<RadialMenu>();
        tempObject = null;

        grid.OnGridInitialised += Grid_OnGridInitialised;
    }

    private void Grid_OnGridInitialised(object sender, System.EventArgs e)
    {
        // Paste generated code here:
        // --------------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------------
        PlaceObject("Mortar", 1, 0, 0, EDirection.Right, "Light Blue");
        PlaceObject("Pressure Plate", 1, 0, 2, EDirection.Right, "Light Blue");
        PlaceObject("Piston", 1, 0, 3, EDirection.Left, "Light Blue");
        PlaceObject("Blower", 5, 0, 3, EDirection.Right, "Light Blue");
        PlaceObject("Block", 0, 0, 4, EDirection.Left, "");
        PlaceObject("Block", 1, 0, 4, EDirection.Left, "");
        PlaceObject("Block", 2, 0, 4, EDirection.Down, "");
        PlaceObject("Block", 3, 0, 4, EDirection.Up, "");
        PlaceObject("Block", 4, 0, 4, EDirection.Left, "");
        PlaceObject("Pressure Plate", 5, 0, 4, EDirection.Right, "Light Blue");
        PlaceObject("Block", 6, 0, 4, EDirection.Down, "");
        PlaceObject("Pressure Plate", 0, 0, 5, EDirection.Left, "Green");
        PlaceObject("Block", 1, 0, 5, EDirection.Left, "");
        PlaceObject("Block", 2, 0, 5, EDirection.Left, "");
        PlaceObject("Blower", 3, 0, 5, EDirection.Down, "Light Blue");
        PlaceObject("Ramp", 4, 0, 5, EDirection.Left, "");
        PlaceObject("Pressure Plate", 5, 0, 5, EDirection.Up, "Green");
        PlaceObject("Blower", 6, 0, 5, EDirection.Up, "Green");
        PlaceObject("Block", 0, 0, 6, EDirection.Right, "");
        PlaceObject("Block", 2, 0, 6, EDirection.Left, "");
        PlaceObject("Block", 3, 0, 6, EDirection.Left, "");
        PlaceObject("Block", 4, 0, 6, EDirection.Left, "");
        PlaceObject("Block", 5, 0, 6, EDirection.Left, "");
        PlaceObject("Block", 0, 0, 7, EDirection.Right, "");
        PlaceObject("Blower", 0, 1, 4, EDirection.Down, "Red");
        PlaceObject("Pressure Plate", 1, 1, 4, EDirection.Right, "Red");
        PlaceObject("Ramp", 2, 1, 4, EDirection.Right, "");
        PlaceObject("Blower", 3, 1, 4, EDirection.Up, "Light Blue");
        PlaceObject("Ramp", 4, 1, 4, EDirection.Left, "");
        PlaceObject("Block", 6, 1, 4, EDirection.Down, "");
        PlaceObject("Block", 1, 1, 5, EDirection.Left, "");
        PlaceObject("Block", 5, 1, 5, EDirection.Up, "");
        PlaceObject("Block", 6, 1, 5, EDirection.Up, "");
        PlaceObject("Boulder", 0, 1, 6, EDirection.Right, "Green");
        PlaceObject("Mortar", 2, 1, 6, EDirection.Left, "Green");
        PlaceObject("Piston", 0, 1, 7, EDirection.Left, "Green");
        PlaceObject("Block", 3, 2, 3, EDirection.Up, "");
        PlaceObject("Block", 4, 2, 3, EDirection.Up, "");
        PlaceObject("Block", 5, 2, 3, EDirection.Down, "");
        PlaceObject("Block", 6, 2, 3, EDirection.Down, "");
        PlaceObject("Block", 1, 2, 4, EDirection.Right, "");
        PlaceObject("Block", 6, 2, 4, EDirection.Down, "");
        PlaceObject("Block", 1, 2, 5, EDirection.Left, "");
        PlaceObject("Block", 5, 2, 5, EDirection.Down, "");
        PlaceObject("Block", 6, 2, 5, EDirection.Down, "");
        PlaceObject("Block", 4, 3, 3, EDirection.Up, "");
        PlaceObject("Block", 5, 3, 3, EDirection.Down, "");
        PlaceObject("Block", 6, 3, 3, EDirection.Down, "");
        PlaceObject("Boulder", 1, 3, 4, EDirection.Right, "Light Blue");
        PlaceObject("Block", 6, 3, 4, EDirection.Down, "");
        PlaceObject("Blower", 1, 3, 5, EDirection.Left, "Light Blue");
        PlaceObject("Block", 4, 3, 5, EDirection.Up, "");
        PlaceObject("Block", 5, 3, 5, EDirection.Down, "");
        PlaceObject("Block", 6, 3, 5, EDirection.Down, "");
        PlaceObject("Block", 4, 4, 3, EDirection.Up, "");
        PlaceObject("Block", 5, 4, 3, EDirection.Up, "");
        PlaceObject("Block", 6, 4, 3, EDirection.Up, "");
        PlaceObject("Block", 6, 4, 4, EDirection.Up, "");
        PlaceObject("Block", 4, 4, 5, EDirection.Up, "");
        PlaceObject("Block", 5, 4, 5, EDirection.Up, "");
        PlaceObject("Block", 6, 4, 5, EDirection.Up, "");


    }

    private void PlaceObject(string name, int x, int y, int z, EDirection direction, string colour)
    {
        grid.CreateGridObject(name, new Vector3Int(x, y, z), direction, out tempObject);

        if (colour != "")
        {
            tempObject.GetComponentInChildren<IGridObject>().Colour = colour;
            GridPlacer.UpdateAllMaterials(tempObject, "Colour (Instance)", ColourEventManager.ColourMaterials[colour]);
        }
    }
}
