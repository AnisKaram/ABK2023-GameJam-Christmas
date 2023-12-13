using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPresenterParent : MonoBehaviour
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private ColorsData _enemyHealthColors;

    public void UpdateHealthImageUI(float health, float defaultHealth)
    {
        _healthImage.fillAmount = health / defaultHealth;
        Debug.Log($"{health}, {defaultHealth}, {health / defaultHealth}");

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
}