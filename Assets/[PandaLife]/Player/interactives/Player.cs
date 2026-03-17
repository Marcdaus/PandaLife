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

        Harvest harvesttarget = null;
        WaterCrop watertarget = null;
        IInteractuable othertarget = null;

        // Primero buscamos cosecha
        foreach (Collider col in detected)
        {
            Harvest harvest = col.GetComponentInParent<Harvest>();
            if (harvest != null && CanHarvest(harvest))
            {
                harvesttarget = harvest;
                break; // Prioridad: cosechar primero
            }
        }

        // Luego buscamos riego solo si no hay cosecha
        if (harvesttarget == null)
        {
            foreach (Collider col in detected)
            {
                WaterCrop watercrop = col.GetComponentInParent<WaterCrop>();
                if (watercrop != null && CanWater(watercrop))
                {
                    watertarget = watercrop;
                    break;
                }
            }
        }

        // Finalmente otros
        if (harvesttarget == null && watertarget == null)
        {
            foreach (Collider col in detected)
            {
                IInteractuable interactuable = col.GetComponentInParent<IInteractuable>();
                if (interactuable != null)
                {
                    othertarget = interactuable;
                    break;
                }
            }
        }

        // Ejecutamos la interacción según prioridad
        if (harvesttarget != null) harvesttarget.Interactuar();
        else if (watertarget != null) watertarget.Interactuar();
        else if (othertarget != null) HandleOtherInteraction(othertarget);
    }

    public bool IsHoldingBucket()
    {
        return pickedobject != null && pickedobject.GetComponent<BucketWater>() != null;
    }

    public bool IsHoldingDish()
    {
        return pickedobject != null && pickedobject.GetComponent<PickupDrop>() != null;
    }

    bool CanWater(WaterCrop watercrop)
    {
        PickupDrop bucket = GetBucket();
        if (bucket == null || !bucket.GetComponent<BucketWater>().haswater)
        {
            return false;
        }
        return true;
    }

    bool CanHarvest(Harvest harvest)
    {
        if (IsHoldingBucket())
        {
            Debug.Log("No puedes cosechar mientras sostienes el cubo");
            return false;
        }

        Crop crop = harvest.GetCrop();
        if (crop != null && crop.IsHarvestable()) return true;

        Debug.Log("Crop null o no está lista para cosechar");
        return false;
    }

    void HandleOtherInteraction(IInteractuable interactuable)
    {
        if (IsHoldingBucket() && !(interactuable is River))
        {
            Debug.Log("No puedes plantar mientras sostienes el cubo");
            return;
        }

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
            pickedobject.Drop();
            pickedobject = null;
        }
    }

    public PickupDrop GetBucket()
    {
        return pickedobject;
    }
}