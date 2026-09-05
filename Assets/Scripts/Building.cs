using UnityEngine;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    public BuildingController buildingController;
    public TileController tileController;
    public Tile coreTile; //Bottom left tile of building, x and z count from this tile to the right and up so (0,0)
    public int lengthX; // X direction size of building always x > 0, x = 2 means its two tiles wide, 1 to the right of (0,0)
    public int lengthZ; // See X but for z value upwards
    public List<Tile> containingTiles = new List<Tile>();
    public List<Tile> surroundingTiles = new List<Tile>();
    public float interactionDistance = 1.0f;
    void Start()
    {

    }

    void Update()
    {

    }

    public void Initialize(int xSize, int zSize)
    {
        lengthX = xSize;
        lengthZ = zSize;
        SetContainingTiles();
        SetSurroundingTiles();
    }

    private void SetContainingTiles()
    {
        for (int x = coreTile.x; x < coreTile.x + lengthX; x++)
        {
            for (int z = coreTile.z; z < coreTile.z + lengthZ; z++)
            {
                containingTiles.Add(tileController.GetTile(x, z));
            }
        }
    }

    private void SetSurroundingTiles()
    {
        int[,] offsets = { { -1, -1 }, { 0, -1 }, { +1, -1 }, { +1, 0 }, { +1, +1 }, { 0, +1 }, { -1, +1 }, { -1, 0 } };
        foreach (Tile tile in containingTiles)
        {
            for (int i = 0; i < 8; i++)
            {
                Tile adjacentTile = tileController.GetTile(tile.x + offsets[i, 0], tile.z + offsets[i, 1]);
                if (adjacentTile != null && !surroundingTiles.Contains(adjacentTile))
                {
                    surroundingTiles.Add(adjacentTile);
                }
            }
        }
    }

    public Tile getClosestSurroundingTile(Vector3 unitPosition)
    {
        Tile closestTile = null;
        float closestDistance = Mathf.Infinity;
        foreach( Tile tile in surroundingTiles)
        {
            if (!tile.walkable)
            {
                continue;
            }

            float distance = Vector3.Distance(unitPosition, tile.transform.position);
            if( distance < closestDistance)
            {
                closestTile = tile;
            }
        }

        return closestTile;
    }
}
