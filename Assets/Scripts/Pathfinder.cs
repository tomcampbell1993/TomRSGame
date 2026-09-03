using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{

    public TileController tileController;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void FindPath(Tile startTile, Tile targetTile)
    {
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        // g is distance between current tile and start tile
        // h heuristic estimated distance between current tile and target tile
        // f is total cost of the node

        int[,] offsets = { { -1, -1 }, { 0, -1 }, { +1, -1 }, { +1, 0 }, { +1, +1 }, { 0, +1 }, { -1, +1 }, { -1, 0 } };

        Tile[] adjacentTiles = new Tile[8];
        for(int i = 0; i< 8; i++)
        {
            int x = startTile.x + offsets[i, 0];
            int z = startTile.z + offsets[i, 1];
            adjacentTiles[i] = tileController.GetTile(x, z);
        }
        
        for(int i = 0;i< 8; i++)
        {
            if( adjacentTiles[i] == null)
            {
                continue;
            }

            if(!adjacentTiles[i].walkable)
            {
                continue;
            }

            adjacentTiles[i].g = Mathf.Pow((adjacentTiles[i].x - startTile.x),2) + Mathf.Pow((adjacentTiles[i].z - startTile.z),2) + adjacentTiles[i].movementCost;
            adjacentTiles[i].h = Mathf.Pow((adjacentTiles[i].x - targetTile.x), 2) + Mathf.Pow((adjacentTiles[i].z - targetTile.z), 2);
            adjacentTiles[i].f = adjacentTiles[i].g + adjacentTiles[i].h;
        }

    }
}
