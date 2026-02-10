using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    private BoxCollider2D _feetCol;
   
    [SerializeField] private float moveSpeed = 12;
    [SerializeField] private float groundCheckLength= 12;
    [SerializeField] private float groundAcceleration = 12;
    [SerializeField] private float groundDeceleration = 12;
    [SerializeField] private float airAcceleration = 12;
    [SerializeField] private float airDeceleration = 12;
    [SerializeField] private float groundGravity = 9.8f;
    [SerializeField] private float airGravity = 9.8f;
    [SerializeField] private float jumpVelocity = 60f;

    [SerializeField] private LayerMask _groundLayer;

    private RaycastHit2D _groundHit;
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    private bool _isGrounded;
    private bool _bumpedHead;

    private Vector2 input = Vector2.zero;

    private void Awake()
    {
        Time.timeScale = 1.2f;
        _isFacingRight = false;

        _rb = GetComponent<Rigidbody2D>();
        _feetCol = GetComponent<BoxCollider2D>();
}

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        HandleJump();
    }
    private void FixedUpdate()
    {
        CollisionCheck();
        HandleGravity();
        
        if (_isGrounded )
        {
            Move(groundAcceleration, groundDeceleration, input);
        }
        else
        {
            Move(airAcceleration, airDeceleration, input);
        }
    }

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);
            Vector2 targetVelocity = Vector2.zero;
            targetVelocity = new Vector2(moveInput.x, 0f) * moveSpeed;
            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _moveVelocity.y);
        }
        else if(moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _moveVelocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if(_isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if(turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            transform.Rotate(0f, -180f, 0f);
        }
    }

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCol.bounds.center.x, _feetCol.bounds.center.y);
        Vector2 boxCastSize = new Vector2(_feetCol.bounds.size.x, _feetCol.bounds.size.y);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, groundCheckLength, _groundLayer);

        if(_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        Color rayColor;
        if (_isGrounded)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * groundCheckLength, rayColor);
        Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * groundCheckLength, rayColor);
        Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - groundCheckLength), Vector2.down * boxCastSize.x, rayColor);
    }

    private void CollisionCheck()
    {
        IsGrounded();
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(_isGrounded)
            {
                _moveVelocity.y += jumpVelocity;
            }
        }
    }

    private void HandleGravity()
    {
        if (_isGrounded)
        {
            if (_moveVelocity.y < 0f)
                _moveVelocity.y = 0f;
        }
        else
        {
            _moveVelocity.y -= airGravity * Time.fixedDeltaTime;
        }
    }
}
