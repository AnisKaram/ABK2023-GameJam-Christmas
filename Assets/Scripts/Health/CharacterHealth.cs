using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CharacterHealth : MonoBehaviour
{
    #region Fields
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _currentHealth;
    [SerializeField] private TextMeshProUGUI _maxHealth;

    [SerializeField] private int _health;
    #endregion

    #region Events
    public static event UnityAction OnPlayerDied;
    #endregion

    #region Properties
    public int Health
    {
        get { return _health; }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// This method is used to set the health for every character.
    /// </summary>
    /// <param name="health">Health integer</param>
    public void SetHealth(int health)
    {
        _health = health;
        UpdateMaxHealthOnUI();
        UpdateCurrentHealthOnUI();
        UpdateHealthSlider();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(_health, 0, 100);
        UpdateCurrentHealthOnUI();
        UpdateHealthSlider();
        CheckHealth();
    }

    public void GainHealth(int health)
    {
        _health += health;
        _health = Mathf.Clamp(_health, 1, 100);
        UpdateCurrentHealthOnUI();
        UpdateHealthSlider();
    }
    #endregion

    #region Private Methods
    private void CheckHealth()
    {
        if (_health < 1)
        {
            OnPlayerDied?.Invoke();
        }
    }

    private void UpdateHealthSlider()
    {
        _healthSlider.value = _health + 10;
    }

    private void UpdateCurrentHealthOnUI()
    {
        _currentHealth.text = $"{_health}";
    }

    private void UpdateMaxHealthOnUI()
    {
        _maxHealth.text = $"{_health}";
    }
    #endregion
}