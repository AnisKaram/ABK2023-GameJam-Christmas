using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonScript : MonoBehaviour
{
    public void loadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
        Debug.Log("Pressed");
    }
}
