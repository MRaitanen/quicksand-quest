using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [Tooltip("Time allowed to jump after leaving the ground")]
    [SerializeField] private float coyoteTimeDuration = 0.2f;
    [Tooltip("Multiplier for jump height when jump button is released early")]
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [Tooltip("Multiplier for gravity when falling")]
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Ground check")]
    [Tooltip("Layer for ground detection")]
    [SerializeField] private LayerMask groundLayer;
    [Tooltip("Size of the ground check box")]
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.1f);
    [Tooltip("Offset for the ground check")]
    [SerializeField] private Vector2 groundCheckOffset = new Vector2(0f, -0.5f);

    // Variables 
    private float _coyoteTimeCounter;
    private bool _grounded;
    private bool _jumpPressed;

    // Components
    private Rigidbody2D _rb;

    private void Awake()
    {
        // Attempt to get the Rigidbody2D component; add it if not found
        if (!TryGetComponent<Rigidbody2D>(out _rb))
            _rb = gameObject.AddComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Call Methods
        HandleMovement();
        HandleJump();
        UpdateCoyoteTime();
        GroundCheck();
        Falling();

        // TODO: Handle animations
        // TODO: Handle camera follow
    }

    private void HandleMovement()
    {
        // Get horizontal input (A/D or Left/Right Arrow)
        float horInput = Input.GetAxis("Horizontal");

        // Set new velocity based on input
        Vector2 velocity = _rb.linearVelocity;
        velocity.x = horInput * moveSpeed;

        // Apply new velocity to the Rigidbody2D
        _rb.linearVelocity = velocity;
    }

    private void HandleJump()
    {
        // Check if the jump button was pressed
        if (Input.GetButtonDown("Jump"))
        {
            // Allow jump if grounded or within coyote time
            if (_grounded || _coyoteTimeCounter > 0)
            {
                // Set jump velocity
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, jumpForce);
                _jumpPressed = true;
            }
        }

        // Check for cut cut when player releases the jump button
        if (Input.GetButtonUp("Jump") && _jumpPressed)
        {
            // Check if the player is still ascending
            if (_rb.linearVelocityY > 0)
            {
                // Reduce the jump velocity
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _rb.linearVelocityY * jumpCutMultiplier);
            }
            _jumpPressed = false;
        }
    }

    private void Falling()
    {
        // Check if the player is falling
        if (_rb.linearVelocityY < 0)
        {
            // Apply extra gravity when falling
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void UpdateCoyoteTime()
    {
        // Reset coyote time if grounded; otherwise, decrement the counter
        _coyoteTimeCounter = _grounded ? coyoteTimeDuration : Mathf.Max(0, _coyoteTimeCounter - Time.deltaTime);
    }

    private void GroundCheck()
    {
        // Calculate the position for the ground check box cast
        Vector2 boxCastPosition = (Vector2)transform.position + groundCheckOffset;

        // Check if the player is grounded using BoxCast
        _grounded = Physics2D.BoxCast(boxCastPosition, boxSize, 0f, Vector2.down, 0, groundLayer);

    }

    // This will only be visible in the Unity Editor and is used for debugging purposes
    void OnDrawGizmos()
    {
        // Calculate position for the ground check box cast
        Vector2 boxCastPosition = (Vector2)transform.position + groundCheckOffset;

        // Set the Gizmos color for visualization
        Gizmos.color = Color.red; // Change the color as desired

        // Draw the box at the calculated position to visualize the ground check
        Gizmos.DrawWireCube(boxCastPosition, boxSize);
    }
}
