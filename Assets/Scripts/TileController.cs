using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject tilePrefab;

    public int width;
    public int height;
    public float tileSize;

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
                tile.transform.localPosition = new Vector3(x * tileSize, 0 , z * tileSize);

                tiles[x, z] = tile;
            }
        } 
    }

    public Tile GetTile(int x, int z)
    {
        return tiles[x, z];
    }

}
