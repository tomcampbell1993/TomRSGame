using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public UnitController unitController;
    public float moveSpeed;
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

    public void Initialize(UnitController unitController, UnitType unitType)
    {
        this.unitController = unitController;
        this.unitType = unitType;
    }

    private void Movement()
    {
        if (pathIndex < currentPath.Count)
        {

            Tile targetTile = currentPath[pathIndex];

            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position + Vector3.up, moveSpeed * Time.deltaTime);

            if (transform.position == targetTile.transform.position + Vector3.up)
            {
                currentTile = targetTile;
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
}
