using UnityEngine;
using System.Collections;

public class Crop : MonoBehaviour
{
    [Range(1, 3)] public int growthStage = 1;
    [SerializeField] private int valor = 1;

    [SerializeField] private GameObject stage1;
    [SerializeField] private GameObject stage2;
    [SerializeField] private GameObject stage3;

    [SerializeField] private float growTime = 10f;

    private bool watered = false;
    public int Valor
    {
        get { return valor; }
    }


    public bool IsHarvestable()
    {
        return growthStage >= 3;
    }

    // ========================
    // REGAR
    // ========================

    public void Water()
    {
        if (watered)
        {
            Debug.Log("Ya está regada");
            return;
        }

        if (growthStage >= 3)
        {
            Debug.Log("Ya está completamente crecida");
            return;
        }

        watered = true;
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        Debug.Log("Planta regada, creciendo...");

        yield return new WaitForSeconds(growTime);

        watered = false;

        if (growthStage == 1)
        {
            stage1.SetActive(false);
            stage2.SetActive(true);
            growthStage = 2;
        }
        else if (growthStage == 2)
        {
            stage2.SetActive(false);
            stage3.SetActive(true);
            growthStage = 3;
        }

        Debug.Log("Nueva fase: " + growthStage);
        Debug.Log("Fase actual: " + growthStage + " | Stage3 activo: " + stage3.activeSelf);
    }


    // ========================
    // COSECHAR
    // ========================
    public void Harvest()
    {
        Debug.Log("Bambú cosechado");
        GameManager.instance.sumarBambuVerde(Valor);
        Destroy(gameObject);

    }
}