using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    [Header("Scripts")]
    [SerializeField] private CharacterHealth _characterHealth;

    [Header("Character Controller")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private GameObject PauseMenu;
    private GameObject pauseMenuInstance;

    [Header("Transforms")]
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Transform _playerGroundCheck;
    [SerializeField] private Transform _playerBody;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask groundMask;

    private InputManager _inputManager;
    private Controls controls;

    private const float _mouseSensitivity = 50f;

    private float _xRotation = 0f;

    private float _playerSpeed;
    private const float _defaultPlayerSpeed = 7.5f;
    private const float _sprintingSpeed = 10f;
    private const float _gravity = -19.62f; // default is -9.81f

    private const float groundDistance = 0.4f;
    
    private Vector3 _velocity;
    private bool _isGrounded;

    private const float jumpHeight = 3f;

    // Crouching fields
    private const float _crouchSpeed = 3.5f;
    private const float _crouchingHeight = 0.5f;
    private const float _normalHeight = 2f;
    private bool _isCrouching;
    #endregion

    #region Properties
    public float PlayerSpeed
    {
        get { return _playerSpeed; }
    }

    public bool PlayerGrounded
    {
        get { return _isGrounded; }
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        InputManager.OnJumpTriggered += HandlePlayerJump;
        InputManager.OnSprintTriggered += HandlePlayerSprint;
        InputManager.OnSprintStopped += HandlePlayerStoppedSprinting;
        InputManager.OnCrouchTriggered += HandlePlayerCrouching;
        InputManager.OnF3Pressed += F3Pressed;

        controls = new Controls();

        _characterHealth.SetHealth(health: 100);
    }

    private void F3Pressed()
    {
        if (Input.GetKey(KeyCode.F3))
        {
            if (pauseMenuInstance == null)
            {
                //stop time
                Time.timeScale = 0; //this just stops time, does not disable player input
                pauseMenuInstance = Instantiate(PauseMenu);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                controls.Gameplay.Disable(); // why does this not work? :/
                controls.Gameplay.Pause.Enable();
            }
            else
            {
                //resume
                Time.timeScale = 1;
                Destroy(pauseMenuInstance);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                controls.Gameplay.Enable();
            }
        }
    }


    private void Start()
    {
        _inputManager = InputManager.Instance;

        Cursor.lockState = CursorLockMode.Locked;

        _playerSpeed = _defaultPlayerSpeed;
    }

    private void Update()
    {
        _isGrounded = IsPlayerGrounded();
        TryResetPlayerVelocity();

        HandlePlayerMovement();
        HandlePlayerLookingAround();
    }

    private void OnDestroy()
    {
        InputManager.OnJumpTriggered -= HandlePlayerJump;
        InputManager.OnSprintTriggered -= HandlePlayerSprint;
        InputManager.OnSprintStopped -= HandlePlayerStoppedSprinting;
        InputManager.OnCrouchTriggered -= HandlePlayerCrouching;
        InputManager.OnF3Pressed -= F3Pressed;
    }
    #endregion

    #region Private Methods
    private void HandlePlayerMovement()
    {
        Vector2 input = _inputManager.GetPlayerMovement();
        float x = input.x;
        float z = input.y;

        Vector3 movement = transform.right * x + transform.forward * z;

        _characterController.Move(movement * _playerSpeed * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void HandlePlayerLookingAround()
    {
        Vector2 input = _inputManager.GetMouseDelta();
        float mouseX = input.x * _mouseSensitivity * Time.deltaTime;
        float mouseY = input.y * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -70f, 70f);

        // Up and down rotation.
        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Left and right rotation.
        _playerBody.Rotate(Vector3.up * mouseX);
    }

    private void HandlePlayerJump()
    {
        if (_isGrounded && !_isCrouching)
        {
            ChangePlayerSpeed(speed: _defaultPlayerSpeed);
            _velocity.y += Mathf.Sqrt(jumpHeight * -2f * _gravity);
        }
    }

    private void TryResetPlayerVelocity()
    {
        if (_isGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }
    }

    private bool IsPlayerGrounded()
    {
        return Physics.CheckSphere(_playerGroundCheck.position, groundDistance, groundMask);
    }

    /// <summary>
    /// This method is used to let the player sprint only when he's on the
    /// ground.
    /// </summary>
    private void HandlePlayerSprint()
    {
        if (_isGrounded && !_isCrouching)
        {
            ChangePlayerSpeed(speed: _sprintingSpeed);
        }
    }

    /// <summary>
    /// This method is used to let the player stop sprinting only when the
    /// player speed is greater than the original one, which is 7.5f.
    /// </summary>
    private void HandlePlayerStoppedSprinting()
    {
        if (_playerSpeed > _defaultPlayerSpeed)
        {
            ChangePlayerSpeed(speed: _defaultPlayerSpeed);
        }
    }

    private void HandlePlayerCrouching(bool isPlayerCrouching)
    {
        _isCrouching = isPlayerCrouching;

        // Crouch
        if (isPlayerCrouching)
        {
            _characterController.height = _crouchingHeight;
            ChangePlayerSpeed(speed: _crouchSpeed);
            return;
        }

        // Back to its normal height
        _characterController.height = _normalHeight;
        ChangePlayerSpeed(speed: _defaultPlayerSpeed);
    }

    private void ChangePlayerSpeed(float speed)
    {
        _playerSpeed = speed;
    }
    #endregion
}