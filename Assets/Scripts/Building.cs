using UnityEngine;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    public BuildingController buildingController;
    public TileController tileController;
    public Tile coreTile; // Start or (centre) tile of building, x and z count from this tile to the right and up so (0,0)
    public int lengthX; // X direction size of building always x > 0, x = 2 means its two tiles wide, 1 to the right of (0,0)
    public int lengthZ; // See X but for z value upwards
    public List<Tile> containingTiles;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetContainingTiles()
    {
        for (int x = coreTile.x; x < coreTile.x + lengthX; x++)
        {
            for (int z = coreTile.z; z < coreTile.z + lengthZ; z++)
            {
                containingTiles.Add(tileController.GetTile(x, z));
            }
        }
    }
}
