using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed of the character")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    private Vector2 movementInput;

    private void Awake()
    {
        // Automatically try to get Rigidbody2D if not assigned in Inspector
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        // Read movement input from Unity's Input Manager (configured for WASD / Arrow keys)
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        // Normalize vector to prevent faster diagonal movement
        if (movementInput.sqrMagnitude > 1)
        {
            movementInput.Normalize();
        }
    }

    private void FixedUpdate()
    {
        // Move the Rigidbody2D if it exists, otherwise fallback to transform translation
        if (rb != null)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(movementInput * moveSpeed * Time.deltaTime);
        }
    }
}
