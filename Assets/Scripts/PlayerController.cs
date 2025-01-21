using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Script de ejemplo:P
    public float playerSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public float knockbackForce = 10f; 

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    [SerializeField] private bool isGrounded = true;
    private int hitsTaken = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 2f, groundLayer);

        float horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * playerSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeHitAndKnockback(collision.contacts[0].point);
        }
    }

    private void TakeHitAndKnockback(Vector2 hitPoint)
    {
        hitsTaken++;

        if (hitsTaken >= 5)
        {
            DieAndRespawn();
        }
        else
        {
            ApplyKnockback(hitPoint);
        }
    }

    private void DieAndRespawn()
    {
        hitsTaken = 0;
        transform.position = initialPosition;
    }

    private void ApplyKnockback(Vector2 hitPoint)
    {
        Vector2 knockbackDirection = ((Vector2)transform.position - hitPoint).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}
