using System.Collections.Generic;
using UnityEngine;

public class PandaRequest : MonoBehaviour
{
    [Header("Pedidos")]
    [SerializeField] private List<string> UnlockedCrops = new List<string>();

    // Esta será la "memoria" de los pedidos actuales
    private List<string> currentActiveRequests = new List<string>();

    private void Awake()
    {
        if (!UnlockedCrops.Contains("Bamboo")) UnlockedCrops.Add("Bamboo");
        if (!UnlockedCrops.Contains("Blueberry")) UnlockedCrops.Add("Blueberry");

        // Generamos los primeros pedidos al empezar el juego por primera vez
        GenerateRandomRequests();
    }

    public void UnlockCropsForDay(int dayNumber)
    {
        if (dayNumber == 2)
        {
            if (!UnlockedCrops.Contains("RedDragon")) UnlockedCrops.Add("RedDragon");
        }
        if (dayNumber == 3)
        {
            if (!UnlockedCrops.Contains("Uchuva")) UnlockedCrops.Add("Uchuva");
        }
    }

    // Esta función ahora GUARDA los resultados en la memoria
    public void GenerateRandomRequests()
    {
        currentActiveRequests.Clear();
        if (UnlockedCrops.Count == 0) return;

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, UnlockedCrops.Count);
            currentActiveRequests.Add(UnlockedCrops[randomIndex]);
        }
    }

    // Nueva función para que otros scripts solo LEAN los pedidos sin cambiarlos
    public List<string> GetCurrentRequests()
    {
        // Si por alguna razón está vacía, generamos unos
        if (currentActiveRequests.Count == 0) GenerateRandomRequests();
        return currentActiveRequests;
    }
}