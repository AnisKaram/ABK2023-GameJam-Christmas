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

    private UnityAction mainMenuAction;
    private UnityAction exitAction;

    private void Awake()
    {
        initializeButtons();
    }

    private void initializeButtons() {
        //actions
        mainMenuAction = new UnityAction(onMainMenu);
        exitAction = new UnityAction(onExit);

        //listeners
        MainMenuButton.onClick.AddListener(mainMenuAction);
        ExitButton.onClick.AddListener(exitAction);

    }

    private void onMainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void onExit() {
        Application.Quit();
        Debug.Log("Exit");
    }
}
