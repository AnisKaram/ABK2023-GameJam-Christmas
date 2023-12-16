using UnityEngine;
using TMPro;

/// <summary>
/// Note: only use this script in the testing scene.
/// </summary>
public class PlayerDebugger : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CharacterHealth _characterHealth;

    [Header("Texts - TextMeshPro")]
    [SerializeField] private TextMeshProUGUI _playerSpeedText;
    [SerializeField] private TextMeshProUGUI _playerGroundedText;
    [SerializeField] private TextMeshProUGUI _playerHealthText;

    private void Update()
    {
        _playerSpeedText.text = $"Player Speed: {_playerController.PlayerSpeed}";
        _playerGroundedText.text = $"Player Grounded: {_playerController.PlayerGrounded}";
        _playerHealthText.text = $"Player Health: {_characterHealth.Health}";
    }
}
