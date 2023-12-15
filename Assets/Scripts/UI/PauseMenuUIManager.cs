using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUIManager : MonoBehaviour
{
    // buttons
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button ResumeButton;
    [SerializeField] GameObject PauseMenuCanvas;

    private UnityAction mainMenuAction;
    private UnityAction exitAction;
    private UnityAction resumeAction;

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
        if (!PauseMenuCanvas.activeInHierarchy) {
            onPause();
        }
        else
        {
            onResume();
        }
    }

    private void initializeButtons() {
        //actions
        mainMenuAction = new UnityAction(onMainMenu);
        exitAction = new UnityAction(onExit);
        resumeAction = new UnityAction(onResume);

        //listeners
        MainMenuButton.onClick.AddListener(mainMenuAction);
        ExitButton.onClick.AddListener(exitAction);
        ResumeButton.onClick.AddListener(resumeAction);

    }

    private void onMainMenu() {
        GameManager.Instance.StartGame();
        SceneManager.LoadScene("MainMenu");
        GameAudioManager.Instance.PlaySFX("Button Click");
    }

    private void onExit() {
        Application.Quit();
        Debug.Log("Exit");
        GameAudioManager.Instance.PlaySFX("Button Click");
    }

    private void onResume() {

        //resume game
        GameManager.Instance.StartGame();
        InputManager.Instance.Controls.Gameplay.Enable();
        PauseMenuCanvas.SetActive(false);
        GameManager.Instance.LockCursor();
        GameAudioManager.Instance.PlaySFX("Button Click");
    }

    private void onPause()
    {
        //pause game
        Debug.Log("pause");
        InputManager.Instance.Controls.Gameplay.Disable();
        GameManager.Instance.StopGame();
        GameManager.Instance.ConfineCursor();
        PauseMenuCanvas.SetActive(true);
        GameAudioManager.Instance.PlaySFX("Button Click");
    }
}
