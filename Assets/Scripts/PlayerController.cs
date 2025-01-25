using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings Move")]
    [SerializeField] private float playerSpeed = 15f;


    [Header("Settings Jump")]
    [SerializeField] private float platformJumpForce = 12f;
    [SerializeField] private float airJumpForce = 6f;
    [SerializeField] private int maxAirJumps = 4;
    private int currentAirJumps;


    [Header("Settings Platform Timer")]
    [SerializeField] private float platformTimeLimit = 3f;
    [SerializeField] private float platformTimerRecovery = 1f;
    [SerializeField] private float platformZoomThreshold = 1f;
    private float platformTimer;


    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask RompibleLayer;
    private bool isGrounded;


    [Header("Reference")]
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;


    [Header("InputActions")]
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Vector2 input;


    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float zoomSize = 5.5f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private float defaultZoomSize;


    [Header("Animations")]
    private string currentState;
    const string Player_Idle = "player_idle";
    const string Player_Run = "player_run";
    const string Player_Jump = "player_jump";
    const string Player_Air = "player_air";


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerCollider = GetComponent<Collider2D>();

        platformTimer = platformTimeLimit;
        if (virtualCamera != null)
        {
            defaultZoomSize = virtualCamera.Lens.OrthographicSize;
        }
    }

    private void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();

        isGrounded = IsGrounded();

        if (isGrounded)
        {
            currentAirJumps = 0;
            platformTimer -= Time.deltaTime;

            if (platformTimer >= platformZoomThreshold)
            {
                ApplyEffectsCamera();
            }

            if (platformTimer <= 0)
            {
                Die();
            }

            if (playerInput.actions["Jump"].WasPressedThisFrame())
            {
                Jump(platformJumpForce);
                ChangeAnimationState(Player_Jump);
            }
            else if (Mathf.Abs(input.x) > 0.1)
            {
                ChangeAnimationState(Player_Run);
                transform.localScale = new Vector3(Mathf.Sign(input.x), 1, 1);
            }
            else
            {
                ChangeAnimationState(Player_Idle);
            }

        }
        else
        {
            ResetCameraEffects();
            RecoveryPlatformTimer();

            if (playerInput.actions["Jump"].WasPressedThisFrame() && currentAirJumps < maxAirJumps)
            {
                currentAirJumps++;
                Jump(airJumpForce);
                ChangeAnimationState(Player_Jump);
            }
            else if (rb.linearVelocity.y < 0)
            {
                ChangeAnimationState(Player_Air);
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
        //Disparar Bala
        chr_OP.instance.SpawnBullet(firePoint.position, firePoint.rotation);
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

    private void ApplyEffectsCamera()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Lens.OrthographicSize = Mathf.Lerp(virtualCamera.Lens.OrthographicSize, zoomSize, zoomSpeed * Time.deltaTime);
        }

        if (impulseSource != null && !impulseSource.enabled)
        {
            impulseSource.GenerateImpulse();
        }
    }

    private void ResetCameraEffects()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Lens.OrthographicSize = Mathf.Lerp(virtualCamera.Lens.OrthographicSize, defaultZoomSize, zoomSpeed * Time.deltaTime);
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
