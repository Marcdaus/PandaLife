using System.Collections.Generic;
using UnityEngine;

public class PandaRequest : MonoBehaviour
{
    [Header("Pedidos de platos")]
    [SerializeField] private List<string> UnlockedDishes = new List<string>();

    // Esta será la "memoria" de los pedidos actuales
    private List<string> currentActiveRequests = new List<string>();

    private void Awake()
    {

        if (!UnlockedDishes.Contains("Plato_BambuCocido")) UnlockedDishes.Add("Plato_BambuCocido");
        if (!UnlockedDishes.Contains("Plato_BambuRelleno")) UnlockedDishes.Add("Plato_BambuRelleno");

        // Generamos los primeros pedidos al empezar el juego por primera vez
        GenerateRandomRequests();
    }

    public void UnlockDishesForDay(int dayNumber)
    {
        if (dayNumber == 2)
        {
            if (!UnlockedDishes.Contains("Plato_Ensalada")) UnlockedDishes.Add("Plato_Ensalada");
        }
        if (dayNumber == 3)
        {
            if (!UnlockedDishes.Contains("Plato_Sopa")) UnlockedDishes.Add("Plato_Sopa");
            if (!UnlockedDishes.Contains("Plato_BobaTea")) UnlockedDishes.Add("Plato_BobaTea");
        }
    }

    // Esta función ahora GUARDA los resultados en la memoria
    public void GenerateRandomRequests()
    {
        currentActiveRequests.Clear();
        if (UnlockedDishes.Count == 0) return;

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, UnlockedDishes.Count);
            currentActiveRequests.Add(UnlockedDishes[randomIndex]);
        }
    }

    // Nueva función para que otros scripts solo LEAN los pedidos sin cambiarlos
    public List<string> GetCurrentRequests()
    {
        // Si por alguna razón está vacía, generamos unos
        if (currentActiveRequests.Count == 0) GenerateRandomRequests();
        return currentActiveRequests;
    }
    public void ClearList()
    {
        UnlockedDishes.Clear();
        if (!UnlockedDishes.Contains("Plato_BambuCocido")) UnlockedDishes.Add("Plato_BambuCocido");
        if (!UnlockedDishes.Contains("Plato_BambuRelleno")) UnlockedDishes.Add("Plato_BambuRelleno");
        GenerateRandomRequests();
    }

    // Genera un nuevo plato cada vez q le damos un plato
    public void ReplaceRequestAtIndex(int index)
    {
        if (UnlockedDishes.Count == 0) return;

        // Comprobamos que el índice sea válido
        if (index >= 0 && index < currentActiveRequests.Count)
        {
            int randomIndex = Random.Range(0, UnlockedDishes.Count);
            currentActiveRequests[index] = UnlockedDishes[randomIndex];
            Debug.Log($"El pedido {index} ha sido actualizado a: {currentActiveRequests[index]}");
        }
    }
}