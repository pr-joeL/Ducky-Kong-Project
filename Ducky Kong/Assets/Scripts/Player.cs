using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _feetCol;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float groundAcceleration = 12f;
    [SerializeField] private float groundDeceleration = 12f;
    [SerializeField] private float airAcceleration = 12f;
    [SerializeField] private float airDeceleration = 12f;

    [Header("Jump & Gravity")]
    [SerializeField] private float airGravity = 40f;
    [SerializeField] private float jumpVelocity = 18f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckLength = 0.1f; // FIXED (was 12)
    [SerializeField] private LayerMask _groundLayer;

    [Header("Ladder")]
    [SerializeField] private float climbSpeed = 6f;
    [SerializeField] private LayerMask ladderLayer;

    private bool _isOnLadder;
    private bool _isClimbing;

    private Vector2 _moveVelocity;
    private Vector2 input;

    private bool _isGrounded;
    private bool _isFacingRight;
    private bool _jumpRequested;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _feetCol = GetComponent<BoxCollider2D>();

        _rb.gravityScale = 0f; // We handle gravity manually
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (_isOnLadder && Mathf.Abs(input.y) > 0)
        {
            _isClimbing = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
            _jumpRequested = true;
    }

    private void FixedUpdate()
    {
        // Sync with real Rigidbody velocity FIRST
        _moveVelocity = _rb.linearVelocity;

        CheckGrounded();

        if (_isClimbing)
        {
            HandleClimbing();
        }
        else
        {
            HandleJump();
            HandleGravity();
            HandleMovement();
        }

        // Apply final velocity ONCE
        _rb.linearVelocity = _moveVelocity;
    }

    private void HandleMovement()
    {
        gameObject.layer = 0;
        float targetSpeed = input.x * moveSpeed;

        if (input.x != 0)
        {
            TurnCheck(input.x);

            float accel = _isGrounded ? groundAcceleration : airAcceleration;
            _moveVelocity.x = Mathf.Lerp(_moveVelocity.x, targetSpeed, accel * Time.fixedDeltaTime);
        }
        else
        {
            float decel = _isGrounded ? groundDeceleration : airDeceleration;
            _moveVelocity.x = Mathf.Lerp(_moveVelocity.x, 0f, decel * Time.fixedDeltaTime);
        }
    }

    private void HandleClimbing()
    {
        gameObject.layer = 9;

        // Exit ladder at bottom
        if (_isGrounded && input.y <= 0)
        {
            _isClimbing = false;
            return;
        }

        _moveVelocity.y = input.y * climbSpeed;
        _moveVelocity.x = 0f;

        if (_isOnLadder)
        {
            RaycastHit2D ladder = Physics2D.BoxCast(
                _feetCol.bounds.center,
                _feetCol.bounds.size,
                0f,
                Vector2.zero,
                0f,
                ladderLayer
            );

            if (ladder.collider != null)
            {
                Vector3 pos = transform.position;
                pos.x = ladder.collider.bounds.center.x;
                transform.position = pos;
            }
        }

        if (!_isOnLadder)
        {
            _isClimbing = false;
        }
    }

    private void HandleJump()
    {
        if (_jumpRequested && _isGrounded)
        {
            _moveVelocity.y = jumpVelocity;
        }

        _jumpRequested = false;
    }

    private void HandleGravity()
    {
        if (!_isGrounded)
        {
            _moveVelocity.y -= airGravity * Time.fixedDeltaTime;
        }
        else if (_moveVelocity.y < 0f)
        {
            _moveVelocity.y = 0f;
        }
    }

    private void CheckGrounded()
    {
        Vector2 origin = _feetCol.bounds.center;
        Vector2 size = _feetCol.bounds.size;

        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            size,
            0f,
            Vector2.down,
            groundCheckLength,
            _groundLayer
        );

        Collider2D ladderHit = Physics2D.OverlapBox(
        _feetCol.bounds.center,
        _feetCol.bounds.size,
        0f,
        ladderLayer
        );

        _isOnLadder = ladderHit != null;

        _isGrounded = hit.collider != null;

        // Debug Visual
        Color rayColor = _isGrounded ? Color.green : Color.red;
        Debug.DrawRay(origin + Vector2.left * size.x / 2, Vector2.down * groundCheckLength, rayColor);
        Debug.DrawRay(origin + Vector2.right * size.x / 2, Vector2.down * groundCheckLength, rayColor);
    }

    private void TurnCheck(float moveDirection)
    {
        if (_isFacingRight && moveDirection < 0)
            Flip();
        else if (!_isFacingRight && moveDirection > 0)
            Flip();
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
