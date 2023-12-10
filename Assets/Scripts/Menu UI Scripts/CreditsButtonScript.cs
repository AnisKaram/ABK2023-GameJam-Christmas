using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButtonScript : MonoBehaviour
{
    public Canvas creditsCanvas;
    public void LoadCredits()
    {
        creditsCanvas.enabled = true;
        GetComponentInParent<Canvas>().enabled = false;
        Debug.Log("Load Credits Pressed");
    }
}
