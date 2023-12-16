using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance;
    public Sounds[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, playerSource, weaponSource, footStepsSource;
    public AudioClip walkingSFX;
    public AudioClip sprintingSFX;
    [SerializeField] float inGameVolume;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMusic(string name) {
        Sounds mySound = Array.Find(musicSounds, x => x.clipName == name);
        if(mySound != null)
        { 
            musicSource.clip = mySound.clip;
            musicSource.loop = true;
            if (mySound.clipName == "In Game Music") { musicSource.volume = inGameVolume; }
            else { musicSource.volume = 0.5f; }
            musicSource.Play();
        }
        else
        {
            Debug.Log($"Sound \"{name}\" not found (Possible Misspelling?)");
        }
    }

    public void PlaySFX(string name)
    {
        Sounds mySound = Array.Find(sfxSounds, x => x.clipName == name);
        if (mySound != null)
        {
            sfxSource.clip = mySound.clip;
            sfxSource.Play();
        }
        else {
            Debug.Log($"Sound \"{name}\" not found (Possible Misspelling?)");
        }
    }
    public void PlaySFXFromPlayer(string name)
    {
        Sounds mySound = Array.Find(sfxSounds, x => x.clipName == name);
        if (mySound != null)
        {
            playerSource.clip = mySound.clip;
            playerSource.Play();
        }
        else
        {
            Debug.Log($"Sound \"{name}\" not found (Possible Misspelling?)");
        }
    }

    public void PlaySFXFromGun(string name)
    {
        Sounds mySound = Array.Find(sfxSounds, x => x.clipName == name);
        if (mySound != null)
        {
            weaponSource.clip = mySound.clip;
            weaponSource.Play();
        }
        else
        {
            Debug.Log($"Sound \"{name}\" not found (Possible Misspelling?)");
        }
    }

    public void walkSFX() {
        if (footStepsSource.clip == null || footStepsSource.clip == sprintingSFX) {
            footStepsSource.clip = walkingSFX;
            footStepsSource.Play();
        }
    }
    public void sprintSFX() {
        if (footStepsSource.clip == null || footStepsSource.clip == walkingSFX)
        {
            footStepsSource.clip = sprintingSFX;
            footStepsSource.Play();
        }
    }
    public void standStill() {
        footStepsSource.clip = null;
    }
}
