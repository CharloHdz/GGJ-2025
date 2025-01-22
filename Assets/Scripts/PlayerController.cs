using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings Player")]
    public float playerSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public float knockbackForce = 10f;
    private Rigidbody2D rb;
    [SerializeField] private bool isGrounded = true;
    [Header("InputActions")]
    private Vector2 moveInput;
    public InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction jumpAction;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        moveAction = inputActions.FindAction("Move");
        jumpAction = inputActions.FindAction("Jump");

        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

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
        }
    }

    private void ApplyKnockback(Vector2 hitPoint)
    {
        Vector2 knockbackDirection = ((Vector2)transform.position - hitPoint).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}
