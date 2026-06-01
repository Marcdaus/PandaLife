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
        CogerCubo = 3,//
        LlenarCubo = 4,//
        RegarPlanta = 5,//
        Cosechar = 6,
        EntrarEnCasa = 7,//
        Caldero = 8,
        Completado = 9 // Tutorial acabado
    }
    public enum TutorialRecipeBook
    {
        FirstDay = 0, 
        OpenRedDragon = 1,
        CloseRedDragon = 2,
        SecondDay = 3,
        OpenUchuva = 4,
        CloseUchuva = 5,
        Completado = 6
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