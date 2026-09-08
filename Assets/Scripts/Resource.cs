using UnityEngine;
using System.Collections.Generic;

public class Resource : MonoBehaviour
{
    public enum ResourceType
    {
        Tree,
        Stone
    }

    public ResourceType resourceType;

    public TileController tileController;

    public Material treeMaterial;
    public Material stoneMaterial;

    public Tile containingTile;
    public List<Tile> surroundingTiles = new List<Tile>();
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Initialize(TileController tileController, Tile containingTile, ResourceType type)
    {
        this.tileController = tileController;
        this.containingTile = containingTile;
        resourceType = type;
        SetResourceType();
        SetSurroundingTiles();
    }

    void SetResourceType()
    {
        switch (resourceType)
        {
            case ResourceType.Tree:
                containingTile.walkable = false;
                break;

            case ResourceType.Stone:
                containingTile.walkable = false;
                break;
        }
    }
    private void SetSurroundingTiles()
    {
        surroundingTiles = containingTile.GetAdjacentTiles();
    }
}
