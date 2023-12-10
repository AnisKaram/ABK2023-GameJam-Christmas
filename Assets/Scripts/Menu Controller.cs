using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        //switch case to manage different scenes
        switch (currentScene) {
            case "Main Menu":
                handleMainMenuInput();
                break;
            case "Splash Screen":
                handleSplashScreenInput();
                break;
            case "GameScene":
                handleGameSceneInput();
                break;
        }
    }
    void handleSplashScreenInput() {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    void handleMainMenuInput() { }
    void handleGameSceneInput() { }

    void Awake()
    {
        // need this script to be persistent in all scenes
        DontDestroyOnLoad(gameObject);
    }

}
