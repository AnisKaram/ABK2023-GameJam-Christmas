using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance;
    public Sounds[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, playerSource, weaponSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //DEBUG
        for (int i = 0; i < sfxSounds.Length; i++) {
            Debug.Log(sfxSounds[i].clipName + "\n");
        }
    }

    public void PlayMusic(string name) {
        Sounds mySound = Array.Find(musicSounds, x => x.clipName == name);
        if(mySound != null)
        {
            musicSource.clip = mySound.clip;
            musicSource.loop = true;
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
}
