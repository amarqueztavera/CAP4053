using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Store movement direction
        movement = new Vector2(moveX, moveY);

        // Set animation state based on movement
        animator.SetBool("isWalking", movement.sqrMagnitude > 0);
    }

    void FixedUpdate()
    {
        // Move player with Rigidbody
        rb.linearVelocity = movement.normalized * moveSpeed;
    }
}
