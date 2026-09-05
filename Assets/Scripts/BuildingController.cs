using UnityEngine;
using System.Collections.Generic;


public class BuildingController : MonoBehaviour
{
    public GameObject buildingPrefab;
    public TileController tileController;
    public List<GameObject> buildings = new List<GameObject>();
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateBuilding(int xPosition, int zPosition)
    {
        GameObject building = Instantiate(buildingPrefab, transform);
        Tile targetTile = tileController.GetTile(xPosition, zPosition);

        Building buildingScript = building.GetComponent<Building>();
        buildingScript.buildingController = this;
        buildingScript.tileController = tileController;
        buildingScript.coreTile = targetTile;  
        buildingScript.lengthX = 5;
        buildingScript.lengthZ = 2;
        buildingScript.SetContainingTiles();
        foreach(Tile tile in buildingScript.containingTiles)
        {
            tile.walkable = false;
        }

        building.transform.position = PlaceBuilding(buildingScript);

        building.transform.localScale =
            new Vector3(
                buildingScript.lengthX * tileController.tileSize,
                building.transform.localScale.y,
                buildingScript.lengthZ * tileController.tileSize);
        buildings.Add(building);
    }

    private Vector3 PlaceBuilding(Building building)
    {
        Vector3 coreTilePosition = building.coreTile.transform.position;

        float offsetX = (building.lengthX - 1) * tileController.tileSize / 2f;
        float offsetZ = (building.lengthZ - 1) * tileController.tileSize / 2f;

        return coreTilePosition + new Vector3(offsetX, 1, offsetZ);
    }
}
