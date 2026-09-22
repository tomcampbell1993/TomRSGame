using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class SelectionController : MonoBehaviour
{

    public UnitController unitController;
    public TileController tileController;
    public BuildingController buildingController;
    public ResourceController resourceController;
    public RectTransform selectionBox;

    public List<Unit> selectedUnits = new List<Unit>();


    private Tile clickedTile;
    private Unit clickedUnit;
    private Building clickedBuilding;
    private Resource clickedResource;

    private Vector2 dragStartPosition; // This is here so that it doesnt change per frame.
    void Start()
    {

    }

    void Update()
    {
        HandleLeftMouse();
        HandleRightClick();
    }

    void HandleLeftMouse()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DeSelect();
            dragStartPosition = Mouse.current.position.ReadValue();           
        }

        if (Mouse.current.leftButton.isPressed)
        {
            selectionBox.gameObject.SetActive(true);
            UpdateSelectionBox(Mouse.current.position.ReadValue());
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Vector2 dragEndPosition = Mouse.current.position.ReadValue();
            float dragDistance = Vector2.Distance(dragStartPosition, dragEndPosition);
            if (dragDistance < 10f)
            {
                HandleLeftClick();
            }
            else
            {
                HandleLeftDrag(dragStartPosition, dragEndPosition);
            }
            selectionBox.gameObject.SetActive(false);
        }
    }

    void HandleLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GetClickedObject(hit);
            if (clickedUnit != null)
            {
                selectedUnits.Add(clickedUnit);
            }
        }
    }

    void HandleLeftDrag(Vector2 dragStart, Vector2 dragEnd)
    {
        float minX = Mathf.Min(dragStart.x, dragEnd.x);
        float maxX = Mathf.Max(dragStart.x, dragEnd.x);
        float minY = Mathf.Min(dragStart.y, dragEnd.y);
        float maxY = Mathf.Max(dragStart.y, dragEnd.y);

        foreach (GameObject unit in unitController.units)
        {
            Vector3 unitScreenPosition = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (unitScreenPosition.x < maxX && unitScreenPosition.x > minX && unitScreenPosition.y < maxY && unitScreenPosition.y > minY)
            {
                selectedUnits.Add(unit.GetComponent<Unit>());
            }
        }
    }

    void UpdateSelectionBox(Vector2 currentMousePosition)
    {
        Vector2 centre = (dragStartPosition + currentMousePosition) / 2;
        float width = Mathf.Abs(dragStartPosition.x - currentMousePosition.x);
        float height = Mathf.Abs(dragStartPosition.y - currentMousePosition.y);
        selectionBox.position = centre;
        selectionBox.sizeDelta = new Vector2(width, height);
    }

    void HandleRightClick()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GetClickedObject(hit);
                if (selectedUnits.Count == 0)
                {
                    return;
                }
                if (clickedTile != null)
                {
                    HandleMovement();
                    return;
                }
                if (clickedBuilding != null)
                {
                    HandleMovement();
                    return;
                }
                if (clickedResource != null)
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
        selectedUnits.Clear();
    }

    void HandleMovement()
    {
        Tile targetTile;

        foreach (Unit unit in selectedUnits)
        {

            if (clickedBuilding != null)
            {
                targetTile = clickedBuilding.GetClosestSurroundingTile(unit.transform.position);
                if (targetTile == null)
                {
                    continue;
                }
                unit.targetResource = null;
                unit.targetBuilding = clickedBuilding;
            }

            else if (clickedResource != null)
            {
                targetTile = clickedResource.GetClosestTile(unit.transform.position);
                if (targetTile == null)
                {
                    continue;
                }
                unit.targetResource = clickedResource;
                unit.targetBuilding = null;
            }
            else
            {
                unit.targetResource = null;
                unit.targetBuilding = null;
                targetTile = clickedTile;
            }

            if (!targetTile.walkable)
            {
                continue;
            }
            Tile startTile = tileController.GetTileFromWorldPosition(unit.transform.position);
            List<Tile> path = unitController.pathfinder.FindPath(startTile, targetTile);
            if (path != null)
            {
                path = unitController.pathfinder.SmoothPath(path);
                unit.FollowPath(path);
            }
        }
    }
}
