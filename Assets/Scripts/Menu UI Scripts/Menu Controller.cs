using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    private GameObject pauseMenuInstance;
    // Update is called once per frame
    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        //switch case to manage different scenes
        switch (currentScene)
        {
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
    void handleGameSceneInput() {
        if (Input.GetKey(KeyCode.F3)) {
            if (pauseMenuInstance == null)
            {
                //stop time
                Time.timeScale = 0;
                pauseMenuInstance = Instantiate(PauseMenu);
            }
            else {
                //resume
                Time.timeScale = 1;
                Destroy(pauseMenuInstance); 
            }
        }
    }

    void Awake()
    {
        // need this script to be persistent in all scenes
        DontDestroyOnLoad(gameObject);
    }

}
