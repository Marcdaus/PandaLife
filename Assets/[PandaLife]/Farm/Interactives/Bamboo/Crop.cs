using UnityEngine;

public class Crop : MonoBehaviour
{
    [Range(1, 3)] public int growthStage = 1;
    [SerializeField] private int valor = 1;
     public int Valor
    {
        get { return valor; }
    }

    public bool IsHarvestable()
    {
        return growthStage >= 3;
    }

    // Función que destruye el bambú crecido
    public void Harvest()
    {
        Debug.Log("Bambú cosechado");
        GameManager.instance.sumarBambuVerde(Valor);
        Destroy(gameObject);
    }
}