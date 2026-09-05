using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingController buildingController;
    public Tile coreTile; // Start or (centre) tile of building, x and z count from this tile to the right and up so (0,0)
    public int lengthX; // X direction size of building always x > 0, x = 2 means its two tiles wide, 1 to the right of (0,0)
    public int lengthZ; // See X but for z value upwards
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
