using UnityEngine;

public class FireWisp : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float leftBound;
    public float rightBound;
    public float directionChangeChance = 0.01f; // chance per frame to flip direction

    [Header("Damage")]
    public float damageCooldown = 1.5f;

    private float direction = 1f;
    private float cooldownTimer = 0f;

    private void Start()
    {
        // Start moving randomly left or right
        direction = Random.value < 0.5f ? -1f : 1f;
    }

    private void Update()
    {
        HandleMovement();
        HandleCooldown();
    }

    private void HandleMovement()
    {
        // Move
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // Bounds check
        if (transform.position.x <= leftBound)
        {
            direction = 1f;
        }
        else if (transform.position.x >= rightBound)
        {
            direction = -1f;
        }
        else
        {
            // Random chance to change direction
            if (Random.value < directionChangeChance)
            {
                direction *= -1f;
            }
        }
    }

    private void HandleCooldown()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && cooldownTimer <= 0f)
        {
            collision.gameObject.GetComponent<Player>().LoseLife();
            cooldownTimer = damageCooldown;
        }
    }
}
