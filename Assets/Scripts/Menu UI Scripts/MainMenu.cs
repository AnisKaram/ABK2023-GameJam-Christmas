using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas creditsCanvas;
    public Canvas mainCanvas;
    public Canvas howToPlay;

    public void SwitchToGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void loadHowToPlay() {
        howToPlay.enabled = true;
        mainCanvas.enabled = false;
        Debug.Log("Play Pressed");
    }

    public void Exit()
    {
        Debug.Log("Quit Pressed");
        Application.Quit();
    }

    private void Start()
    {
        creditsCanvas.enabled = false;
        howToPlay.enabled = false;
        mainCanvas.enabled = true;
    }

    private void Update()
    {
        if (creditsCanvas.isActiveAndEnabled)
        {
            if (Input.anyKeyDown)
            {
                creditsCanvas.enabled = false;
                mainCanvas.enabled = true;
            }
        }
        if (howToPlay.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                howToPlay.enabled = false;
                mainCanvas.enabled = true;
            }
        }

    }

    public void LoadCredits()
    {
        creditsCanvas.enabled = true;
        mainCanvas.enabled = false;
        Debug.Log("Load Credits Pressed");
    }
}
