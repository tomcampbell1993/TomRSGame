using UnityEngine;
using System.Collections.Generic;


public class TileController : MonoBehaviour
{
    public GameObject tilePrefab;

    public int width;
    public int height;
    public float tileSize;
    public List<Tile> allTiles = new List<Tile>();

    private Tile[,] tiles;

    void Start()
    {

    }

    public void initialize()
    {
        tiles = new Tile[width, height];
        GenerateGrid(width, height);
    }

    void GenerateGrid(int gridWidth, int gridHeight)
    {



        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                GameObject tileObject = Instantiate(tilePrefab, transform);

                Tile tile = tileObject.GetComponent<Tile>();

                tile.x = x;
                tile.z = z;

                tile.name = $"Tile_{x}_{z}";

                if ((x == 2 && z <= 5) || (x == 5 && z >= 2))
                {
                    tile.Initialize(Tile.TileType.Water);
                }

                else
                {
                    tile.Initialize(Tile.TileType.Ground);
                }

                tile.transform.localPosition = new Vector3(x * tileSize, 0, z * tileSize);

                tiles[x, z] = tile;
                allTiles.Add(tile);
            }
        }
    }

    public Tile GetTile(int x, int z)
    {
        if (x < 0 || x >= width || z < 0 || z >= height)
        {
            return null;
        }
        else
        {
            return tiles[x, z];
        }           
    }

}
