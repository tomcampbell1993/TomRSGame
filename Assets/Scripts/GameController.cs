using UnityEngine;

public class GameController : MonoBehaviour
{

    public TileController tileController;
    public UnitController unitController;
    public BuildingController buildingController;
    public ResourceController resourceController;
    public TerrainGenerator terrainGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terrainGenerator.Initialize(TerrainGenerator.GeneratedTerrain.Lakes);
        tileController.initialize(terrainGenerator.terrainData);
        unitController.CreateUnit(1, 1, Unit.UnitType.Worker);
        unitController.CreateUnit(2, 2, Unit.UnitType.Fighter);
        buildingController.CreateBuilding(5, 7);
        resourceController.CreateResource(12, 12, Resource.ResourceType.Tree);
        resourceController.CreateResource(14, 12, Resource.ResourceType.Stone);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
