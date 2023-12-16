using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LosingScreenUI : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button RetryButton;
    [SerializeField] GameObject LosingScreenCanvas;

    private UnityAction mainMenuAction;
    private UnityAction exitAction;
    private UnityAction retryAction;

    private void Awake()
    {
        InitializeButtons();
    }
    private void InitializeButtons() {
        mainMenuAction = new UnityAction(onMainMenu);
        exitAction = new UnityAction(onExit);
        retryAction = new UnityAction(onRetry);   

        MainMenuButton.onClick.AddListener(mainMenuAction);
        ExitButton.onClick.AddListener(exitAction);
        RetryButton.onClick.AddListener(onRetry);
    }

    private void onMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    private void onExit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
    private void onRetry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
