using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Fields
    private static GameManager _instance;

    private bool _isSplashScreenShowed;
    #endregion

    #region Properties
    public static GameManager Instance
    {
        get { return _instance; }
    }

    public bool IsSplashScreenShowed
    {
        get { return _isSplashScreenShowed; }
        set { _isSplashScreenShowed = value; }
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        StartGame();
    }

    private void SceneManager_activeSceneChanged(Scene prevScene, Scene currScene)
    {
        if (currScene.name == "GameScene")
        {
            GameAudioManager.Instance.PlayMusic("In Game Music");
            return;
        }
        GameAudioManager.Instance.PlayMusic("Main Menu Music");
    }
    #endregion

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }

    public void ConfineCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}