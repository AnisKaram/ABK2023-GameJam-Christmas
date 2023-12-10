using UnityEngine;

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
    }
    #endregion
}