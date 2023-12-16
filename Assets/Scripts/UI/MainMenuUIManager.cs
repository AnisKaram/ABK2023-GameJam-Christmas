using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject _spashScreenCanvas;
    [SerializeField] private GameObject _howToPlayCanvas;
    [SerializeField] private GameObject _creditsCanvas;

    [Header("Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _howToPlayButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _backInCreditsButton;
    [SerializeField] private Button _backInHowToPlayButton;
    [SerializeField] private Button _startButton;


    private UnityAction _playAction;
    private UnityAction _howToPlayAction;
    private UnityAction _creditsAction;
    private UnityAction _exitAction;
    private UnityAction _backAction;


    private void Awake()
    {
        InputManager.OnAnyKeyPressed += AnyKeyPressed;
        InitializeButtons();
    }

    private void Start()
    {
        EnableDisableSplashScreen(!GameManager.Instance.IsSplashScreenShowed);
    }

    private void OnDestroy()
    {
        InputManager.OnAnyKeyPressed -= AnyKeyPressed;
    }

    private void InitializeButtons()
    {
        _playAction = new UnityAction(OnPlayButtonClicked);
        _howToPlayAction = new UnityAction(OnHowToPlayButtonClicked);
        _creditsAction = new UnityAction(OnCreditsButtonClicked);
        _exitAction = new UnityAction(OnExitButtonClicked);
        _backAction = new UnityAction(OnBackButtonClicked);

        _playButton.onClick.AddListener(_playAction);
        _howToPlayButton.onClick.AddListener(_howToPlayAction);
        _creditsButton.onClick.AddListener(_creditsAction);
        _exitButton.onClick.AddListener(_exitAction);
        _backInCreditsButton.onClick.AddListener(_backAction);
        _backInHowToPlayButton.onClick.AddListener(_backAction);
        _startButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        int gameSceneIndex = 1;
        SceneManager.LoadSceneAsync(gameSceneIndex);
    }

    private void OnHowToPlayButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        _howToPlayCanvas.SetActive(true);
    }

    private void OnCreditsButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        _creditsCanvas.SetActive(true);
    }

    private void OnExitButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        Application.Quit();
    }

    private void OnBackButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");
        if (_creditsCanvas.activeInHierarchy)
        {
            _creditsCanvas.SetActive(false);
            return;
        }
        _howToPlayCanvas.SetActive(false);
    }

    private void EnableDisableSplashScreen(bool showSplashScreen)
    {
        _spashScreenCanvas.SetActive(showSplashScreen);
    }

    private void AnyKeyPressed()
    {
        GameManager.Instance.IsSplashScreenShowed = true;
        _spashScreenCanvas.SetActive(false);
    }
}