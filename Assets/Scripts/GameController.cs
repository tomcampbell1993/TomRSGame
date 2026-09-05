using UnityEngine;

public class GameController : MonoBehaviour
{

    public TileController tileController;
    public UnitController unitController;
    public BuildingController buildingController;
    public TerrainGenerator terrainGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terrainGenerator.Initialize(TerrainGenerator.GeneratedTerrain.Lakes);
        tileController.initialize(terrainGenerator.terrainData);
        unitController.CreateUnit(1, 1);
        unitController.CreateUnit(2, 2);
        buildingController.CreateBuilding(5, 7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
