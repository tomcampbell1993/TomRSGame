using UnityEngine;

public class Unit : MonoBehaviour
{

    public UnitController unitController;
    public bool selected = false;
    public float moveSpeed;

    private bool isMoving = false;
    private Vector3 targetPosition;
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
        if(!isMoving)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
    }

    public void MoveTo(Vector3 position)
    {
        targetPosition = position;
        isMoving = true;
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
