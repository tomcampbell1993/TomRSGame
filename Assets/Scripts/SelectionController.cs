using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


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

                Unit unit = unitController.selectedUnit;
                unit.targetPoint = new Vector3(hit.point.x, 1.0f, hit.point.z);

                Tile startTile = unit.currentTile;
                Tile targetTile = clickedTile;

                List<Tile> path = unitController.pathfinder.FindPath(startTile, targetTile);

                if(path != null)
                {
                    path = unitController.pathfinder.SmoothPath(path);

                    unit.FollowPath(path);
                }

            }
        }
    }
}
