using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class WinningScreenUI : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button CreditsButton;
    [SerializeField] private Button BackInCreditsButton;
    [SerializeField] GameObject WinningScreenCanvas;
    [SerializeField] GameObject CreditsCanvas;

    private UnityAction mainMenuAction;
    private UnityAction exitAction;
    private UnityAction creditsAction;
    private UnityAction backAction;

    private void Awake()
    {
        InitializeButtons();
    }
    private void InitializeButtons() {
        mainMenuAction = new UnityAction(onMainMenu);
        exitAction = new UnityAction(onExit);
        creditsAction = new UnityAction(onCredits);
        backAction = new UnityAction(onBack);   

        MainMenuButton.onClick.AddListener(mainMenuAction);
        ExitButton.onClick.AddListener(exitAction);
        CreditsButton.onClick.AddListener(creditsAction);
        BackInCreditsButton.onClick.AddListener(onBack);
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
    private void onCredits() {
        CreditsCanvas.SetActive(true);
    }

    private void onBack() {
        CreditsCanvas.SetActive(false);
    }
}
