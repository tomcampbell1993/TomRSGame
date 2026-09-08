using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class SelectionController : MonoBehaviour
{

    public UnitController unitController;
    public TileController tileController;
    public BuildingController buildingController;
    public ResourceController resourceController;

    public Unit selectedUnit;

    private Tile clickedTile;
    private Unit clickedUnit;
    private Building clickedBuilding;
    private Resource clickedResource;
    void Start()
    {

    }

    void Update()
    {
        HandleLeftClick();
        HandleRightClick();
    }

    void HandleLeftClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GetClickedObject(hit);
                if (clickedUnit != null)
                {                   
                    if (selectedUnit == clickedUnit)
                    {
                        return;
                    }
                    DeSelect();
                    selectedUnit = clickedUnit;
                }
            }
        }
    }

    void HandleRightClick()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GetClickedObject(hit);
                if (selectedUnit == null)
                {
                    return;
                }
                if(clickedTile != null)
                {
                    HandleMovement();
                    return;
                }
                if(clickedBuilding != null)
                {
                    HandleMovement();
                    return;
                }
            }
        }
    }

    void GetClickedObject(RaycastHit hit)
    {
        clickedTile = hit.collider.GetComponent<Tile>();
        clickedUnit = hit.collider.GetComponent<Unit>();
        clickedBuilding = hit.collider.GetComponent<Building>();
        clickedResource = hit.collider.GetComponent<Resource>();
    }

    void DeSelect()
    {
        selectedUnit = null;
    }

    void HandleMovement()
    {
        Tile targetTile;

        if(clickedBuilding != null)
        {
            targetTile = clickedBuilding.GetClosestSurroundingTile(selectedUnit.transform.position);
            if (targetTile == null)
            {
                return;
            }
            selectedUnit.targetBuilding = clickedBuilding;
        }
        else
        {
            selectedUnit.targetBuilding = null;
            targetTile = clickedTile;
        }

        if (!targetTile.walkable)
        {
            return;
        }
        Tile startTile = tileController.GetTileFromWorldPosition(selectedUnit.transform.position);
        List<Tile> path = unitController.pathfinder.FindPath(startTile, targetTile);
        if (path != null)
        {
            path = unitController.pathfinder.SmoothPath(path);
            selectedUnit.FollowPath(path);
        }
    }
}
