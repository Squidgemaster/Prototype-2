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
        PlaceObject("Pressure Plate", 1, 0, 1, EDirection.Right, "Purple");
        PlaceObject("Pressure Plate", 6, 0, 1, EDirection.Right, "Purple");
        PlaceObject("Pressure Plate", 7, 0, 1, EDirection.Right, "");
        PlaceObject("Boulder", 10, 0, 1, EDirection.Right, "Light Blue");
        PlaceObject("Pressure Plate", 11, 0, 1, EDirection.Right, "");
        PlaceObject("Mortar", 13, 0, 1, EDirection.Right, "Green");
        PlaceObject("Mortar", 14, 0, 1, EDirection.Right, "Red");
        PlaceObject("Mortar", 16, 0, 1, EDirection.Right, "");
        PlaceObject("Pressure Plate", 1, 0, 2, EDirection.Right, "Purple");
        PlaceObject("Mortar", 2, 0, 2, EDirection.Right, "Purple");
        PlaceObject("Mortar", 3, 0, 2, EDirection.Right, "Red");
        PlaceObject("Blower", 4, 0, 2, EDirection.Left, "Purple");
        PlaceObject("Blower", 5, 0, 2, EDirection.Down, "Purple");
        PlaceObject("Blower", 6, 0, 2, EDirection.Right, "");
        PlaceObject("Boulder", 7, 0, 2, EDirection.Right, "Light Blue");
        PlaceObject("Boulder", 8, 0, 2, EDirection.Right, "Light Blue");
        PlaceObject("Pressure Plate", 11, 0, 2, EDirection.Right, "");
        PlaceObject("Mortar", 15, 0, 2, EDirection.Right, "");
        PlaceObject("Mortar", 2, 0, 3, EDirection.Right, "Purple");
        PlaceObject("Blower", 3, 0, 3, EDirection.Left, "Purple");
        PlaceObject("Mortar", 6, 0, 3, EDirection.Right, "Red");
        PlaceObject("Boulder", 15, 0, 3, EDirection.Right, "Light Blue");
        PlaceObject("Mortar", 16, 0, 3, EDirection.Right, "");
        PlaceObject("Blower", 0, 0, 4, EDirection.Right, "Purple");
        PlaceObject("Blower", 4, 0, 4, EDirection.Right, "Purple");
        PlaceObject("Mortar", 5, 0, 4, EDirection.Right, "Red");
        PlaceObject("Boulder", 6, 0, 4, EDirection.Right, "Light Blue");
        PlaceObject("Pressure Plate", 7, 0, 4, EDirection.Right, "");
        PlaceObject("Ramp", 8, 0, 4, EDirection.Right, "");
        PlaceObject("Piston", 10, 0, 4, EDirection.Right, "");
        PlaceObject("Piston", 13, 0, 4, EDirection.Right, "");
        PlaceObject("Mortar", 16, 0, 4, EDirection.Right, "");
        PlaceObject("Blower", 1, 0, 5, EDirection.Down, "Purple");
        PlaceObject("Blower", 3, 0, 5, EDirection.Right, "");
        PlaceObject("Boulder", 6, 0, 5, EDirection.Right, "Light Blue");
        PlaceObject("Mortar", 11, 0, 5, EDirection.Right, "");
        PlaceObject("Mortar", 13, 0, 5, EDirection.Right, "");
        PlaceObject("Block", 15, 0, 5, EDirection.Right, "");
        PlaceObject("Boulder", 18, 0, 5, EDirection.Right, "");
        PlaceObject("Blower", 0, 0, 6, EDirection.Right, "");
        PlaceObject("Blower", 1, 0, 6, EDirection.Down, "Purple");
        PlaceObject("Mortar", 3, 0, 6, EDirection.Right, "Purple");
        PlaceObject("Piston", 6, 0, 6, EDirection.Right, "");
        PlaceObject("Mortar", 8, 0, 6, EDirection.Right, "");
        PlaceObject("Mortar", 9, 0, 6, EDirection.Right, "");
        PlaceObject("Mortar", 11, 0, 6, EDirection.Right, "");
        PlaceObject("Mortar", 12, 0, 6, EDirection.Right, "");
        PlaceObject("Mortar", 13, 0, 6, EDirection.Right, "");
        PlaceObject("Block", 15, 0, 6, EDirection.Right, "");
        PlaceObject("Block", 16, 0, 6, EDirection.Right, "");
        PlaceObject("Pressure Plate", 0, 0, 7, EDirection.Right, "Purple");
        PlaceObject("Pressure Plate", 2, 0, 7, EDirection.Right, "Purple");
        PlaceObject("Mortar", 5, 0, 7, EDirection.Right, "Purple");
        PlaceObject("Mortar", 7, 0, 7, EDirection.Right, "Red");
        PlaceObject("Mortar", 8, 0, 7, EDirection.Right, "Red");
        PlaceObject("Ramp", 9, 0, 7, EDirection.Right, "");
        PlaceObject("Piston", 11, 0, 7, EDirection.Right, "");
        PlaceObject("Mortar", 13, 0, 7, EDirection.Right, "");
        PlaceObject("Block", 14, 0, 7, EDirection.Right, "");
        PlaceObject("Block", 15, 0, 7, EDirection.Right, "");
        PlaceObject("Block", 17, 0, 7, EDirection.Right, "");
        PlaceObject("Blower", 1, 0, 8, EDirection.Left, "Purple");
        PlaceObject("Mortar", 4, 0, 8, EDirection.Right, "");
        PlaceObject("Mortar", 5, 0, 8, EDirection.Right, "Purple");
        PlaceObject("Mortar", 6, 0, 8, EDirection.Right, "");
        PlaceObject("Mortar", 9, 0, 8, EDirection.Right, "");
        PlaceObject("Ramp", 10, 0, 8, EDirection.Right, "");
        PlaceObject("Block", 12, 0, 8, EDirection.Right, "");
        PlaceObject("Boulder", 13, 0, 8, EDirection.Right, "");
        PlaceObject("Boulder", 15, 0, 8, EDirection.Right, "Light Blue");
        PlaceObject("Boulder", 18, 0, 8, EDirection.Right, "");
        PlaceObject("Blower", 0, 0, 9, EDirection.Up, "Purple");
        PlaceObject("Pressure Plate", 1, 0, 9, EDirection.Right, "Purple");
        PlaceObject("Blower", 3, 0, 9, EDirection.Right, "Purple");
        PlaceObject("Blower", 5, 0, 9, EDirection.Right, "Purple");
        PlaceObject("Piston", 6, 0, 9, EDirection.Right, "");
        PlaceObject("Blower", 8, 0, 9, EDirection.Right, "Purple");
        PlaceObject("Mortar", 10, 0, 9, EDirection.Right, "Green");
        PlaceObject("Mortar", 11, 0, 9, EDirection.Right, "Red");
        PlaceObject("Block", 12, 0, 9, EDirection.Right, "");
        PlaceObject("Mortar", 9, 0, 10, EDirection.Right, "Red");
        PlaceObject("Ramp", 8, 1, 4, EDirection.Right, "");
        PlaceObject("Block", 14, 1, 5, EDirection.Right, "");
        PlaceObject("Block", 15, 1, 5, EDirection.Right, "");
        PlaceObject("Block", 14, 1, 6, EDirection.Right, "");
        PlaceObject("Block", 15, 1, 6, EDirection.Right, "");
        PlaceObject("Mortar", 9, 1, 7, EDirection.Right, "");
        PlaceObject("Block", 14, 1, 7, EDirection.Right, "");
        PlaceObject("Boulder", 15, 1, 7, EDirection.Right, "Light Blue");
        PlaceObject("Mortar", 10, 1, 8, EDirection.Right, "Green");
        PlaceObject("Mortar", 12, 1, 9, EDirection.Right, "Green");
        PlaceObject("Block", 14, 2, 5, EDirection.Right, "");
        PlaceObject("Block", 14, 2, 6, EDirection.Right, "");
        PlaceObject("Boulder", 14, 2, 7, EDirection.Right, "Light Blue");
        PlaceObject("Piston", 14, 3, 5, EDirection.Right, "");
        PlaceObject("Mortar", 14, 3, 6, EDirection.Right, "Red");

        // --------------------------------------------------------------------------------------

        colourMenu.SelectedSegment = "";
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
