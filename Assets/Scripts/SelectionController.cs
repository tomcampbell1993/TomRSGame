using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class NewMonoBehaviourScript : MonoBehaviour
{

    public UnitController unitController;
    public TileController tileController;
    void Start()
    {

    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {

        if (unitController.selectedUnit == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                Building clickedBuilding = hit.collider.GetComponent<Building>();
                Unit unit = unitController.selectedUnit;
                Tile targetTile;
                Tile startTile;
                List<Tile> path;

                if (clickedBuilding != null)
                {

                    targetTile = clickedBuilding.GetClosestSurroundingTile(unit.transform.position);

                    if (targetTile == null)
                    {
                        Debug.Log("No walkable surrounding tile found.");
                        return;
                    }                   
                    startTile = tileController.GetTileFromWorldPosition(unit.transform.position);
                    path = unitController.pathfinder.FindPath(startTile, targetTile);
                    if (path != null)
                    {
                        path = unitController.pathfinder.SmoothPath(path);
                        unit.targetBuilding = clickedBuilding;
                        unit.FollowPath(path);
                    }
                    return;
                }

                Tile clickedTile = hit.collider.GetComponent<Tile>();
                if (clickedTile == null)
                {
                    return;
                }
                if (!clickedTile.walkable)
                {
                    return;
                }
                unit.targetBuilding = null;
                unit.targetPoint = new Vector3(hit.point.x, 1.0f, hit.point.z);
                startTile = tileController.GetTileFromWorldPosition(unit.transform.position);
                targetTile = clickedTile;
                path = unitController.pathfinder.FindPath(startTile, targetTile);
                if (path != null)
                {
                    path = unitController.pathfinder.SmoothPath(path);
                    unit.FollowPath(path);
                }
            }
        }
    }
}
