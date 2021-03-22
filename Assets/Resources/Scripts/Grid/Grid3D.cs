using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection
{
    Right,
    Down,
    Left,
    Up
}

public struct GridObjectData
{
    // Need rotation data to access
    private bool[,,] SolidTiles;
    private Vector3Int SolidTilesOffset;

    public bool MustBeSupported;
    public bool CanBeBuiltOn;

    public GridObjectData(bool[,,] solidTable, Vector3Int solidOffset, bool mustBeSupported, bool canBeBuiltOn)
    {
        SolidTiles = solidTable;
        SolidTilesOffset = solidOffset;

        MustBeSupported = mustBeSupported;
        CanBeBuiltOn = canBeBuiltOn;

        VerifySolidArray();
    }

    // Make sure there is no empty space outside of the solid tiles
    public void VerifySolidArray()
    {
        // Get the tile dimensions of the object
        int width = SolidTiles.GetLength(0);
        int depth = SolidTiles.GetLength(1);
        int length = SolidTiles.GetLength(2);

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int minZ = int.MaxValue;

        int maxX = int.MinValue;
        int maxY = int.MinValue;
        int maxZ = int.MinValue;

        for (int y = 0; y < depth; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int z = 0; z < length; ++z)
                {
                    // Check if the tile is solid
                    if (SolidTiles[x, y, z])
                    {
                        // Update mins and maxs
                        minX = Mathf.Min(minX, x);
                        minY = Mathf.Min(minY, y);
                        minZ = Mathf.Min(minZ, z);

                        maxX = Mathf.Max(maxX, x);
                        maxY = Mathf.Max(maxY, y);
                        maxZ = Mathf.Max(maxZ, z);
                    }
                }
            }
        }

        // Check if there are any borders empty
        if (minX > 0 || minY > 0 || minZ > 0 ||
            maxX < width - 1 || maxY < depth - 1 || maxZ < length - 1)
        {
            Debug.LogError("Grid Error: Grid object has empty space around the outside of the solid area");
        }
    }

    // Get the new tiles offset based on the rotation
    public Vector3Int GetTilesOffset(EDirection direction = EDirection.Right)
    {
        int x = -SolidTilesOffset.x;
        int y = -SolidTilesOffset.y;
        int z = -SolidTilesOffset.z;

        int w = SolidTiles.GetLength(0);
        int l = SolidTiles.GetLength(2);

        switch (direction)
        {
            case EDirection.Right:
                return -new Vector3Int(x, y, z);

            case EDirection.Down:
                return -new Vector3Int(z, y, w - x - 1);

            case EDirection.Left:
                return -new Vector3Int(w - x - 1, y, l - z - 1);

            case EDirection.Up:
                return -new Vector3Int(l - z - 1, y, x);

            default:
                return Vector3Int.zero;
        }
    }

    // Gets the solid value of a tile based on rotation
    public bool GetIsSolid(int x, int y, int z, EDirection direction = EDirection.Right)
    {
        Vector3Int loc = GetRotatedLocation(new Vector3Int(x, y, z), direction);
        return SolidTiles[loc.x, loc.y, loc.z];
    }

    // Get the dimensions of the solid tiles value but modified with rotation in mind
    public Vector3Int GetDimensions(EDirection direction = EDirection.Right)
    {
        switch (direction)
        {
            case EDirection.Left:
            case EDirection.Right:
                return new Vector3Int(SolidTiles.GetLength(0), SolidTiles.GetLength(1), SolidTiles.GetLength(2));

            case EDirection.Down:
            case EDirection.Up:
                return new Vector3Int(SolidTiles.GetLength(2), SolidTiles.GetLength(1), SolidTiles.GetLength(0));

            default:
                return Vector3Int.zero;
        }
    }

    // Converts a normal location to the new rotated location (Based on ObjectData.SolidTiles)
    private Vector3Int GetRotatedLocation(Vector3Int location, EDirection direction)
    {
        int x = location.x;
        int y = location.y;
        int z = location.z;

        int w = SolidTiles.GetLength(0);
        int l = SolidTiles.GetLength(2);

        switch (direction)
        {
            case EDirection.Right:
                return new Vector3Int(x, y, z);

            case EDirection.Down:
                return new Vector3Int(w - z - 1, y, x);

            case EDirection.Left:
                return new Vector3Int(w - x - 1, y, l - z - 1);

            case EDirection.Up:
                return new Vector3Int(z, y, l - x - 1);

            default:
                return Vector3Int.zero;
        }
    }

}

public class GridObject
{
    public Transform Object;
    public GridObjectData ObjectData;
    public Vector3Int ObjectGridLocation;
    public EDirection ObjectDirection;

    public GridObject(Transform gameObject, GridObjectData data, Vector3Int gridLocation, EDirection direction)
    {
        ObjectData = data;
        Object = gameObject;
        ObjectGridLocation = gridLocation;
        ObjectDirection = direction;
    }
}

public class GridTile
{
    Vector3 GridLocation;
    GridObject Object;
}

public class Grid3D : MonoBehaviour
{
    private static readonly string PrefabPath = "Prefabs/Grid Objects/";
    private static Dictionary<string, KeyValuePair<Transform, GridObjectData>> GridObjectTypes;

    [Header("Debug")]
    public bool DisplaySolidTiles;
    
    [Header("Dimensions")]
    public int Width;
    public int Length;
    public int Depth;

    [Space(10)]
    public float TileSize;

    // Utility events
    public event EventHandler OnTileAdded;
    public event EventHandler OnTileRemoved;

    // The world space dimensions
    private Vector3 WorldDimensions;
    private Vector3 TileDimensions;
    private Vector3 OriginOffset;

    // Grid data
    private GridObject[,,] Tiles;
    private BoxCollider[,,] Colliders;
    private GameObject ColliderObject;
    private int CollisionLayer;

    // Start is called before the first frame update
    private void Start()
    {
        // Populate grid objects once regardless of number of instances
        if (GridObjectTypes == null) { PopulateGridObjectTypes(); }

        UpdateTileDimensions();

        // Initialise the grids
        Tiles = new GridObject[Width, Depth, Length];
        Colliders = new BoxCollider[Width, Depth, Length];

        // Find the grid collision layer (used to limit raycasting)
        CollisionLayer = LayerMask.GetMask("Grid Collider");

        // Make the collider object a child of this
        ColliderObject = new GameObject();
        ColliderObject.name = "Colliders";
        ColliderObject.transform.parent = transform;
        ColliderObject.layer = LayerMask.NameToLayer("Grid Collider");

        // Add a base box collider to the bottom of the grid
        BoxCollider baseCollider = ColliderObject.AddComponent<BoxCollider>();
        baseCollider.size = new Vector3(WorldDimensions.x, 0.1f, WorldDimensions.z);
        baseCollider.center = new Vector3(transform.position.x, transform.position.y - baseCollider.size.y * 0.5f, transform.position.z);
    }

    // Returns true if the world position is within the grid bounds
    public bool WorldToTile(Vector3 worldLoc, out Vector3Int tileLoc)
    {
        // Find the location in regards to the origin location
        Vector3 relativeLocation = worldLoc - OriginOffset;

        tileLoc = new Vector3Int();

        // Move to tile locations
        tileLoc.x = Mathf.FloorToInt(relativeLocation.x / TileSize);
        tileLoc.y = Mathf.FloorToInt(relativeLocation.y / TileSize);
        tileLoc.z = Mathf.FloorToInt(relativeLocation.z / TileSize);

        // Only return true if the tile location is within the grid
        return IsTileLocationValid(tileLoc);
    }

    // Returns true if the tile location is within the grid bounds
    public bool TileToWorld(Vector3Int tileLoc, out Vector3 worldLoc)
    {
        // Check if the tile location is within the boundries
        if (IsTileLocationValid(tileLoc))
        {
            worldLoc = new Vector3(tileLoc.x, tileLoc.y, tileLoc.z) * TileSize;
            worldLoc += OriginOffset;
            worldLoc += TileDimensions * 0.5f;

            return true;
        }
        else
        {
            worldLoc = new Vector3();
            return false;
        }
    }

    // Create a grid object and add it to the tileset
    public bool CreateGridObject(string objectName, Vector3Int tileLocation, EDirection direction)
    {
        // Get object data
        KeyValuePair<Transform, GridObjectData> data = GridObjectTypes[objectName];
        Transform prefab = data.Key;
        GridObjectData objectData = data.Value;

        // Check if the object fits within the groud boundries
        if (IsObjectValid(objectData, tileLocation, direction))
        {
            // Get the tile dimensions of the object
            Vector3Int dimensions = objectData.GetDimensions(direction);
            int width = dimensions.x;
            int depth = dimensions.y;
            int length = dimensions.z;

            // The offset to the origin of the object in tiles space
            Vector3Int globalOffset = tileLocation + objectData.GetTilesOffset(direction);

            // Calculate the world location of the new grid object
            Vector3 worldLocation = new Vector3();
            TileToWorld(tileLocation, out worldLocation);


            // Create that object and add it to the scene
            Transform sceneObject = GameObject.Instantiate(prefab);
            sceneObject.name = objectName + ": " + tileLocation.ToString();
            sceneObject.position = worldLocation;
            sceneObject.parent = transform;
            sceneObject.rotation = Quaternion.AngleAxis((int)direction * 90, Vector3.up);

            // Initialise the grid object variable to be placed into the grid slots
            GridObject obj = new GridObject(sceneObject, objectData, tileLocation, direction);

            for (int y = 0; y < depth; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    for (int z = 0; z < length; ++z)
                    {
                        // Only overwrite if the object tile is solid
                        if (objectData.GetIsSolid(x, y, z, direction))
                        {
                            // Get the location of the tile on the grid space
                            Vector3Int globalTileLoc = new Vector3Int(x, y, z) + globalOffset;
                            Tiles[globalTileLoc.x, globalTileLoc.y, globalTileLoc.z] = obj;

                            // Get the location of that tile
                            Vector3 worldLoc = new Vector3();
                            TileToWorld(globalTileLoc, out worldLoc);

                            // Generate a collider at that location
                            BoxCollider collider = ColliderObject.AddComponent<BoxCollider>();
                            collider.center = worldLoc;
                            collider.size = Vector3.one * TileSize;

                            // Add to the collider objects
                            Colliders[globalTileLoc.x, globalTileLoc.y, globalTileLoc.z] = collider;
                        }
                    }
                }
            }

            // Invoke function
            OnTileAdded?.Invoke(this, EventArgs.Empty);

            // Finished placing object
            return true;
        }
        else
        {
            // Object was not in bounds of the tile grid
            return false;
        }
    }

    public bool DestroyGridObject(Vector3Int tileLocation)
    {
        // Make sure the location is within the grid bounds
        if (IsTileLocationValid(tileLocation))
        {
            GridObject gridObject = Tiles[tileLocation.x, tileLocation.y, tileLocation.z];

            if (gridObject != null)
            {
                // Get the tile dimensions of the object
                Vector3Int dimensions = gridObject.ObjectData.GetDimensions(gridObject.ObjectDirection);
                int width = dimensions.x;
                int depth = dimensions.y;
                int length = dimensions.z;

                Vector3Int globalOffset = gridObject.ObjectGridLocation + gridObject.ObjectData.GetTilesOffset(gridObject.ObjectDirection);

                for (int y = 0; y < depth; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        for (int z = 0; z < length; ++z)
                        {
                            // Only overwrite if the object tile is solid
                            if (gridObject.ObjectData.GetIsSolid(x, y, z, gridObject.ObjectDirection))
                            {
                                // Get the location of the tile on the grid space
                                Vector3Int globalTileLoc = new Vector3Int(x, y, z) + globalOffset;
                                Tiles[globalTileLoc.x, globalTileLoc.y, globalTileLoc.z] = null;

                                // Destroy the collider at that location
                                Destroy(Colliders[globalTileLoc.x, globalTileLoc.y, globalTileLoc.z]);
                                Colliders[globalTileLoc.x, globalTileLoc.y, globalTileLoc.z] = null;
                            }
                        }
                    }
                }

                // Removed all instances in tiles, destroy object from the scene
                Destroy(gridObject.Object.gameObject);

                // Invoke function
                OnTileRemoved?.Invoke(this, EventArgs.Empty);

                return true;
            }
        }

        // Failed somewhere, could not destroy object
        return false;
    }

    public bool IsObjectValid(GridObjectData objectData, Vector3Int tileLocation, EDirection direction)
    {
        // Get the tile dimensions of the object
        Vector3Int dimensions = objectData.GetDimensions(direction);
        int width = dimensions.x;
        int depth = dimensions.y;
        int length = dimensions.z;

        // The offset to the origin of the object in tiles space
        Vector3Int globalOffset = tileLocation + objectData.GetTilesOffset(direction);

        // Check if object is in range of the grid
        if (globalOffset.x < 0 ||
            globalOffset.y < 0 ||
            globalOffset.z < 0 ||
            globalOffset.x + width - 1 >= Width ||
            globalOffset.y + depth - 1 >= Depth ||
            globalOffset.z + length - 1 >= Length)
        {
            return false;
        }

        for (int y = 0; y < depth; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int z = 0; z < length; ++z)
                {
                    // Only do checks if the current tile is solid
                    if (objectData.GetIsSolid(x, y, z, direction))
                    {
                        // Get the location of the tile on the grid space
                        Vector3Int globalTileLoc = new Vector3Int(x, y, z) + globalOffset;

                        // If the tile is not free
                        if (Tiles[globalTileLoc.x, globalTileLoc.y, globalTileLoc.z] != null)
                        {
                            // Intersection found
                            return false;
                        }
                        // Make sure that if the object needs to be supported the tile has something to stand on
                        else if (objectData.MustBeSupported &&
                                 globalTileLoc.y > 0 && // The ground is solid so no need to check
                                 (y == 0 || !objectData.GetIsSolid(x, y - 1, z, direction)) && // Only check tiles with space underneath it
                                 (Tiles[globalTileLoc.x, globalTileLoc.y - 1, globalTileLoc.z] == null || // Check if the block under is null
                                 !Tiles[globalTileLoc.x, globalTileLoc.y - 1, globalTileLoc.z].ObjectData.CanBeBuiltOn)) // Block under is not null but is unstable
                        {
                            // No support found
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    // Gets the tile location of the to be placed tile
    public bool GetMousePlaceTileLocation(GridObjectData data, EDirection rotation, Camera camera, Vector2 screenLocation, float range, out Vector3Int tileLocation)
    {
        // Define raycast
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(screenLocation);

        // Raycast out 100 units on the grids collision layer
        if (Physics.Raycast(ray, out hit, range, CollisionLayer))
        {
            // The to be pushed out locations
            Vector3 newLoc;

            // We know that all colliders are box colliders
            Vector3 colliderLocation = ((BoxCollider)hit.collider).center;

            // Check if the collider is the floor collider
            if (colliderLocation.y > transform.position.y)
            {
                // Push the location away from the tile in the greatest direction
                Vector3 delta = hit.point - colliderLocation;
                Vector3 deltaAbs = new Vector3(Mathf.Abs(delta.x), Mathf.Abs(delta.y), Mathf.Abs(delta.z));
                Vector3 direction = new Vector3(
                    Convert.ToInt32(deltaAbs.x > deltaAbs.y && deltaAbs.x > deltaAbs.z) * Mathf.Sign(delta.x),
                    Convert.ToInt32(deltaAbs.y > deltaAbs.x && deltaAbs.y > deltaAbs.z) * Mathf.Sign(delta.y),
                    Convert.ToInt32(deltaAbs.z > deltaAbs.y && deltaAbs.z > deltaAbs.x) * Mathf.Sign(delta.z));

                // Push the tile far enough away that it fits
                Vector3Int dimensions = data.GetDimensions(rotation);
                Vector3Int offset = data.GetTilesOffset(rotation);
                Vector3Int diff = (direction.x + direction.y + direction.z > 0.0f ? -offset + Vector3Int.one : dimensions + offset);

                // Bring it all together xd
                newLoc = colliderLocation + Vector3.Scale(direction, new Vector3(diff.x, diff.y, diff.z)) * TileSize;
            }
            else
            {
                // Bring location out of the ground
                newLoc = new Vector3(hit.point.x, transform.position.y + TileSize * 0.5f, hit.point.z);
            }

            // Convert to tile coordinates and make sure it's within bounds
            if (WorldToTile(newLoc, out tileLocation))
            {
                // Tile location is already set in the above if statement ^
                return true;
            }
        }

        // Failed somewhere
        tileLocation = new Vector3Int();
        return false;
    }

    // Gets the location of the tile the mouse is pointing at
    public bool GetMouseDestroyTileLocation(Camera camera, Vector2 screenLocation, float range, out Vector3Int tileLocation)
    {
        // Define raycast
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(screenLocation);

        // Raycast out 100 units on the grids collision layer
        if (Physics.Raycast(ray, out hit, range, CollisionLayer))
        {
            // Make sure collider is not the floor object
            Vector3 colliderLocation = ((BoxCollider)hit.collider).center;
            if (colliderLocation.y > transform.position.y)
            {
                // Convert to tile location
                if (WorldToTile(colliderLocation, out tileLocation))
                {
                    // Tile location is already set in the above if statement ^
                    return true;
                }
            }
        }

        // Failed somewhere
        tileLocation = new Vector3Int();
        return false;
    }



    // Check if the tile location is within the tiles bound
    public bool IsTileLocationValid(Vector3Int tileLocation)
    {
        // True if all the checks are true
        return (tileLocation.x < Width &&
                tileLocation.y < Depth &&
                tileLocation.z < Length &&
                tileLocation.x >= 0 &&
                tileLocation.y >= 0 &&
                tileLocation.z >= 0);
    }

    // Updates the dimensions, offsets and tile locationss
    public void UpdateTileDimensions()
    {
        // Update world distances
        WorldDimensions.x = Width * TileSize;
        WorldDimensions.y = Depth * TileSize;
        WorldDimensions.z = Length * TileSize;

        OriginOffset = new Vector3(-WorldDimensions.x * 0.5f, 0.0f, -WorldDimensions.z * 0.5f);
        OriginOffset += transform.position;

        TileDimensions = new Vector3(TileSize, TileSize, TileSize);
    }

    // Draw out a wire frame of the grid
    private void OnDrawGizmos()
    {
        UpdateTileDimensions();

        Gizmos.color = Color.white;


        // Draw x axis lines
        for (int x = 0; x <= Width; ++x)
        {
            Vector3 a = new Vector3(x * TileSize, 0.0f, 0.0f) + OriginOffset;
            Vector3 b = new Vector3(x * TileSize, 0.0f, WorldDimensions.z) + OriginOffset;

            Gizmos.DrawLine(a, b);
        }

        // Draw z axis lines
        for (int z = 0; z <= Length; ++z)
        {
            Vector3 a = new Vector3(0.0f, 0.0f, z * TileSize) + OriginOffset;
            Vector3 b = new Vector3(WorldDimensions.x, 0.0f, z * TileSize) + OriginOffset;

            Gizmos.DrawLine(a, b);
        }

        Gizmos.DrawWireCube(transform.position + new Vector3(0.0f, WorldDimensions.y * 0.5f, 0.0f), WorldDimensions);

        // Only draw during runtime
        if (Tiles != null && DisplaySolidTiles)
        {
            // Set colour to transparent red
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);

            // Draw filled in blocks
            for (int y = 0; y < Depth; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    for (int z = 0; z < Length; ++z)
                    {
                        if (Tiles[x, y, z] != null)
                        {
                            Vector3 worldLoc;
                            TileToWorld(new Vector3Int(x, y, z), out worldLoc);

                            Gizmos.DrawCube(worldLoc, TileDimensions);
                        }
                    }
                }
            }
        }
    }

    // Generates all the objects for the grid object types
    public static void PopulateGridObjectTypes()
    {
        GridObjectTypes = new Dictionary<string, KeyValuePair<Transform, GridObjectData>>();

        // Block
        // -------------------------------------------------------------------------------------------------------
        bool[,,] blockSolidTable = new bool[1, 1, 1] { { { true } } };

        Vector3Int blockSolidOffset = new Vector3Int(0, 0, 0);
        GridObjectData blockData = new GridObjectData(blockSolidTable, blockSolidOffset, true, true);
        Transform blockPrefab = Resources.Load<Transform>(PrefabPath + "Block");

        GridObjectTypes.Add("Block", new KeyValuePair<Transform, GridObjectData>(blockPrefab, blockData));
        // -------------------------------------------------------------------------------------------------------

        // Ramp
        // -------------------------------------------------------------------------------------------------------
        bool[,,] rampSolidTable = new bool[1, 1, 1] { { { true } } };

        Vector3Int rampSolidOffset = new Vector3Int(0, 0, 0);
        GridObjectData rampData = new GridObjectData(rampSolidTable, rampSolidOffset, true, true);
        Transform rampPrefab = Resources.Load<Transform>(PrefabPath + "Ramp");

        GridObjectTypes.Add("Ramp", new KeyValuePair<Transform, GridObjectData>(rampPrefab, rampData));
        // -------------------------------------------------------------------------------------------------------

        // Pressure Plate
        // -------------------------------------------------------------------------------------------------------
        bool[,,] pressurePlateSolidTable = new bool[1, 1, 1] { { { true } } };
        Vector3Int pressurePlateSolidOffset = new Vector3Int(0, 0, 0);

        GridObjectData pressurePlateData = new GridObjectData(pressurePlateSolidTable, pressurePlateSolidOffset, true, true);
        Transform pressurePlatePrefab = Resources.Load<Transform>(PrefabPath + "PressurePlate");

        GridObjectTypes.Add("Pressure Plate", new KeyValuePair<Transform, GridObjectData>(pressurePlatePrefab, pressurePlateData));
        // -------------------------------------------------------------------------------------------------------

        // Blower
        // -------------------------------------------------------------------------------------------------------
        bool[,,] blowerSolidTable = new bool[1, 1, 1] { { { true } } };
        Vector3Int blowerSolidOffset = new Vector3Int(0, 0, 0);

        GridObjectData blowerData = new GridObjectData(blowerSolidTable, blowerSolidOffset, true, true);
        Transform blowerPrefab = Resources.Load<Transform>(PrefabPath + "Blower");

        GridObjectTypes.Add("Blower", new KeyValuePair<Transform, GridObjectData>(blowerPrefab, blowerData));
        // -------------------------------------------------------------------------------------------------------

    }

    // Get the data of an object from name
    public static KeyValuePair<Transform, GridObjectData> GetObjectData(string name)
    {
        if (!GridObjectTypes.ContainsKey(name)) { Debug.LogError("Grid object '" + name + "' does not exist"); }
        return GridObjectTypes[name];
    }

    private static void FillSolidTable(ref bool[,,] table, bool value)
    {
        int w = table.GetLength(0);
        int d = table.GetLength(1);
        int l = table.GetLength(2);

        for (int y = 0; y < d; ++y)
        {
            for (int z = 0; z < l; ++z)
            {
                for (int x = 0; x < w; ++x)
                {
                    table[x, y, z] = value;
                }
            }
        }

    }
}
