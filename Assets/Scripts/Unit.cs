using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{

    public UnitController unitController;
    public bool selected = false;
    public float moveSpeed;
    public Tile currentTile;
    public List<Tile> currentPath;

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

        Tile targetTile = currentPath[pathIndex];

        transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position + Vector3.up, moveSpeed * Time.deltaTime);

        if (transform.position == targetTile.transform.position + Vector3.up)
        {
            currentTile = targetTile;

            if (pathIndex == currentPath.Count - 1)
            {
                isMoving = false;
                return;
            }
            pathIndex++;

        }
    }

    public void FollowPath(List<Tile> path)
    {
        isMoving = true;
        pathIndex = 1;
        currentPath = path;

        foreach (Tile tile in path)
        {
            Debug.Log("unit is moving to " + tile);
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
