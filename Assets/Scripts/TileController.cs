using UnityEngine;
using System.Collections.Generic;


public class TileController : MonoBehaviour
{
    public GameObject tilePrefab;
    public TerrainGenerator terrainGenerator;
    public float tileSize;
    public List<Tile> allTiles = new List<Tile>();

    private Tile[,] tiles;

    void Start()
    {

    }

    public void initialize(List<TileTerrainData> terrainData)
    {
        tiles = new Tile[terrainGenerator.mapSize, terrainGenerator.mapSize];
        GenerateGrid(terrainData);
    }

    void GenerateGrid(List<TileTerrainData> terrainData)
    {
        foreach (TileTerrainData data in terrainData)
        {
            GameObject tileObject = Instantiate(tilePrefab, transform);
            Tile tile = tileObject.GetComponent<Tile>();
            tile.x = data.x;
            tile.z = data.z;
            tile.Initialize(data.type);
            tile.name = $"Tile_{tile.x}_{tile.z}_{tile.tiletype}";
            tile.transform.localPosition = new Vector3(tile.x * tileSize, 0, tile.z * tileSize);
            tiles[tile.x, tile.z] = tile;
            allTiles.Add(tile);
        }
    }

    public Tile GetTile(int x, int z)
    {
        if (x < 0 || x >= tiles.GetLength(0) || z < 0 || z >= tiles.GetLength(1))
        {
            return null;
        }
        return tiles[x, z];
    }
    public Tile GetTileFromWorldPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / tileSize);
        int z = Mathf.RoundToInt(worldPosition.z / tileSize);

        return GetTile(x, z);
    }

}
