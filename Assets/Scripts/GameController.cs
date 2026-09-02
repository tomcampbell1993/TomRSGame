using UnityEngine;

public class GameController : MonoBehaviour
{

    public TileController tileController;
    public UnitController unitController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileController.initialize();
        unitController.CreateUnit(1, 1);
        unitController.CreateUnit(2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
