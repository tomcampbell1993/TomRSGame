using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject tilePrefab;

    public int width;
    public int height;

    public float tileSize;

    void Start()
    {
        GenerateGrid(width, height);
    }

    void GenerateGrid(int gridWidth, int gridHeight)
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GameObject tile = Instantiate(tilePrefab, transform);

                tile.name = $"Tile_{width}_{height}";
                tile.transform.localPosition = new Vector3(i * tileSize, 0 , j * tileSize);
            }
        }
    }

}
