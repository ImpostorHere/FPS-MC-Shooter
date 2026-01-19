using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    public float rotateSpeed;
    public Rigidbody RB;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            RB.linearVelocity = transform.forward * moveSpeed;
        }
    }
}
