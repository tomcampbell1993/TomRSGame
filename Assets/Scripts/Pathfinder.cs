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

        // g is accumulated movement cost from the start tile to this tile
        // h heuristic estimated distance between current tile and target tile
        // f is total cost of the node

        int[,] offsets = { { -1, -1 }, { 0, -1 }, { +1, -1 }, { +1, 0 }, { +1, +1 }, { 0, +1 }, { -1, +1 }, { -1, 0 } };
        Tile currentTile = startTile;
        currentTile.g = 0;
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
            //Debug.Log(currentTile.name);

            if (currentTile == targetTile)
            {
                Debug.Log(closedList);
                break;
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
                    }
                }
                else
                {
                    adjacentTiles[i].g = newG;
                    adjacentTiles[i].h = Mathf.Sqrt(Mathf.Pow((adjacentTiles[i].x - targetTile.x), 2) + Mathf.Pow((adjacentTiles[i].z - targetTile.z), 2));
                    adjacentTiles[i].f = adjacentTiles[i].g + adjacentTiles[i].h;
                    openList.Add(adjacentTiles[i]);
                }
            }
        }

    }
}
