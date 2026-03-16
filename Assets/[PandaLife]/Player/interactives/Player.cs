using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform interactionarea;
    [SerializeField] private float detectionradius = 1f;
    public LayerMask interactlayer;

    private PickupDrop pickedobject = null; // referencia al objeto que tienes en la mano

    private void OnDrawGizmos()
    {
        if (interactionarea != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(interactionarea.position, detectionradius);
        }

    }
    void Update()
    {
        // Interactuar con E (cultivos, parcelas, cubo)
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        // Soltar objetos con Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    void Interact()
    {
        Collider[] detected = Physics.OverlapSphere(interactionarea.position, detectionradius, interactlayer);

        Harvest harvestTarget = null;
        WaterCrop waterTarget = null;
        IInteractuable otherTarget = null;

        // Primero buscamos cosecha
        foreach (Collider col in detected)
        {
            Harvest harvest = col.GetComponentInParent<Harvest>();
            if (harvest != null && CanHarvest(harvest))
            {
                harvestTarget = harvest;
                break; // Prioridad: cosechar primero
            }
        }

        // Luego buscamos riego solo si no hay cosecha
        if (harvestTarget == null)
        {
            foreach (Collider col in detected)
            {
                WaterCrop waterCrop = col.GetComponentInParent<WaterCrop>();
                if (waterCrop != null && CanWater(waterCrop))
                {
                    waterTarget = waterCrop;
                    break;
                }
            }
        }

        // Finalmente otros
        if (harvestTarget == null && waterTarget == null)
        {
            foreach (Collider col in detected)
            {
                IInteractuable interactuable = col.GetComponentInParent<IInteractuable>();
                if (interactuable != null)
                {
                    otherTarget = interactuable;
                    break;
                }
            }
        }

        // Ejecutamos la interacción según prioridad
        if (harvestTarget != null) harvestTarget.Interactuar();
        else if (waterTarget != null) waterTarget.Interactuar();
        else if (otherTarget != null) HandleOtherInteraction(otherTarget);
    }

    bool CanWater(WaterCrop waterCrop)
    {
        PickupDrop bucket = GetBucket();
        if (bucket == null || !bucket.GetComponent<BucketWater>().hasWater)
        {
            // Solo mostrar mensaje si no hay cosecha pendiente
            return false;
        }
        return true;
    }

    bool CanHarvest(Harvest harvest)
    {
        Crop crop = harvest.GetCrop();
        if (crop != null && crop.IsHarvestable()) return true;
        Debug.Log("Crop null o no está lista para cosechar");
        return false;
    }

    void HandleOtherInteraction(IInteractuable interactuable)
    {
        if (interactuable is PickupDrop cube && pickedobject == null)
        {
            cube.PickUp();
            pickedobject = cube;
        }
        else
        {
            interactuable.Interactuar();
        }
    }

    void Drop()
    {
        if (pickedobject != null)
        {
            pickedobject.Drop(); // función separada para soltar
            pickedobject = null;
        }
    }

    public PickupDrop GetBucket()
    {
        return pickedobject;
    }
}