using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{

    public enum GeneratedTerrain
    {
        Lakes,
        Ground
    }
    public int mapSize;
    public List<TileTerrainData> terrainData = new List<TileTerrainData>();

    public void Initialize(GeneratedTerrain terrain)
    {
        switch (terrain)
        {
            case GeneratedTerrain.Lakes:
                GenerateLakes(); break;

            case GeneratedTerrain.Ground:
                break;
        }

    }

    void GenerateLakes()
    {
        mapSize = 24;

        terrainData.Clear();

        for (int i = 0; i < mapSize; i++)
        {
            for ( int j = 0;  j < mapSize; j++)
            {
                if ( i  > 4 && i < 8 && j > 3 && j < 7)
                {
                    terrainData.Add(new TileTerrainData
                    {
                        x = i,
                        z = j,
                        type = Tile.TileType.Water
                    });
                }
                else
                {
                    terrainData.Add(new TileTerrainData
                    {
                        x = i,
                        z = j,
                        type = Tile.TileType.Ground
                    });
                }
            }
        }
    }
}
