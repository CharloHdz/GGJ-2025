using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private float bubbleBonusTime = 0.5f;
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
    private float defaultZoomSize;


    [Header("Animations")]
    private string currentState;
    const string Player_Idle = "player_idle";
    const string Player_Run = "player_run";
    const string Player_Jump = "player_jump";
    const string Player_Air = "player_air";


    [Header("Effects")]
    [SerializeField] private ParticleSystem jumpParticle;
    [SerializeField] private ParticleSystem launchParticle;
    [SerializeField] private ParticleSystem landParticle;


    [Header("HUD")]
    [SerializeField] private Slider airJumpsSlider;

    [SerializeField] private chr_GameManager gm;
    [SerializeField] private LevelGenerator lg;
    [SerializeField] private TextMeshProUGUI TimeText;


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

        if (airJumpsSlider != null)
        {
            airJumpsSlider.maxValue = maxAirJumps;
            airJumpsSlider.value = currentAirJumps;
        }
    }

    private void Update()
    {
        //Poner el platformTimer en el HUD en decimales
        TimeText.text = "Air: " + platformTimer.ToString("F2");
        input = playerInput.actions["Move"].ReadValue<Vector2>();

        bool wasGrounded = isGrounded;
        isGrounded = IsGrounded();

        if (isGrounded && !wasGrounded)
        {
            PlayLandParticles();
        }

        if (isGrounded)
        {
            currentAirJumps = maxAirJumps;
            UpdateAirJumpsSlider();

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
                PlayJumpParticles();
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

            if (playerInput.actions["Jump"].WasPressedThisFrame() && currentAirJumps > 0)
            {
                currentAirJumps--;
                UpdateAirJumpsSlider();
                Jump(airJumpForce);
                PlayLaunchParticles();
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
        //Destroy(gameObject);
        lg.InitializePool();
        gm.Respawn();
        platformTimer = platformTimeLimit;
        //Debug.Log("GAME OVER: Time limit");
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
            var shake = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
            if (shake != null)
            {
                shake.AmplitudeGain = 0.5f;
                shake.FrequencyGain = 2f;
            }
        }
    }

    private void ResetCameraEffects()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Lens.OrthographicSize = Mathf.Lerp(virtualCamera.Lens.OrthographicSize, defaultZoomSize, zoomSpeed * Time.deltaTime);
            var shake = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
            if (shake != null)
            {
                shake.AmplitudeGain = 0f;
                shake.FrequencyGain = 0f;
            }
        }
    }

    private void UpdateAirJumpsSlider()
    {
        if (airJumpsSlider != null)
        {
            airJumpsSlider.value = currentAirJumps;
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    private void PlayJumpParticles()
    {
        var mainModule = jumpParticle.main;
        jumpParticle.Play();
        launchParticle.Play();
    }

    private void PlayLaunchParticles()
    {
        launchParticle.Play();
    }

    private void PlayLandParticles()
    {
        var mainModule = landParticle.main;
        landParticle.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detectar colisión con burbujas
        if (collision.CompareTag("Bubble"))
        {
            platformTimer += bubbleBonusTime; // Añadir tiempo al temporizador
            collision.gameObject.GetComponent<Animator>().SetTrigger("pop");
        }
    }

    //OnCollision para que con el tag picos se muera con la funcion die
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Picos"))
        {
            Die();
        }
    }
}
