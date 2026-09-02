using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float cameraHeight = 10f;
    [SerializeField] private float cameraAngle = 45f;

    private void Start()
    {
        transform.position = new Vector3(
            transform.position.x,
            cameraHeight,
            transform.position.z
        );

        transform.rotation = Quaternion.Euler(cameraAngle, 0f, 0f);
    }

    private void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed)
            horizontal -= 1f;

        if (Keyboard.current.dKey.isPressed)
            horizontal += 1f;

        if (Keyboard.current.sKey.isPressed)
            vertical -= 1f;

        if (Keyboard.current.wKey.isPressed)
            vertical += 1f;

        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        transform.position += movement.normalized * moveSpeed * Time.deltaTime;
    }
}