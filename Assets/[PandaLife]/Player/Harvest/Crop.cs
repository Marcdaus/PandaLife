using UnityEngine;

public class Crop : MonoBehaviour
{
    [Range(1, 3)] public int growthStage = 1;

    public bool IsHarvestable()
    {
        return growthStage >= 3;
    }

    // Función que destruye el bambú crecido
    public void Harvest()
    {
        Debug.Log("Bambú cosechado");
        Destroy(gameObject);
    }
}