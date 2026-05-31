using System.Collections.Generic;
using UnityEngine;

public class Plant : Interactuable
{
    private FarmingArea area;
    [SerializeField] private List<GameObject> cropslist = new List<GameObject>();
    [SerializeField] private List<GameObject> childrenlist = new List<GameObject>();

    void Awake()
    {
        area = GetComponent<FarmingArea>();
    }

    public override bool ShouldShakeHead(Player player)
    {
        // Si el jugador tiene las manos vacías
        if (player.IsHandEmpty()) return true;

        // Si ya hay algo plantado
        if (area != null && area.ThereIsSomething) return true;

        return false;
    }

    public override void Interactuar(Player player)
    {
        if (area == null) return;

        if (area.ThereIsSomething)
        {
            Debug.Log("Ya hay algo creciendo aquí.");
            return;
        }

        // Comprueba qué tiene en la mano
        GameObject cropselected = ChangePlant(player);

        if (cropselected == null)
        {
            Debug.Log("No tienes un saco de semillas en la mano");
            return;
        }

        area.SetCropPrefab(cropselected);
        area.sowing();
        // Si el tutorial está activo y esto es una parcela, completamos el paso
        if (TutorialManager.instance != null && GetComponent<FarmingArea>() != null)
        {
            TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.Plantar);
        }
        Debug.Log("Sembrado correctamente y registrado en el sistema de guardado.");
    }

    // Actualizamos ChangePlant para leer el inventario del jugador directamente
    private GameObject ChangePlant(Player player)
    {
        // Si la mano está vacía o no hay objeto recogido, devolvemos null
        if (player.IsHandEmpty() || player.pickedobject == null) return null;

        for (int i = 0; i < childrenlist.Count; i++)
        {
            // Usamos .Contains por si Unity le ha añadido "(Clone)" al nombre del objeto
            if (player.pickedobject.gameObject.name.Contains(childrenlist[i].name))
            {
                return cropslist[i]; // Devolvemos el Prefab del cultivo
            }
        }
        return null;
    }
}