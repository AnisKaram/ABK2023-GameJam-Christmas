using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsSaverLoader : MonoBehaviour
{
    #region Fields
    [Header("Scriptable Objects")]
    [SerializeField] private PlayerSettings _playerSettings;

    [Header("Sliders")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _sensitivitySlider;

    [Header("Buttons")]
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _defaultButton;

    private UnityAction<float> _musicAction;
    private UnityAction<float> _sfxAction;
    private UnityAction<float> _sensitivityAction;
    private UnityAction _saveAction;
    private UnityAction _resetAction;
    #endregion

    #region Events
    public static event UnityAction OnSettingsSaved;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Music
        float musicVolume = GetSavedMusicValue();
        _musicSlider.value = musicVolume;
        _playerSettings.musicVolume = musicVolume;

        // Sfx
        float sfxVolume = GetSavedSfxValue();
        _sfxSlider.value = sfxVolume;
        _playerSettings.sfxVolume = sfxVolume;

        // Sensitivity
        float sensitivity = GetSavedSensitivity();
        _sensitivitySlider.value = sensitivity;
        _playerSettings.sensitivity = sensitivity;
    }

    private void Start()
    {
        InitializeActions();
    }
    #endregion

    #region Private Methods
    private void InitializeActions()
    {
        _musicAction = new UnityAction<float>(OnMusicValueChanged);
        _musicSlider.onValueChanged.AddListener(_musicAction);

        _sfxAction = new UnityAction<float>(OnSfxValueChanged);
        _sfxSlider.onValueChanged.AddListener(_sfxAction);

        _sensitivityAction = new UnityAction<float>(OnSensitivityChanged);
        _sensitivitySlider.onValueChanged.AddListener(_sensitivityAction);

        _saveAction = new UnityAction(OnSaveButtonClicked);
        _resetAction = new UnityAction(OnResetButtonClicked);
        _saveButton.onClick.AddListener(_saveAction);
        _defaultButton.onClick.AddListener(_resetAction);
    }

    private void OnMusicValueChanged(float sliderVal)
    {
        _playerSettings.musicVolume = sliderVal;
        GameAudioManager.Instance.ChangeMusicSourceVolume(sliderVal);
    }

    private void OnSfxValueChanged(float sliderVal)
    {
        _playerSettings.sfxVolume = sliderVal;
        GameAudioManager.Instance.ChangeSoundEffectsSourceVolume(sliderVal);
    }

    private void OnSensitivityChanged(float sliderVal)
    {
        _playerSettings.sensitivity = sliderVal;
    }

    private float GetSavedMusicValue()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            return PlayerPrefs.GetFloat("music");
        }
        return 0.5f;
    }

    private float GetSavedSfxValue()
    {
        if (PlayerPrefs.HasKey("sfx"))
        {
            return PlayerPrefs.GetFloat("sfx");
        }
        return 1f;
    }

    private float GetSavedSensitivity()
    {
        if (PlayerPrefs.HasKey("sensitivity"))
        {
            return PlayerPrefs.GetFloat("sensitivity");
        }
        return 0.5f;
    }

    private void OnSaveButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");

        PlayerPrefs.SetFloat("music", _playerSettings.musicVolume);
        PlayerPrefs.SetFloat("sfx", _playerSettings.sfxVolume);
        PlayerPrefs.SetFloat("sensitivity", _playerSettings.sensitivity);
        PlayerPrefs.Save();

        OnSettingsSaved?.Invoke();
    }

    private void OnResetButtonClicked()
    {
        GameAudioManager.Instance.PlaySFX("Button Click");

        PlayerPrefs.SetFloat("music", 0.5f);
        PlayerPrefs.SetFloat("sfx", 1f);
        PlayerPrefs.SetFloat("sensitivity", 0.5f);
        PlayerPrefs.Save();

        OnMusicValueChanged(0.5f);
        OnSfxValueChanged(1f);
        OnSensitivityChanged(0.5f);

        _musicSlider.value = 0.5f;
        _sfxSlider.value = 1f;
        _sensitivitySlider.value = 0.5f;
    }
    #endregion
}