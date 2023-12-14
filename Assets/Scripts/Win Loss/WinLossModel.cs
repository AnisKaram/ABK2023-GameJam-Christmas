using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLossModel : MonoBehaviour
{
    [SerializeField] private WinLossPresenter _winLossPresenter;

    private void Awake()
    {
        CharacterHealth.OnPlayerDied += GameOver;
    }

    private void OnDestroy()
    {
        CharacterHealth.OnPlayerDied -= GameOver;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameWon();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GameOver();
        }
    }

    // TODO add for both Game won/over a coroutine to wait a bit
    // then show the canvases
    private void GameWon()
    {
        GameManager.Instance.StopGame();
        InputManager.Instance.Controls.Disable();
        GameManager.Instance.ConfineCursor();
        _winLossPresenter.ShowGameWonCanvas();
    }

    private void GameOver()
    {
        GameManager.Instance.StopGame();
        InputManager.Instance.Controls.Disable();
        GameManager.Instance.ConfineCursor();
        _winLossPresenter.ShowGameOverCanvas();
    }
}