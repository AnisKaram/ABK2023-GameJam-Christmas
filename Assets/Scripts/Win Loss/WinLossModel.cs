using System.Collections;
using UnityEngine;

public class WinLossModel : MonoBehaviour
{
    [SerializeField] private WinLossPresenter _winLossPresenter;

    private void Awake()
    {
        CharacterHealth.OnPlayerDied += GameOver;
        SpawnManager.OnGoalReached += GameWon;
    }

    private void OnDestroy()
    {
        CharacterHealth.OnPlayerDied -= GameOver;
        SpawnManager.OnGoalReached -= GameWon;
    }

    private void GameWon()
    {
        StartCoroutine(WaitAndGameWon());
    }

    private void GameOver()
    {
        StartCoroutine(WaitAndGameOver());
    }

    private IEnumerator WaitAndGameWon()
    {
        Time.timeScale = 0.25f;
        InputManager.Instance.Controls.Disable();
        GameManager.Instance.ConfineCursor();
        yield return new WaitForSeconds(1f);
        _winLossPresenter.ShowGameWonCanvas();
    }

    private IEnumerator WaitAndGameOver()
    {
        Time.timeScale = 0.25f;
        InputManager.Instance.Controls.Disable();
        GameManager.Instance.ConfineCursor();
        yield return new WaitForSeconds(1f);
        _winLossPresenter.ShowGameOverCanvas();
    }
}