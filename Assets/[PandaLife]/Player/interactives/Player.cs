using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform interactionarea;
    [SerializeField] private float detectionradius = 1f;
    public LayerMask interactlayer;

    private PickupDrop pickedobject = null; // referencia al objeto que tienes en la mano

    // Guardar el objeto prioritario detectado
    private object currentTarget = null;

    // Guardar el texto para mostrar en la pantalla
    private string currentActionText = "";

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
        ScanInteractables();

        // Interactuar con E (cultivos, parcelas, cubo)
        if (Input.GetButtonDown("Interactuar") && currentTarget != null)
        {
            Interact();
        }

        // Soltar objetos con Q
        if (Input.GetButtonDown("Soltar"))
        {
            Drop();
        }
    }

    // Buscar objetos
    void ScanInteractables()
    {
        // Lanzamos la esfera de detección
        Collider[] detected = Physics.OverlapSphere(interactionarea.position, detectionradius, interactlayer);

        // Variables temporales para ver qué encontramos
        PickupDrop bucketTarget = null;
        Harvest harvesttarget = null;
        WaterCrop watertarget = null;
        IInteractuable othertarget = null;

        // 1. Prioridad máxima: coger cubo si hay uno y no estás sosteniendo nada
        if (pickedobject == null)
        {
            foreach (Collider col in detected)
            {
                PickupDrop cube = col.GetComponentInParent<PickupDrop>();
                if (cube != null && cube.GetComponent<BucketWater>() != null)
                {
                    bucketTarget = cube;
                    break;
                }
            }
        }

        // 2. Buscar cosecha solo si no hay cubo
        if (bucketTarget == null)
        {
            foreach (Collider col in detected)
            {
                Harvest harvest = col.GetComponentInParent<Harvest>();
                if (harvest != null && CanHarvest(harvest))
                {
                    harvesttarget = harvest;
                    break;
                }
            }
        }

        // 3. Buscar riego solo si no hay cubo ni cosecha
        if (bucketTarget == null && harvesttarget == null)
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

        // 4. Otros objetos si no hay cubo, cosecha ni riego
        if (bucketTarget == null && harvesttarget == null && watertarget == null)
        {
            foreach (Collider col in detected)
            {
                IInteractuable interactuable = col.GetComponentInParent<IInteractuable>();

                if (interactuable != null)
                {
                    // Ignorar el objeto que ya tenemos en las manos
                    Component interactuableComp = interactuable as Component;
                    if (interactuableComp != null && pickedobject != null && interactuableComp.gameObject == pickedobject.gameObject)
                    {
                        continue;
                    }

                    // Si tenemos el cubo en la mano, ignorar lo que no sea el río
                    if (IsHoldingBucket() && !(interactuable is River))
                    {
                        continue;
                    }

                    // objetivo real
                    othertarget = interactuable;
                    break;
                }
            }
        }

        // Reseteamos las variables por si hemos dejado de mirar un objeto
        currentTarget = null;
        currentActionText = "";

        if (bucketTarget != null)
        {
            currentTarget = bucketTarget;
            currentActionText = "coger cubeta";
        }
        else if (harvesttarget != null)
        {
            currentTarget = harvesttarget;
            currentActionText = "cosechar";
        }
        else if (watertarget != null)
        {
            currentTarget = watertarget;
            currentActionText = "regar";
        }
        else if (othertarget != null)
        {
            currentTarget = othertarget;

            // Excepción para el río
            if (othertarget is River)
            {
                currentActionText = "llenar cubeta";
            }
            else
            {
                currentActionText = "interactuar"; // Mensaje por defecto
            }
        }

        // Avisamos al objeto de la UI para que encienda o apague el texto
        if (currentTarget != null)
        {
            InteractionTextUI.instance.MostrarMensaje(currentActionText);
        }
        else
        {
            InteractionTextUI.instance.OcultarMensaje();
        }
    }

    void Interact()
    {
        // Miramos de qué tipo es el objeto que guardamos en "currentTarget" y ejecutamos su función
        if (currentTarget is PickupDrop bucket)
        {
            bucket.PickUp();
            pickedobject = bucket;
        }
        else if (currentTarget is Harvest harvest)
        {
            harvest.Interactuar();
        }
        else if (currentTarget is WaterCrop water)
        {
            water.Interactuar();
        }
        else if (currentTarget is IInteractuable other)
        {
            HandleOtherInteraction(other);
        }
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