using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{

    public TileController tileController;

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

        Tile currentTile = startTile;

        currentTile.g = 0;
        currentTile.h = CalculateHeuristic(currentTile, targetTile);
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

            foreach (Tile tile in currentTile.GetAdjacentTiles())
            {

                if (closedList.Contains(tile))
                {
                    continue;
                }

                if (!tile.walkable)
                {
                    continue;
                }

                float diagonalMultiplier = 1f;

                int xDifference = Mathf.Abs(tile.x - currentTile.x);
                int zDifference = Mathf.Abs(tile.z - currentTile.z);

                bool isDiagonal = xDifference == 1 && zDifference == 1;

                if (isDiagonal)
                {
                    int xDirection = tile.x - currentTile.x;
                    int zDirection = tile.z - currentTile.z;

                    if (!CanMoveDiagonally(currentTile, xDirection, zDirection))
                    {
                        continue;
                    }
                    diagonalMultiplier = Mathf.Sqrt(2);
                }

                float newG = currentTile.g + (tile.movementCost * diagonalMultiplier);

                if (openList.Contains(tile))
                {
                    if (newG < tile.g)
                    {
                        tile.g = newG;
                        tile.f = newG + tile.h;
                        tile.cameFrom = currentTile;
                    }
                }
                else
                {
                    tile.g = newG;
                    tile.h = CalculateHeuristic(tile, targetTile);
                    tile.f = tile.g + tile.h;
                    tile.cameFrom = currentTile;
                    openList.Add(tile);
                }
            }
        }
        return null;
    }

    private float CalculateHeuristic(Tile currentTile, Tile targetTile)
    {
        float xDistance = currentTile.x - targetTile.x;
        float zDistance = currentTile.z - targetTile.z;

        return Mathf.Sqrt(xDistance * xDistance + zDistance * zDistance);
    }

    public bool HasClearPath(Tile startTile, Tile targetTile)
    {
        int x0 = startTile.x;
        int z0 = startTile.z;

        int x1 = targetTile.x;
        int z1 = targetTile.z;

        int dx = Mathf.Abs(x1 - x0);
        int dz = Mathf.Abs(z1 - z0);

        int xStep = x0 < x1 ? 1 : -1;
        int zStep = z0 < z1 ? 1 : -1;

        int x = x0;
        int z = z0;

        int error = dx - dz;

        while (true)
        {
            Tile tile = tileController.GetTile(x, z);

            if (tile == null || !tile.walkable)
            {
                return false;
            }

            if (x == x1 && z == z1)
            {
                return true;
            }

            int previousX = x;
            int previousZ = z;

            int error2 = 2 * error;

            if (error2 > -dz)
            {
                error -= dz;
                x += xStep;
            }

            if (error2 < dx)
            {
                error += dx;
                z += zStep;
            }

            // If we moved diagonally, make sure we aren't
            // cutting through the corner of an obstacle.
            if (x != previousX && z != previousZ)
            {
                Tile previousTile = tileController.GetTile(previousX, previousZ);

                if (!CanMoveDiagonally(previousTile, xStep, zStep))
                {
                    return false;
                }
            }
        }
    }

    public List<Tile> SmoothPath(List<Tile> path)
    {
        List<Tile> smoothPath = new List<Tile>();

        int currentIndex = 0;

        smoothPath.Add(path[currentIndex]);

        while (currentIndex < path.Count - 1)
        {
            int furthestIndex = currentIndex + 1;

            for (int i = currentIndex + 1; i < path.Count; i++)
            {
                if (HasClearPath(path[currentIndex], path[i]))
                {
                    furthestIndex = i;
                }
                else
                {
                    break;
                }
            }
            smoothPath.Add(path[furthestIndex]);
            currentIndex = furthestIndex;
        }
        return smoothPath;
    }

    private bool CanMoveDiagonally(Tile currentTile, int xDirection, int zDirection)
    {
        Tile horizontalTile = tileController.GetTile(currentTile.x + xDirection, currentTile.z);
        Tile verticalTile = tileController.GetTile(currentTile.x, currentTile.z + zDirection);

        if (horizontalTile == null || !horizontalTile.walkable || verticalTile == null || !verticalTile.walkable)
        {
            return false;
        }
        return true;
    }
}
