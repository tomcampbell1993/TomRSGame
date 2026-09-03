using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public UnitController unitController;
    void Start()
    {
        
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {

        if(unitController.selectedUnit == null) {
            return;
        }

        if( Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                Tile clickedTile = hit.collider.GetComponent<Tile>();
                if(clickedTile == null)
                {
                    return;
                }
                if (!clickedTile.walkable)
                {
                    return;
                }

                unitController.selectedUnit.MoveTo(hit.point);
            }
        }
    }
}
