using System;
using UnityEngine;

public class CauldronPersistenceManager : MonoBehaviour
{
    public static CauldronPersistenceManager instance { get; private set; }

    // Estado de cocción
    public bool isCooking { get; private set; }
    public RecipesData cookingRecipe { get; private set; }
    public DateTime cookingStartTime { get; private set; }

    // Estado del plato pendiente
    public bool hasPendingDish { get; private set; }
    public RecipesData pendingDishRecipe { get; private set; }
    public Vector3 pendingDishPosition { get; private set; }
    public bool dishWasInHand { get; private set; } // si estaba en mano cae al suelo

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveCookingState(RecipesData recipe, float tiempoTranscurrido)
    {
        isCooking = true;
        cookingRecipe = recipe;
        // Guardamos el momento real de inicio hacia atrás
        cookingStartTime = DateTime.UtcNow - TimeSpan.FromSeconds(tiempoTranscurrido);
    }

    public void ClearCookingState()
    {
        isCooking = false;
        cookingRecipe = null;
    }

    public void SaveDishState(RecipesData recipe, Vector3 position, bool inHand)
    {
        hasPendingDish = true;
        pendingDishRecipe = recipe;
        pendingDishPosition = position;
        dishWasInHand = inHand;
    }

    public void ClearDishState()
    {
        hasPendingDish = false;
        pendingDishRecipe = null;
    }

    // Devuelve cuánto tiempo ha pasado desde que empezó a cocinar (en segundos)
    public float GetElapsedCookingTime()
    {
        if (!isCooking) return 0f;
        return (float)(DateTime.UtcNow - cookingStartTime).TotalSeconds;
    }
}