using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonScript : MonoBehaviour
{
    public Canvas howToPlay;
    public void loadHowToPlay()
    {
        howToPlay.enabled = true;
        GetComponentInParent<Canvas>().enabled = false;
        Debug.Log("Play Pressed");
    }
}
