using UnityEngine;
using System.Collections.Generic;

public class UnitController : MonoBehaviour
{

    public GameObject unitPrefab;
    public TileController tileController;
    public Pathfinder pathfinder;
    public List<GameObject> units = new List<GameObject>();
    public Unit selectedUnit;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void CreateUnit(int xPosition, int zPosition)
    {
        GameObject unit = Instantiate(unitPrefab, transform);
        Tile targetTile = tileController.GetTile(xPosition, zPosition);

        Unit unitScript = unit.GetComponent<Unit>();
        unitScript.unitController = this;

        unitScript.currentTile = targetTile;
        unit.transform.position = targetTile.transform.position + Vector3.up;
        units.Add(unit);
    }

    public void selectUnit(Unit unit)
    {
        if(selectedUnit != null)
        {
            selectedUnit.Deselect();
        }
        selectedUnit = unit;
        unit.Select();
    }

}
