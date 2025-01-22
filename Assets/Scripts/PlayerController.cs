using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings Move")]
    [SerializeField] private float playerSpeed = 5f;


    [Header("Settings Jump")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float knockbackForce = 10f;
    private LayerMask groundLayer;
    private bool isGrounded = true;


    [Header("Reference")]
    private Animator animator;
    private Rigidbody2D rb;


    [Header("InputActions")]
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Vector2 input;


    [Header("Animations")]
    private string currentState;
    const string Player_Idle = "player_idle";
    const string Player_Run = "player_run";
    const string Player_Jump = "player_jump";
    const string Player_Shoot = "player_shoot";


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        Debug.Log(input);
    }

    private void FixedUpdate()
    {
        moveInput = new Vector2(input.x, 0).normalized;
        Vector2 moveDirection = moveInput.normalized;
        rb.MovePosition(rb.position + moveDirection * playerSpeed * Time.fixedDeltaTime);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
