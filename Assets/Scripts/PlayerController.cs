using UnityEngine;
using Unity.Burst;
using Unity.Jobs;

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
        HandleJump();
        UpdateCoyoteTime();
        GroundCheck();

        // TODO: Handle animations
        // TODO: Handle camera follow
    }

    private void FixedUpdate()
    {
        // Call movement and falling logic in FixedUpdate for consistent physics behavior
        HandleMovement();
        Falling();
    }

    [BurstCompile]
    private void HandleMovement()
    {
        // Get horizontal input (A/D or Left/Right Arrow)
        float horInput = Input.GetAxis("Horizontal");

        // Set new velocity based on input
        Vector2 velocity = _rb.linearVelocity; // Use linearVelocity in Unity 6
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
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce); // Use linearVelocity for Y jump
                _jumpPressed = true;
            }
        }

        // Check for jump cut when player releases the jump button
        if (Input.GetButtonUp("Jump") && _jumpPressed)
        {
            // Check if the player is still ascending
            if (_rb.linearVelocity.y > 0)
            {
                // Reduce the jump velocity
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * jumpCutMultiplier);
            }
            _jumpPressed = false;
        }
    }

    [BurstCompile] // Optimize falling logic with Burst
    private void Falling()
    {
        // Check if the player is falling
        if (_rb.linearVelocity.y < 0)
        {
            // Apply extra gravity when falling
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void UpdateCoyoteTime()
    {
        // Reset coyote time if grounded; otherwise, decrement the counter
        _coyoteTimeCounter = _grounded ? coyoteTimeDuration : Mathf.Max(0, _coyoteTimeCounter - Time.deltaTime);
    }

    [BurstCompile] // Optimize ground checking with Burst
    private void GroundCheck()
    {
        // Calculate the position for the ground check box cast
        Vector2 boxCastPosition = (Vector2)transform.position + groundCheckOffset;

        // Check if the player is grounded using BoxCast
        _grounded = Physics2D.BoxCast(boxCastPosition, boxSize, 0f, Vector2.down, 0, groundLayer);

        // BoxCast for tag "Platform" to allow player to jump through platforms
        var hit = Physics2D.BoxCast(boxCastPosition, boxSize, 0f, Vector2.down, 0, LayerMask.GetMask("Platform"));
        if (hit)
        {
            // Set the player to parent of the platform
            transform.SetParent(hit.transform);

            // Set interpolation to extrapolate to reduce jittering
            _rb.interpolation = RigidbodyInterpolation2D.Extrapolate;
        }
        else
        {
            // Unparent the player
            transform.SetParent(null);

            // Reset interpolation
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    // This will only be visible in the Unity Editor and is used for debugging purposes
    void OnDrawGizmos()
    {
        // Calculate position for the ground check box cast
        Vector2 boxCastPosition = (Vector2)transform.position + groundCheckOffset;

        // Set the Gizmos color for visualization
        Gizmos.color = Color.red;

        // Draw the box at the calculated position to visualize the ground check
        Gizmos.DrawWireCube(boxCastPosition, boxSize);
    }
}
