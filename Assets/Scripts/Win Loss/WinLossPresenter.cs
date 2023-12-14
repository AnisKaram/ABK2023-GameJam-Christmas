using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinLossPresenter : MonoBehaviour
{
    [Header("Game Won Buttons")]
    [SerializeField] private Button _mainMenuButtonInGameWon;
    [SerializeField] private Button _exitButtonInGameWon;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _backInCreditsButton;

    [Header("Game Over Buttons")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButtonInGameOver;
    [SerializeField] private Button _exitButtonInGameOver;

    [Header("Game Won GameObjects")]
    [SerializeField] private GameObject _winningScreenCanvas;
    [SerializeField] private GameObject _creditsCanvas;

    [Header("Game Over GameObjects")]
    [SerializeField] private GameObject _losingScreenCanvas;

    private UnityAction _mainMenuAction;
    private UnityAction _exitAction;
    private UnityAction _creditsAction;
    private UnityAction _backAction;

    private UnityAction _retryAction;

    private void Awake()
    {
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        _mainMenuAction = new UnityAction(OnMainMenuButtonClicked);
        _exitAction = new UnityAction(OnExitButtonClicked);
        _creditsAction = new UnityAction(OnCreditsButtonClicked);
        _backAction = new UnityAction(OnBackButtonClicked);

        _retryAction = new UnityAction(OnRetryButtonClicked);

        // Game won
        _mainMenuButtonInGameWon.onClick.AddListener(_mainMenuAction);
        _exitButtonInGameWon.onClick.AddListener(_exitAction);
        _creditsButton.onClick.AddListener(_creditsAction);
        _backInCreditsButton.onClick.AddListener(_backAction);

        // Game over
        _retryButton.onClick.AddListener(_retryAction);
        _mainMenuButtonInGameOver.onClick.AddListener(_mainMenuAction);
        _exitButtonInGameOver.onClick.AddListener(_exitAction);
    }

    private void OnMainMenuButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void OnCreditsButtonClicked()
    {
        _winningScreenCanvas.SetActive(false);
        _creditsCanvas.SetActive(true);
    }
    private void OnBackButtonClicked()
    {
        _winningScreenCanvas.SetActive(true);
        _creditsCanvas.SetActive(false);
    }

    private void OnRetryButtonClicked()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ShowGameWonCanvas()
    {
        _winningScreenCanvas.SetActive(true); 
    }

    public void ShowGameOverCanvas()
    {
        _losingScreenCanvas.SetActive(true);
    }
}