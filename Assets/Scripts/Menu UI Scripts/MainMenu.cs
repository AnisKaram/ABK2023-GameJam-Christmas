using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas creditsCanvas;
    public Canvas mainCanvas;
    public Canvas howToPlay;
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
}
