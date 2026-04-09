using System.Collections.Generic;
using UnityEngine;

public class Plant : Interactuable
{
    private FarmingArea area;
    [SerializeField] private List<GameObject> cropslist = new List<GameObject>();
    [SerializeField] private List<GameObject> childrenlist = new List<GameObject>();

    [SerializeField] private GameObject handpoint;
  

    void Awake()
    {
        area = GetComponent<FarmingArea>();
    }

    public override void Interactuar()
    {
        // Verificamos que tengamos la referencia a la parcela
        if (area == null) return;

        //  Si ya hay algo plantado, no hacemos nada (evita solapar cultivos)
        if (area.ThereIsSomething)
        {
            Debug.Log("Ya hay algo creciendo aquí.");
            return;
        }

        //  Detectamos qué saco tiene el jugador en la mano
        GameObject cropselected = ChangePlant();

        if (cropselected == null)
        {
            Debug.Log("No tienes un saco de semillas en la mano");
            return;
        }

        area.SetCropPrefab(cropselected);
        area.sowing();
        Debug.Log("Sembrado correctamente y registrado en el sistema de guardado.");
    }

    private GameObject ChangePlant()
    {
        // Recorre la lista de posibles semillas
        for (int i = 0; i < childrenlist.Count; i++)
        {
            // Si el objeto visual está activo en la mano del jugador
            if (handpoint.transform.Find(childrenlist[i].name))
            {
                // Devolvemos el Prefab del cultivo correspondiente
                return cropslist[i];
            }
        }
        return null;
    }
}

