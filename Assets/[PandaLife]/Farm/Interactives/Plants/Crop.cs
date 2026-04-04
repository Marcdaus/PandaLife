using UnityEngine;
using System.Collections;

public class Crop : MonoBehaviour
{
    [Range(1, 3)] public int growthstage = 1;
    [SerializeField] private int valor = 1;

    [SerializeField] private GameObject stage1;
    [SerializeField] private GameObject stage2;
    [SerializeField] private GameObject stage3;
    
    [SerializeField] private float growtime = 10f;
    [SerializeField] private int Type;

    private bool watered = false;
    public bool IsWatered { get { return watered; } }
    public int Valor
    {
        get { return valor; }
    }

    public bool IsHarvestable()
    {
        return growthstage >= 3;
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

        if (growthstage >= 3)
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

        yield return new WaitForSeconds(growtime);

        watered = false;

        if (growthstage == 1)
        {
            stage1.SetActive(false);
            stage2.SetActive(true);
            growthstage = 2;
        }
        else if (growthstage == 2)
        {
            stage2.SetActive(false);
            stage3.SetActive(true);
            growthstage = 3;
        }

        Debug.Log("Nueva fase: " + growthstage);
        Debug.Log("Fase actual: " + growthstage + " | Stage3 activo: " + stage3.activeSelf);
    }

    // ========================
    // COSECHAR
    // ========================
    public void Harvest()
    {
        Debug.Log("Bambú cosechado");
        GameManager.instance.sumarBambu(Valor,Type);
        Destroy(gameObject);
    }
}
