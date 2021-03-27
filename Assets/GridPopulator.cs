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
