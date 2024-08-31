using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public AudioSource musica;
    public AudioSource efectos;
    public static AudioManager instance;
    public AudioSource[] soundEffects;

    private void Awake() {
        instance = this;
    }

    public void Reproducir(int soundPlay)
    {
        soundEffects[soundPlay].Play();
    }

    public void Pausar(int soundPlay)
    {
        soundEffects[soundPlay].Pause();
    }
}
