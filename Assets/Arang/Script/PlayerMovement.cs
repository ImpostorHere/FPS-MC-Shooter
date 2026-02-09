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

    Vector3 MouseDelta => Input.mousePositionDelta;
    Vector2 _moveInput;

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

        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");
        _moveInput.Normalize();
    }

    void FixedUpdate()
    {
        MoveCharacter_Horizontal(_moveInput);
    }

    /// <summary>
    /// Will be called at FixedUpdate()
    /// </summary>
    /// <param name="inputDir"></param>
    void MoveCharacter_Horizontal(Vector2 inputDir)
    {
        Vector3 moveDirection = new Vector3(inputDir.x, 0, inputDir.y);
        
        Vector3 localDir = transform.TransformDirection(moveDirection);

        RB.linearVelocity = localDir * moveSpeed * Time.fixedDeltaTime;
    }
}
