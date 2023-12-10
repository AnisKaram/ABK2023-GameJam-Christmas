using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour
{
    #region Fields
    private static InputManager _instance;

    private Controls _controls;

    public bool _isAnyKeyPressed;
    #endregion

    #region Properties
    public static InputManager Instance
    {
        get { return _instance; }
    }
    #endregion

    #region Events
    public static event UnityAction OnJumpTriggered;
    public static event UnityAction OnSprintTriggered;
    public static event UnityAction OnSprintStopped;
    public static event UnityAction<bool> OnCrouchTriggered;
    public static event UnityAction OnAnyKeyPressed;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Making one instance of this script.
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        _controls = new Controls();

        _controls.Gameplay.Interact.started += _ => InteractionStarted();

        _controls.Gameplay.Pause.started += _ => PauseStarted();

        _controls.Gameplay.Reload.started += _ => ReloadStarted();

        _controls.Gameplay.Jump.started += _ => JumpStarted();

        _controls.Gameplay.Fire.started += _ => FireStarted();
        _controls.Gameplay.Fire.canceled += _ => FireCanceled();

        _controls.Gameplay.Aim.started += _ => AimStarted();
        _controls.Gameplay.Aim.canceled += _ => AimCanceled();

        _controls.Gameplay.Switch.started += _ => SwitchStarted();

        _controls.Gameplay.Crouch.performed += _ => CrouchStarted();
        _controls.Gameplay.Crouch.canceled += _ => CrouchCanceled();

        _controls.Gameplay.Sprint.started += _ => SprintStarted();
        _controls.Gameplay.Sprint.canceled += _ => SpringCanceled();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsSplashScreenShowed)
        {
            InputSystem.onAnyButtonPress.CallOnce(ctrl => AnyKeyPressed());
        }
    }

    private void OnDestroy()
    {
        _controls.Gameplay.Interact.started -= _ => InteractionStarted();

        _controls.Gameplay.Pause.started -= _ => PauseStarted();

        _controls.Gameplay.Reload.started -= _ => ReloadStarted();

        _controls.Gameplay.Jump.started -= _ => JumpStarted();

        _controls.Gameplay.Fire.started -= _ => FireStarted();
        _controls.Gameplay.Fire.canceled -= _ => FireCanceled();

        _controls.Gameplay.Aim.started -= _ => AimStarted();
        _controls.Gameplay.Aim.canceled -= _ => AimCanceled();

        _controls.Gameplay.Switch.started -= _ => SwitchStarted();

        _controls.Gameplay.Crouch.started -= _ => CrouchStarted();
        _controls.Gameplay.Crouch.canceled -= _ => CrouchCanceled();

        _controls.Gameplay.Sprint.started -= _ => SprintStarted();
        _controls.Gameplay.Sprint.canceled -= _ => SpringCanceled();
    }
    #endregion

    #region Public Methods
    public Vector2 GetPlayerMovement()
    {
        return _controls.Gameplay.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return _controls.Gameplay.Look.ReadValue<Vector2>();
    }

    public bool IsPlayerJumped()
    {
        return _controls.Gameplay.Jump.triggered;
    }
    #endregion

    #region Private Methods
    private void InteractionStarted()
    {
        Debug.Log("Interact");
    }

    private void PauseStarted()
    {
        Debug.Log("Pause");
    }

    private void ReloadStarted()
    {
        Debug.Log("Reload");
    }

    private void JumpStarted()
    {
        OnJumpTriggered?.Invoke();
    }

    private void FireStarted()
    {
        Debug.Log("Fire started");
    }

    private void FireCanceled()
    {
        Debug.Log("Fire canceled");
    }

    private void AimStarted()
    {
        Debug.Log("Aim started");
    }

    private void AimCanceled()
    {
        Debug.Log("Aim canceled");
    }

    private void SwitchStarted()
    {
        Debug.Log($"Switch");
    }

    private void CrouchStarted()
    {
        OnCrouchTriggered?.Invoke(true);
    }

    private void CrouchCanceled()
    {
        OnCrouchTriggered?.Invoke(false);
    }

    private void SprintStarted()
    {
        OnSprintTriggered?.Invoke();
    }

    private void SpringCanceled()
    {
        OnSprintStopped?.Invoke();
    }

    private void AnyKeyPressed()
    {
        _isAnyKeyPressed = true;
        OnAnyKeyPressed?.Invoke();
    }
    #endregion
}