using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Setting")]
    public float moveSpeed = 5;
    public float horizontalRotateSpeed = 10f;
    public Rigidbody RB;

    [SerializeField]
    Vector3 MouseDelta => Input.mousePositionDelta;

    void Update()
    {
        if (MouseDelta.x != 0)
        {
            transform.Rotate(Vector3.up, horizontalRotateSpeed * Time.deltaTime * MouseDelta.x);
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
