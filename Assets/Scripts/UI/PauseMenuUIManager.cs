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
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button BackInSettingsButton;
    [SerializeField] private Button HowToPlayButton;
    [SerializeField] private Button BackInHowToPlayButton;

    [SerializeField] GameObject PauseMenuCanvas;
    [SerializeField] GameObject HowToPlayCanvas;

    private UnityAction mainMenuAction;
    private UnityAction exitAction;
    private UnityAction resumeAction;
    private UnityAction settingsAction;
    private UnityAction backAction;
    private UnityAction howToPlayAction;

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
        settingsAction = new UnityAction(OnSettingsButtonClicked);
        backAction = new UnityAction(OnBackButtonClicked);
        howToPlayAction = new UnityAction(OnHowToPlayButtonClicked);

        //listeners
        MainMenuButton.onClick.AddListener(mainMenuAction);
        ExitButton.onClick.AddListener(exitAction);
        ResumeButton.onClick.AddListener(resumeAction);
        SettingsButton.onClick.AddListener(settingsAction);
        BackInSettingsButton.onClick.AddListener(backAction);
        HowToPlayButton.onClick.AddListener(howToPlayAction);
        BackInHowToPlayButton.onClick.AddListener(backAction);
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
    }

    private void OnSettingsButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        _settingsCanvas.SetActive(true);
    }

    private void OnBackButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");

        if (_settingsCanvas.activeInHierarchy)
        {
            _settingsCanvas.SetActive(false);
            return;
        }
        HowToPlayCanvas.SetActive(false);
    }

    private void OnHowToPlayButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        HowToPlayCanvas.SetActive(true);
    }
}
