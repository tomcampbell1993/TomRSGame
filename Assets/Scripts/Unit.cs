using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{

    public UnitController unitController;
    public bool selected = false;
    public float moveSpeed;
    public Tile currentTile;
    public List<Tile> currentPath;
    public Vector3 targetPoint;

    private bool isMoving = false;
    private int pathIndex = 0;
    void Start()
    {

    }

    void Update()
    {
        Movement();
    }

    private void OnMouseDown()
    {
        unitController.selectUnit(this);
    }

    private void Movement()
    {
        if (!isMoving)
        {
            return;
        }

        if (pathIndex < currentPath.Count -1)
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
            MoveToPoint(targetPoint);
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

        if(transform.position == point)
        {
            isMoving = false;
            return;
        }
    }

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }
}
