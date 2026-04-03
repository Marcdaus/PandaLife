using UnityEngine;

public static class PlacesManager
{
    //Variables
    private static Places[] places;

    // Inicializa los lugares a partir de los puntos dados
    public static void Initialize(Transform[] points)
    {
        places = new Places[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            places[i] = new Places(points[i]);
        }
    }

    // Devuelve un lugar libre aleatorio y lo marca como ocupado
    public static Places GetNextFreePlace()
    {
        // Cuenta los libres
        int freeCount = 0;
        foreach (Places p in places)
            if (!p.Occupied) freeCount++;

        if (freeCount == 0)
        {
            // Todos ocupados: devuelve uno aleatorio
            int index = Random.Range(0, places.Length);
            return places[index];
        }

        // Escoger aleatorio entre los libres
        Places chosen;
        do
        {
            int index = Random.Range(0, places.Length);
            chosen = places[index];
        } while (chosen.Occupied);

        chosen.Occupied = true;
        return chosen;
    }

    // Desocupa el lugar
    public static void FreePlace(Places place)
    {
        place.Occupied = false;
    }
}