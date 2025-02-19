using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the player moves
    private Rigidbody2D rb;       // Reference to the Rigidbody2D component

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component from the player
    }

    // Update is called once per frame
    void Update()
    {
        // Get the player's input (WASD or Arrow keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Create the movement vector
        Vector2 movement = new Vector2(moveX, moveY).normalized * moveSpeed;

        // Apply the movement to the Rigidbody2D's velocity
        rb.linearVelocity = movement;
    }
}
