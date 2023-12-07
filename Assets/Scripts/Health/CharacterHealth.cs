using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    #region Fields
    private int _health;
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
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(_health, 0, 100);
        CheckHealth();
    }

    public void GainHealth(int health)
    {
        _health += health;
        _health = Mathf.Clamp(_health, 1, 100);
    }
    #endregion

    #region Private Methods
    private void CheckHealth()
    {
        if (_health < 1)
        {
            Debug.Log("Player is died");
        }
    }
    #endregion
}