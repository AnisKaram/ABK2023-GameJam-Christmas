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
        GameAudioManager.Instance.PlaySFX("Button Click");
        GameAudioManager.Instance.PlayMusic("Main Menu Music");
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void OnCreditsButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        _winningScreenCanvas.SetActive(false);
        _creditsCanvas.SetActive(true);
    }
    private void OnBackButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        _winningScreenCanvas.SetActive(true);
        _creditsCanvas.SetActive(false);
    }

    private void OnRetryButtonClicked()
    {
        GameAudioManager.Instance.PlayMusic("In Game Music");
        GameAudioManager.Instance.PlaySFX("Button Click");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ShowGameWonCanvas()
    {
        GameAudioManager.Instance.PlayMusic("Winning Music");
        _winningScreenCanvas.SetActive(true); 
    }

    public void ShowGameOverCanvas()
    {
        GameAudioManager.Instance.PlayMusic("Losing Music");
        _losingScreenCanvas.SetActive(true);
    }
}