using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings Move")]
    [SerializeField] private float playerSpeed = 5f;


    [Header("Settings Jump")]
    [SerializeField] private float platformJumpForce = 12f;
    [SerializeField] private float airJumpForce = 6f;
    [SerializeField] private int maxAirJumps = 4;
    private int currentAirJumps;


    [Header("Settings Platform Timer")]
    [SerializeField] private float platformTimeLimit = 3f;
    [SerializeField] private float platformTimerRecovery = 1f;
    private float platformTimer;


    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;


    [Header("Reference")]
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D playerCollider;


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
        //animator = GetComponent<Animator>();      //Descomentar cuando se tenga animaciones
        playerInput = GetComponent<PlayerInput>();
        playerCollider = GetComponent<Collider2D>();
        platformTimer = platformTimeLimit;
    }

    private void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();

        isGrounded = IsGrounded();

        if (isGrounded)
        {
            currentAirJumps = 0;
            platformTimer -= Time.deltaTime;

            if (platformTimer <= 0)
            {
                Die();
            }

            if (playerInput.actions["Jump"].WasPressedThisFrame())
            {
                Jump(platformJumpForce);
            }

        }
        else
        {
            RecoveryPlatformTimer();

            if (playerInput.actions["Jump"].WasPressedThisFrame() && currentAirJumps < maxAirJumps)
            {
                currentAirJumps++;
                Jump(airJumpForce);
            }
        }
    }

    private void FixedUpdate()
    {
        moveInput = new Vector2(input.x, 0).normalized;

        rb.linearVelocity = new Vector2(moveInput.x * playerSpeed, rb.linearVelocity.y);
    }

    private void Jump(float jumpForce)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        //ChangeAnimationState(Player_Jump);    //Descomentar cuando se tenga animaciones
    }

    private void RecoveryPlatformTimer()
    {
        platformTimer += platformTimerRecovery * Time.deltaTime;
        platformTimer = Mathf.Clamp(platformTimer, 0, platformTimeLimit);
    }

    private void Die()
    {
        Destroy(gameObject);
        Debug.Log("GAME OVER: Time limit");
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeight, groundLayer);

        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + extraHeight), raycastHit.collider != null ? Color.green : Color.red);

        return raycastHit.collider != null;
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
