using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform interactionarea;
    [SerializeField] private float detectionradius = 1f;
    public LayerMask interactlayer;

    [SerializeField] private RecipesData receta;

    public PickupDrop pickedobject = null; // referencia al objeto que tienes en la mano

    // Guardar el objeto prioritario detectado
    private object currentTarget = null;

    // Guardar el texto para mostrar en la pantalla
    private string currentActionText = "";

    [SerializeField] private GameObject handpoint;
    [SerializeField] private GameObject bucket;
    [SerializeField] private bool isinto = false;
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
        // Lanzamos la esfera de detecci�n
        Collider[] detected = Physics.OverlapSphere(interactionarea.position, detectionradius, interactlayer);

        // Variables temporales para ver qu� encontramos
        PickupDrop bucketTarget = null;
        Harvest harvesttarget = null;
        WaterCrop watertarget = null;
        IInteractuable othertarget = null;

        // 1. Prioridad m�xima: coger cubo si hay uno y no est�s sosteniendo nada
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
        if(isinto == false) {
            if (bucketTarget == null && harvesttarget == null && handpoint.transform.Find(bucket.name))
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
        }

        // 4. Otros objetos si no hay cubo, cosecha ni riego
        if (bucketTarget == null && harvesttarget == null && watertarget == null)
        {
            // Prioridad 1: caldero con plato pendiente
            foreach (Collider col in detected)
            {
                Cauldron cauldron = col.GetComponentInParent<Cauldron>();
                if (cauldron != null && cauldron.tieneplatopendiente)
                {
                    othertarget = cauldron;
                    break;
                }
            }

            // Prioridad 2: plato en el suelo
            if (othertarget == null && pickedobject == null)
            {
                foreach (Collider col in detected)
                {
                    PickupDrop pickup = col.GetComponentInParent<PickupDrop>();
                    if (pickup != null && pickup.GetComponent<BucketWater>() == null)
                    {
                        othertarget = pickup;
                        break;
                    }
                }
            }

            // Si no hay caldero con plato pendiente, buscar cualquier interactuable
            if (othertarget == null)
            {
                foreach (Collider col in detected)
                {
                    IInteractuable interactuable = col.GetComponentInParent<IInteractuable>();
                    if (interactuable != null)
                    {
                        Component interactuableComp = interactuable as Component;
                        if (interactuableComp != null && pickedobject != null && interactuableComp.gameObject == pickedobject.gameObject)
                            continue;
                        if (IsHoldingBucket() && !(interactuable is River))
                            continue;
                        if (interactuable is River && !IsHoldingBucket())
                            continue;
                        if (interactuableComp != null)
                        {
                            FarmingArea area = interactuableComp.GetComponent<FarmingArea>();
                            if (area != null && area.ThereIsSomething)
                                continue;
                        }
                        if (interactuable is Harvest || interactuable is WaterCrop)
                            continue;

                        othertarget = interactuable;
                        break;
                    }
                }
            }
        }

        // Reseteamos las variables por si hemos dejado de mirar un objeto
        currentTarget = null;
        currentActionText = "";

        // Determinamos el objetivo final y le asignamos su texto al mismo tiempo
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


            // Si es un objeto gen�rico, leemos el texto de su Inspector
            Interactuable interactuableRef = (currentTarget as MonoBehaviour)?.GetComponent<Interactuable>();
            if (interactuableRef != null)
            {
                currentActionText = interactuableRef.mensajeInteraccion;
            }
            else
            {
                currentActionText = "interactuar"; // Texto por defecto
            }
        }

        // Finalmente, encendemos la UI con el texto que haya ganado, o la apagamos
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
        // Miramos de qu� tipo es el objeto que guardamos en "currentTarget" y ejecutamos su funci�n
        if (currentTarget is PickupDrop bucket)
        {
            bucket.SetHandpoint(handpoint.transform);
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
            //Debug.Log("No puedes cosechar mientras sostienes el cubo");
            return false;
        }

        Crop crop = harvest.GetCrop();
        if (crop != null && crop.IsHarvestable()) return true;

        //Debug.Log("Crop null o no est� lista para cosechar");
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
            cube.SetHandpoint(handpoint.transform);
            cube.PickUp();
            pickedobject = cube;

            Dish dishComp = pickedobject.GetComponent<Dish>();
            if (dishComp != null && dishComp.GetSaciedad() == 0)
            {
                // Inicializa con la receta correcta antes de usarla
                dishComp.Initialize(receta); // miReceta debe ser la receta actual que corresponda
            }
        }
        if (interactuable is Minipandas minipanda)
        {
            if (CanInteractWithMiniPanda())
            {
                minipanda.InteractuarConPlato(pickedobject, this);
                return;
            }
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

    public void SetPickedObject(PickupDrop obj)
    {
        pickedobject = obj;
    }

    //------------------------------

    bool CanInteractWithMiniPanda()
    {
        // Tiene objeto en mano
        if (pickedobject == null)
            return false;

        // Es un plato 
        if (pickedobject.GetComponent<Dish>() == null)
            return false;

        return true;
    }
}