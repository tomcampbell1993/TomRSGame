using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{

    public UnitController unitController;
    public float moveSpeed;
    public Tile currentTile;
    public List<Tile> currentPath = new List<Tile>();
    public Vector3 targetPoint;
    public Building targetBuilding;

    public enum UnitType
    {
        Worker,
        Fighter
    }

    public UnitType unitType;

    private bool isMoving = false;
    private int pathIndex = 0;
    void Start()
    {

    }

    void Update()
    {
        Movement();
    }

    public void Initialize(UnitController unitController, UnitType unitType)
    {
        this.unitController = unitController;
        this.unitType = unitType;
    }

    private void Movement()
    {
        if (!isMoving)
        {
            return;
        }

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
            isMoving = false;
            if(targetBuilding != null)
            {
                Debug.Log("Unit has reached " +  targetBuilding.name);
                targetBuilding = null;
            }
        }
    }

    public void FollowPath(List<Tile> path)
    {
        isMoving = true;
        pathIndex = 1;
        currentPath = path;
    }

    public void MoveToPoint(Vector3 point)
    {
        transform.position = Vector3.MoveTowards(transform.position, point, moveSpeed * Time.deltaTime);

        if (transform.position == point)
        {
            isMoving = false;
            return;
        }
    }
}
