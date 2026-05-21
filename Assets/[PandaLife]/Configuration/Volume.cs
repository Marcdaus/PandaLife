using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    [Header("Sliders")]
    public Slider volumeSlider;
    public Slider SFXSlider;
    public Slider BGMSlider;

    [Header("Imágenes de Mute")]
    public Image volumeMute;
    public Image SFXMute;
    public Image BGMMute;

    [Header("Mixer")]
    public AudioMixer mixer;

    void Start()
    {
        // GUARDAR ESTADOS
        float masterVal = PlayerPrefs.GetFloat("volumenMaster", 1f);
        float sfxVal = PlayerPrefs.GetFloat("volumenSFX", 1f);
        float bgmVal = PlayerPrefs.GetFloat("volumenBGM", 1f);

       
        volumeSlider.value = masterVal;
        SFXSlider.value = sfxVal;
        BGMSlider.value = bgmVal;

       
        SetMixerVolume("Master", masterVal, volumeMute);
        SetMixerVolume("SFX", sfxVal, SFXMute);
        SetMixerVolume("BGM", bgmVal, BGMMute);
    }

    // CONTROLADORES DE VOLUMEN

    public void ChangeSlider(float valor)
    {
        SetMixerVolume("Master", valor, volumeMute);
        PlayerPrefs.SetFloat("volumenMaster", valor); 
    }

    public void Volumesfx(float valor)
    {
        SetMixerVolume("SFX", valor, SFXMute);
        PlayerPrefs.SetFloat("volumenSFX", valor);
    }

    public void Volumebgm(float valor)
    {
        SetMixerVolume("BGM", valor, BGMMute);
        PlayerPrefs.SetFloat("volumenBGM", valor);
    }

    // FUNCION PARA CONTROLAR TODAS LAS BARRAS
    private void SetMixerVolume(string parameterName, float sliderValue, Image muteImage)
    {
        if (sliderValue <= 0.001f) 
        {
            mixer.SetFloat(parameterName, -80f); 
            if (muteImage != null) muteImage.enabled = true;
        }
        else
        {
            
            float dB = Mathf.Log10(sliderValue) * 20f;
            mixer.SetFloat(parameterName, dB);
            if (muteImage != null) muteImage.enabled = false;
        }
    }
}