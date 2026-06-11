using UnityEngine;
using System;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    // Pasos tutorial
    public enum TutorialStep
    {
        SalirDeCasa = 0, //
        CogerSaco = 1,//
        Plantar = 2,//
        Plantar2 = 3,//
        CogerCubo = 4,//
        Flecha1 = 5,//
        LlenarCubo = 6,//
        Flecha2 = 7,//
        RegarPlanta = 8,//
        Cosechar = 9,
        EntrarEnCasa = 10,//
        Caldero = 11,
        Completado = 12 // Tutorial acabado
    }
    public enum TutorialRecipeBook
    {
        OpenRecipeBook = 0,
        CloseRecipeBook = 1,
        FirstDay = 2, 
        OpenRedDragon = 3,
        CloseRedDragon = 4,
        SecondDay = 5,
        OpenUchuva = 6,
        CloseUchuva = 7,
        Completado = 8
    }

    

    public TutorialStep currentStep = TutorialStep.SalirDeCasa;
    public TutorialRecipeBook currentStepRecipe = TutorialRecipeBook.FirstDay;


    // Evento para avanzar el tutorial sin usar el Update()
    public event Action OnTutorialAdvanced;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteStep(TutorialStep stepToComplete)
    {
        // Solo avanzamos si el paso que intentamos completar es el paso actual
        if (currentStep == stepToComplete)
        {
            currentStep++; // Pasamos al siguiente
            Debug.Log($"[TutorialManager] Avanzado a: {currentStep}");

            // Avisamos a los pines
            OnTutorialAdvanced?.Invoke();
        }
    }
    public void CompleteRecipeStep(TutorialRecipeBook stepToComplete)
    {
        // Solo avanzamos si el paso que intentamos completar es el paso actual
        if (currentStepRecipe == stepToComplete)
        {
            currentStepRecipe++; // Pasamos al siguiente
            Debug.Log($"[TutorialManager] Avanzado a: {currentStepRecipe}");

            // Avisamos a los pines
            OnTutorialAdvanced?.Invoke();
        }
    }

    public void ResetTutorial()
    {
        currentStep = TutorialStep.SalirDeCasa;
        currentStepRecipe = TutorialRecipeBook.FirstDay;
        OnTutorialAdvanced?.Invoke();
    }
}