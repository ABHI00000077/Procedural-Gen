using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 move = transform.position + movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(move);
    }
}
