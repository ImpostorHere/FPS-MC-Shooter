using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Setting")]
    public float moveSpeed = 5;
    public float horizontalRotateSpeed = 10f;
    public float verticalRotateSpeed = 10f;

    [Header("References")]
    public Camera PlayerCamera;
    public Rigidbody RB;

    [SerializeField]
    Vector3 MouseDelta => Input.mousePositionDelta;

    void Update()
    {
        if (MouseDelta.x != 0)
        {
            transform.Rotate(Vector3.up, horizontalRotateSpeed * Time.deltaTime * MouseDelta.x);
        }
        
        if (MouseDelta.y != 0)
        {
            PlayerCamera.transform.Rotate(Vector3.right, -verticalRotateSpeed * Time.deltaTime * MouseDelta.y);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            RB.linearVelocity = transform.forward * moveSpeed;
        }
    }
}
