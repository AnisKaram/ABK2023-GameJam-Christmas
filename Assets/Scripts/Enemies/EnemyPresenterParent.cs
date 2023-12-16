using UnityEngine;
using UnityEngine.UI;

public class EnemyPresenterParent : MonoBehaviour
{
    #region Fields 
    [Header("Scriptable Objects")]
    [SerializeField] private ColorsData _enemyHealthColors;

    [Header("Images")]
    [SerializeField] private Image _healthImage;
    #endregion

    #region Public Methods
    public void UpdateHealthImageUI(float health, float defaultHealth)
    {
        _healthImage.fillAmount = health / defaultHealth;

        float percentage = health / defaultHealth * 100f;

        if (percentage > 50)
        {
            _healthImage.color = _enemyHealthColors.listOfEnemyHealthColors[0];
            return;
        }

        if (percentage > 30)
        {
            _healthImage.color = _enemyHealthColors.listOfEnemyHealthColors[1];
            return;
        }

        if (percentage <= 30)
        {
            _healthImage.color = _enemyHealthColors.listOfEnemyHealthColors[2];
        }
    }
    #endregion
}