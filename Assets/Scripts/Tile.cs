using UnityEngine;

public class Tile : MonoBehaviour
{
    //coordinates
    public int x;
    public int z;

    //pathfinding values
    public float g;
    public float h;
    public float f;
    public int movementCost;
    public enum TileType
    {
        Ground,
        Water
    }
    public bool walkable = false;

    public TileType tiletype;

    public Material groundMaterial;
    public Material waterMaterial;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Initialize(TileType type)
    {
        tiletype = type;
        SetMaterial();
    }

    void SetMaterial()
    {
        switch (tiletype)
        {
            case TileType.Ground:
                GetComponent<Renderer>().material = groundMaterial;
                walkable = true;
                movementCost = 1;
                break;

            case TileType.Water:
                GetComponent<Renderer>().material = waterMaterial;
                walkable = false;
                movementCost = 0;
                break;
        }
    }
}
