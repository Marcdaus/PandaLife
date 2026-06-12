using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FullScreen : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown dropdownresolution;

    private Resolution[] filteredResolutions;
    [SerializeField] private AudioSource buttonsound;
    private bool isInitialized = false;

    private void Start()
    {
       
        toggle.isOn = Screen.fullScreen;

        
        checkresolutions();

        
        isInitialized = true;
    }

    public void Fullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;

        // Solo suena si el juego ya terminó de iniciar y el usuario interactuó
        if (isInitialized && buttonsound != null)
        {
            buttonsound.Play();
        }
    }

    public void checkresolutions()
    {
        Resolution[] allResolutions = Screen.resolutions;
        dropdownresolution.ClearOptions();

        List<string> options = new List<string>();
        List<Resolution> uniqueResolutions = new List<Resolution>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < allResolutions.Length; i++)
        {
            // Creamos el texto de la opción
            string option = allResolutions[i].width + " x " + allResolutions[i].height;

            // Si la lista de opciones no contiene este tamaño, lo añadimos (así evitamos duplicados por Hz)
            if (!options.Contains(option))
            {
                options.Add(option);
                uniqueResolutions.Add(allResolutions[i]);

                // Guardamos el índice si coincide con la resolución actual de la pantalla
                if (allResolutions[i].width == Screen.currentResolution.width &&
                    allResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = uniqueResolutions.Count - 1;
                }
            }
        }

   
        filteredResolutions = uniqueResolutions.ToArray();

        dropdownresolution.AddOptions(options);
        dropdownresolution.value = currentResolutionIndex;
        dropdownresolution.RefreshShownValue();
    }

    public void changeresolution(int resolutionindex)
    {
        if (resolutionindex < 0 || resolutionindex >= filteredResolutions.Length) return;

        Resolution resolution = filteredResolutions[resolutionindex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        if (isInitialized && buttonsound != null)
        {
            buttonsound.Play();
        }
    }
}