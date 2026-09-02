using UnityEngine;

public class Unit : MonoBehaviour
{

    public UnitController unitController;
    public bool selected = false;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        unitController.selectUnit(this);
        selected = true;
    }
}
