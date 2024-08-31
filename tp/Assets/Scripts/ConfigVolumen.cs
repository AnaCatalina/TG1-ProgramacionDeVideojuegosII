using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfigVolumen : MonoBehaviour
{
    public AudioMixer mixter;
    public Slider sliderMusica;
    public Slider sliderAudio;

    public void setMusicVolumen()
    {
        float volume = sliderMusica.value;
        mixter.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void setAudioVolumen()
    {
        float volume = sliderAudio.value;
        mixter.SetFloat("AudioVolume", Mathf.Log10(volume) * 20);
    }
}
