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

    public List<Tile> FindPath(Tile startTile, Tile targetTile)
    {
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        foreach (Tile tile in tileController.allTiles)
        {
            tile.g = Mathf.Infinity;
            tile.h = 0;
            tile.f = Mathf.Infinity;
            tile.cameFrom = null;

        }

        // g is accumulated movement cost from the start tile to this tile
        // h heuristic estimated distance between current tile and target tile
        // f is total cost of the node

        int[,] offsets = { { -1, -1 }, { 0, -1 }, { +1, -1 }, { +1, 0 }, { +1, +1 }, { 0, +1 }, { -1, +1 }, { -1, 0 } };

        Tile currentTile = startTile;

        currentTile.g = 0;
        currentTile.h = Mathf.Sqrt(Mathf.Pow(currentTile.x - targetTile.x, 2) + Mathf.Pow(currentTile.z - targetTile.z, 2));
        currentTile.f = currentTile.g + currentTile.h;

        openList.Add(currentTile);

        while (openList.Count > 0)
        {

            Tile lowestF = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].f < lowestF.f)
                {
                    lowestF = openList[i];
                }
            }
            openList.Remove(lowestF);
            closedList.Add(lowestF);
            currentTile = lowestF;

            if (currentTile == targetTile)
            {
                Debug.Log("Target found!");

                List<Tile> path = new List<Tile>();

                Tile reverseTile = currentTile;

                while (reverseTile != null)
                {
                    path.Add(reverseTile);
                    reverseTile = reverseTile.cameFrom;

                }

                path.Reverse();
                return path;
            }

            Tile[] adjacentTiles = new Tile[8];
            for (int i = 0; i < 8; i++)
            {
                int x = currentTile.x + offsets[i, 0];
                int z = currentTile.z + offsets[i, 1];
                adjacentTiles[i] = tileController.GetTile(x, z);
            }

            for (int i = 0; i < 8; i++)
            {
                if (adjacentTiles[i] == null)
                {
                    continue;
                }

                if (closedList.Contains(adjacentTiles[i]))
                {
                    continue;
                }

                if (!adjacentTiles[i].walkable)
                {
                    continue;
                }

                float diagonalMultiplier = 1f;

                if (offsets[i, 0] != 0 && offsets[i, 1] != 0)
                {
                    diagonalMultiplier = Mathf.Sqrt(2);
                }

                float newG = currentTile.g + (adjacentTiles[i].movementCost * diagonalMultiplier);

                if (openList.Contains(adjacentTiles[i]))
                {
                    if (newG < adjacentTiles[i].g)
                    {
                        adjacentTiles[i].g = newG;
                        adjacentTiles[i].f = newG + adjacentTiles[i].h;
                        adjacentTiles[i].cameFrom = currentTile;
                    }
                }
                else
                {
                    adjacentTiles[i].g = newG;
                    adjacentTiles[i].h = Mathf.Sqrt(Mathf.Pow((adjacentTiles[i].x - targetTile.x), 2) + Mathf.Pow((adjacentTiles[i].z - targetTile.z), 2));
                    adjacentTiles[i].f = adjacentTiles[i].g + adjacentTiles[i].h;
                    adjacentTiles[i].cameFrom = currentTile;
                    openList.Add(adjacentTiles[i]);
                }
            }
        }
        return null;
    }
}
