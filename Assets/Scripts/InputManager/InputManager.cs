using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Fields
    [Header("Camera")]
    [SerializeField] private Camera _mainCamera;

    private Controls _controls;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _controls = new Controls();
        _controls.Enable();

        _controls.Gameplay.Movement.performed += _ => OnMovementStarted();
        _controls.Gameplay.Movement.canceled += _ => OnMovementCanceled();

        _controls.Gameplay.Interact.started += _ => OnInteractionStarted();

        _controls.Gameplay.Pause.started += _ => OnPauseStarted();

        _controls.Gameplay.Reload.started += _ => OnReloadStarted();

        _controls.Gameplay.Jump.started += _ => OnJumpStarted();

        _controls.Gameplay.Fire.started += _ => OnFireStarted();
        _controls.Gameplay.Fire.canceled += _ => OnFireCanceled();

        _controls.Gameplay.Aim.started += _ => OnAimStarted();
        _controls.Gameplay.Aim.canceled += _ => OnAimCanceled();

        _controls.Gameplay.Switch.started += _ => OnSwitchStarted();

        _controls.Gameplay.Look.performed += _ => OnLookingAround();

        _controls.Gameplay.Crouch.started += _ => OnCrouchStarted();
    }

    private void OnDestroy()
    {
        _controls.Disable();

        _controls.Gameplay.Movement.performed -= _ => OnMovementStarted();
        _controls.Gameplay.Movement.canceled -= _ => OnMovementCanceled();

        _controls.Gameplay.Interact.started -= _ => OnInteractionStarted();

        _controls.Gameplay.Pause.started -= _ => OnPauseStarted();

        _controls.Gameplay.Reload.started -= _ => OnReloadStarted();

        _controls.Gameplay.Jump.started -= _ => OnJumpStarted();

        _controls.Gameplay.Fire.started -= _ => OnFireStarted();
        _controls.Gameplay.Fire.canceled -= _ => OnFireCanceled();

        _controls.Gameplay.Aim.started -= _ => OnAimStarted();
        _controls.Gameplay.Aim.canceled -= _ => OnAimCanceled();

        _controls.Gameplay.Switch.started -= _ => OnSwitchStarted();

        _controls.Gameplay.Look.performed -= _ => OnLookingAround();

        _controls.Gameplay.Crouch.started -= _ => OnCrouchStarted();
    }
    #endregion

    #region Private Methods
    private void OnMovementStarted()
    {
        // Returns a vector2 data type
        Debug.Log($"Movement started {_controls.Gameplay.Movement.ReadValue<Vector2>()}");
    }

    private void OnMovementCanceled()
    {
        Debug.Log($"Movement canceled");
    }

    private void OnInteractionStarted()
    {
        Debug.Log("Interact");
    }

    private void OnPauseStarted()
    {
        Debug.Log("Pause");
    }

    private void OnReloadStarted()
    {
        Debug.Log("Reload");
    }

    private void OnJumpStarted()
    {
        Debug.Log("Jump");
    }

    private void OnFireStarted()
    {
        Debug.Log("Fire started");
    }

    private void OnFireCanceled()
    {
        Debug.Log("Fire canceled");
    }

    private void OnAimStarted()
    {
        Debug.Log("Aim started");
    }

    private void OnAimCanceled()
    {
        Debug.Log("Aim canceled");
    }

    private void OnLookingAround()
    {
        Vector2 GameplayPosition = _mainCamera.ScreenToViewportPoint(_controls.Gameplay.Look.ReadValue<Vector2>());
        Debug.Log($"Looking, position: {GameplayPosition}");
    }

    private void OnSwitchStarted()
    {
        Debug.Log($"Switch");
    }

    private void OnCrouchStarted()
    {
        Debug.Log("Crouch");
    }
    #endregion
}