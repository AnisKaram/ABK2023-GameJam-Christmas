using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    // buttons
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _backInSettingsButton;
    [SerializeField] GameObject PauseMenuCanvas;

    private UnityAction mainMenuAction;
    private UnityAction exitAction;
    private UnityAction resumeAction;
    private UnityAction _settingsAction;
    private UnityAction _backAction;

    private void Awake()
    {
        initializeButtons();
        InputManager.OnPauseTriggered += InputManager_OnPauseTriggered; 
    }
    private void OnDestroy()
    {
        InputManager.OnPauseTriggered -= InputManager_OnPauseTriggered;
    }

    private void InputManager_OnPauseTriggered()
    {
        if (!PauseMenuCanvas.activeInHierarchy)
        {
            onPause();
        }
        else
        {
            onResume(isButtonClicked: false);
        }
    }

    private void initializeButtons() {
        //actions
        mainMenuAction = new UnityAction(onMainMenu);
        exitAction = new UnityAction(onExit);
        resumeAction = new UnityAction(() => { onResume(isButtonClicked: true); });
        _settingsAction = new UnityAction(OnSettingsButtonClicked);
        _backAction = new UnityAction(OnBackButtonClicked);

        //listeners
        MainMenuButton.onClick.AddListener(mainMenuAction);
        ExitButton.onClick.AddListener(exitAction);
        ResumeButton.onClick.AddListener(resumeAction);
        _settingsButton.onClick.AddListener(_settingsAction);
        _backInSettingsButton.onClick.AddListener(_backAction);
    }

    private void onMainMenu() {
        GameManager.Instance.StartGame();
        GameAudioManager.Instance.PlaySFX("Button Click");
        SceneManager.LoadScene("MainMenu");
    }

    private void onExit() {
        GameAudioManager.Instance.PlaySFX("Button Click");
        Application.Quit();
    }

    private void onResume(bool isButtonClicked) {

        //resume game
        GameManager.Instance.StartGame();
        InputManager.Instance.Controls.Gameplay.Enable();
        PauseMenuCanvas.SetActive(false);
        GameManager.Instance.LockCursor();

        if (isButtonClicked)
        {
            GameAudioManager.Instance.PlaySFX("Button Click");
        }
    }

    private void onPause()
    {
        InputManager.Instance.Controls.Gameplay.Disable();
        GameManager.Instance.StopGame();
        GameManager.Instance.ConfineCursor();
        PauseMenuCanvas.SetActive(true);
        //GameAudioManager.Instance.PlaySFX("Button Click");
    }

    private void OnSettingsButtonClicked()
    {
        _settingsCanvas.SetActive(true);
    }

    private void OnBackButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        _settingsCanvas.SetActive(false);
    }
}
