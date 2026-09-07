using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    //coordinates
    public int x;
    public int z;

    //pathfinding values
    public float g;
    public float h;
    public float f;
    public int movementCost;
    public Tile cameFrom = null;

    public TileController tileController;
    public enum TileType
    {
        Ground,
        Water
    }
    public bool walkable = false;

    public TileType tiletype;

    public Material groundMaterial;
    public Material waterMaterial;

    private List<Tile> adjacentTiles = new List<Tile>();
    void Start()
    {

    }

    void Update()
    {

    }

    public void Initialize(TileType type)
    {
        tiletype = type;
        SetMaterial();
    }

    void SetMaterial()
    {
        switch (tiletype)
        {
            case TileType.Ground:
                GetComponent<Renderer>().material = groundMaterial;
                walkable = true;
                movementCost = 1;
                break;

            case TileType.Water:
                GetComponent<Renderer>().material = waterMaterial;
                walkable = false;
                movementCost = 0;
                break;
        }
    }

    public void SetAdjacentTiles()
    {
        int[,] offsets =
    {
        { -1, -1 }, { 0, -1 }, { 1, -1 },
        { -1,  0 },             { 1,  0 },
        { -1,  1 }, { 0,  1 }, { 1, 1 }
    };

        for (int i = 0; i < 8; i++)
        {
            Tile adjacentTile = tileController.GetTile(x + offsets[i,0], z + offsets[i,1]);
            if(adjacentTile != null)
            {
                adjacentTiles.Add(adjacentTile);
            }
        }
    }

    public List<Tile> GetAdjacentTiles()
    {
        return adjacentTiles;
    }
}
