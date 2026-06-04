using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class FullScreen : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown dropdownresolution;
    Resolution[] resolutions;
    [SerializeField] private AudioSource buttonsound;
    private bool isInitialized = false;
    private void Start()
    {
        if(Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        checkresolutions();
        isInitialized = true;
    }
    public void Fullscreen(bool fullscreen)
    {
        if (isInitialized && buttonsound != null)
        {
            buttonsound.Play();
        }
        Screen.fullScreen = fullscreen;
    }
    public void checkresolutions()
    {
        resolutions = Screen.resolutions;
        dropdownresolution.ClearOptions();
        List<string> options = new List<string>();
        int Currentresolution = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                Currentresolution = i;
            }
        }
        dropdownresolution.AddOptions(options);
        dropdownresolution.value = Currentresolution;
        dropdownresolution.RefreshShownValue();
    }
    public void changeresolution(int resolutionindex)
    {
        Resolution resolution = resolutions[resolutionindex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
