using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public UnitController unitController;
    public float moveSpeed;
    public float unitRadius = 0.25f;
    public Tile currentTile;
    public List<Tile> currentPath = new List<Tile>();
    public Vector3 targetPoint;
    public Building targetBuilding;
    public Resource targetResource;
    public BigItem.BigItemType carriedBigItem = BigItem.BigItemType.None;

    public enum UnitType
    {
        Worker,
        Fighter
    }

    public UnitType unitType;

    public enum UnitState
    {
        Idle,
        Moving,
        Mining,
    }
    public UnitState unitState = UnitState.Idle;

    private int pathIndex = 0;
    private float miningTimer = 0f;
    void Start()
    {

    }

    void Update()
    {
        UpdateState();

        if(unitState == UnitState.Moving)
        {
            List<Unit> collidingUnits = GetCollidingUnits();

            foreach(Unit unit in collidingUnits)
            {
                ResolveUnitCollisions(this, unit);
            }
        }
    }

    void UpdateState()
    {
        switch (unitState)
        {
            case UnitState.Idle:
                break;

            case UnitState.Moving:
                Movement();
                break;
            case UnitState.Mining:
                Mining();
                break;
        }
    }

    public void Initialize(UnitController unitController, UnitType unitType, Tile currentTile, float unitRadius)
    {
        this.unitController = unitController;
        this.unitType = unitType;
        this.currentTile = currentTile;
        currentTile.AddOccupyingUnit(this);
        this.unitRadius = unitRadius;
    }

    private void Movement()
    {
        if (pathIndex < currentPath.Count)
        {

            Tile targetTile = currentPath[pathIndex];

            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position + Vector3.up, moveSpeed * Time.deltaTime);
            UpdateCurrentTile();

            if (transform.position == targetTile.transform.position + Vector3.up)
            {
                pathIndex++;
            }
        }
        else
        {
            if (targetResource != null)
            {
                if (targetResource.resourceType == Resource.ResourceType.Stone)
                {
                    unitState = UnitState.Mining;
                    return;
                }
            }

            unitState = UnitState.Idle;

            if (targetBuilding != null)
            {
                targetBuilding = null;
            }
        }
    }

    void UpdateCurrentTile()
    {
        Tile newTile = unitController.tileController.GetTileFromWorldPosition(transform.position);
        if (newTile == null)
        {
            return;
        }
        if (newTile == currentTile)
        {
            return;
        }
        currentTile.RemoveOccupyingUnit(this);
        newTile.AddOccupyingUnit(this);
        currentTile = newTile;
    }

    //This is what you would call externally in selection controller to get this moving, the condition in movements depends on the size of path
    public void FollowPath(List<Tile> path)
    {
        unitState = UnitState.Moving;
        pathIndex = 1;
        currentPath = path;
    }

    public void MoveToPoint(Vector3 point)
    {
        transform.position = Vector3.MoveTowards(transform.position, point, moveSpeed * Time.deltaTime);

        if (transform.position == point)
        {
            unitState = UnitState.Idle;
            return;
        }
    }

    void Mining()
    {
        if (targetResource == null)
        {
            unitState = UnitState.Idle;
            return;
        }

        if (carriedBigItem != BigItem.BigItemType.None)
        {
            unitState = UnitState.Idle;
            return;
        }

        miningTimer += Time.deltaTime;
        if (miningTimer >= 10.0f)
        {
            miningTimer = 0f;

            if (targetResource.resourceType == Resource.ResourceType.Stone)
            {
                carriedBigItem = BigItem.BigItemType.Stone;
            }

            unitState = UnitState.Idle;
            targetResource = null;
            return;
        }
    }

    // Returns bool whether This unit has collided with another based on the distance between their combined radius
    bool IsUnitCollisionDetected(Unit thisUnit, Unit thatUnit)
    {
        Vector2 thisUnitPosition = new Vector2(thisUnit.transform.position.x, thisUnit.transform.position.z);
        Vector2 thatUnitPosition = new Vector2(thatUnit.transform.position.x, thatUnit.transform.position.z);
        float distance = Vector2.Distance(thisUnitPosition, thatUnitPosition);
        if (distance < thisUnit.unitRadius + thatUnit.unitRadius)
        {
            return true;
        }
        return false;
    }

    // This unit looks at its tile and all its surrounding tiles to see which units exist and whether they have collided
    List<Unit> GetCollidingUnits()
    {
        List<Unit> potentialUnits = new List<Unit>();
        List<Unit> collidingUnits = new List<Unit>();

        foreach (Unit unit in currentTile.occupyingUnits)
        {
            if (unit != this)
            {
                potentialUnits.Add(unit);
            }
        }
        foreach (Tile tile in currentTile.GetAdjacentTiles())
        {
            foreach (Unit unit in tile.occupyingUnits)
            {
                potentialUnits.Add(unit);
            }
        }
        if (potentialUnits.Count == 0)
        {
            return collidingUnits;
        }
        foreach (Unit unit in potentialUnits)
        {
            if (IsUnitCollisionDetected(this, unit))
            {
                collidingUnits.Add(unit);
            }
        }
        return collidingUnits;
    }


    void ResolveUnitCollisions(Unit thisUnit, Unit thatUnit)
    {
        Vector2 thisUnitPosition = new Vector2(thisUnit.transform.position.x, thisUnit.transform.position.z);
        Vector2 thatUnitPosition = new Vector2(thatUnit.transform.position.x, thatUnit.transform.position.z);
        Vector2 targetPosition = new Vector2(targetPoint.x, targetPoint.z);
        float overlap = (thisUnit.unitRadius + thatUnit.unitRadius) - (Vector2.Distance(thisUnitPosition, thatUnitPosition));

        Vector2 travelDirection = (targetPosition - thisUnitPosition).normalized;
        // toOtherUnit is a vector between the two units containing distance and direction
        Vector2 toOtherUnit = (thatUnitPosition - thisUnitPosition);
        Vector2 perpendicularTravelDirection = new Vector2(-travelDirection.y, travelDirection.x);
        float opposite = Vector2.Dot(toOtherUnit, perpendicularTravelDirection);
        float angle = Mathf.Asin(opposite / toOtherUnit.magnitude);

        float thisUnitSpeedShare;
        float thatUnitSpeedShare;

        if (thatUnit.unitState == Unit.UnitState.Idle)
        {
            thisUnitSpeedShare = 0f;
            thatUnitSpeedShare = 1f;
        }
        else
        {

            float totalSpeed = thisUnit.moveSpeed + thatUnit.moveSpeed;

            if (totalSpeed <= 0f)
            {
                return;
            }

            thisUnitSpeedShare = thisUnit.moveSpeed / totalSpeed;
            thatUnitSpeedShare = thatUnit.moveSpeed / totalSpeed;

        }

        float thisUnitCorrection = overlap * thisUnitSpeedShare;
        float thatUnitCorrection = overlap * thatUnitSpeedShare;

        Vector2 thisUnitDeflectionDirection = -Mathf.Sign(opposite) * perpendicularTravelDirection;
        Vector2 thatUnitDeflectionDirection = -thisUnitDeflectionDirection;

        Vector2 thisUnitNewPosition = thisUnitPosition + thisUnitDeflectionDirection * thisUnitCorrection;

        Vector2 thatUnitNewPosition = thatUnitPosition + thatUnitDeflectionDirection * thatUnitCorrection;

        thisUnit.transform.position = new Vector3(
            thisUnitNewPosition.x,
            thisUnit.transform.position.y,
            thisUnitNewPosition.y
        );

        thatUnit.transform.position = new Vector3(
            thatUnitNewPosition.x,
            thatUnit.transform.position.y,
            thatUnitNewPosition.y
        );
    }
}
