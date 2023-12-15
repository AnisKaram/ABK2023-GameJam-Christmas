using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance;
    public Sounds[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name) {
        Sounds mySound = Array.Find(sfxSounds, x => x.clipName == name);
        musicSource.clip = mySound.clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sounds mySound = Array.Find(musicSounds, x => x.clipName == name);
        sfxSource.clip = mySound.clip;
        sfxSource.PlayOneShot(sfxSource.clip);
    }
}
