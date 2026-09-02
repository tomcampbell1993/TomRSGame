using UnityEngine;

public class UnitController : MonoBehaviour
{

    public GameObject unitPrefab;
    public TileController tileController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateUnit(int xPosition, int zPosition)
    {
        GameObject unit = Instantiate(unitPrefab, transform);
        Tile targetTile = tileController.GetTile(xPosition, zPosition);

        unit.transform.position = targetTile.transform.position + Vector3.up;
    }
}
