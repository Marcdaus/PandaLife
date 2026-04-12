using System;
using System.Collections.Generic;
using UnityEngine;

public class CauldronPersistenceManager : MonoBehaviour
{
    public static CauldronPersistenceManager instance { get; private set; }

    // Estado de cocción (sin cambios)
    public bool isCooking { get; private set; }
    public RecipesData cookingRecipe { get; private set; }
    public DateTime cookingStartTime { get; private set; }

    //  Platos pendientes (ahora es una lista) 
    [System.Serializable]
    public class DishState
    {
        public RecipesData recipe;
        public Vector3 position;
        public bool wasInHand;
    }

    private List<DishState> pendingDishes = new List<DishState>();

    // Compatibilidad con código existente que lea hasPendingDish
    public bool hasPendingDish => pendingDishes.Count > 0;

    // Si algún código legacy aún lee estas propiedades, apuntan al primer plato
    public RecipesData pendingDishRecipe => pendingDishes.Count > 0 ? pendingDishes[0].recipe : null;
    public Vector3 pendingDishPosition => pendingDishes.Count > 0 ? pendingDishes[0].position : Vector3.zero;
    public bool dishWasInHand => pendingDishes.Count > 0 && pendingDishes[0].wasInHand;

    public IReadOnlyList<DishState> PendingDishes => pendingDishes;

    // 

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Cocción
    public void SaveCookingState(RecipesData recipe, float tiempoTranscurrido)
    {
        isCooking = true;
        cookingRecipe = recipe;
        cookingStartTime = DateTime.UtcNow - TimeSpan.FromSeconds(tiempoTranscurrido);
    }

    public void ClearCookingState()
    {
        isCooking = false;
        cookingRecipe = null;
    }

    public float GetElapsedCookingTime()
    {
        if (!isCooking) return 0f;
        return (float)(DateTime.UtcNow - cookingStartTime).TotalSeconds;
    }

    // Platos , guardar AÑADE a la lista, no reemplaza
    public void SaveDishState(RecipesData recipe, Vector3 position, bool inHand)
    {
        pendingDishes.Add(new DishState
        {
            recipe = recipe,
            position = position,
            wasInHand = inHand
        });
    }

    // Limpia TODOS los platos (úsalo solo al terminar de restaurarlos todos)
    public void ClearAllDishStates()
    {
        pendingDishes.Clear();
    }

    // Limpia solo el primero (para compatibilidad con Cauldron.RestoreFromPersistence)
    public void ClearDishState()
    {
        if (pendingDishes.Count > 0)
            pendingDishes.RemoveAt(0);
    }
}