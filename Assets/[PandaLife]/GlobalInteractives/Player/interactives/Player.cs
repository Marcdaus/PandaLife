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

    private Cauldron cauldronRef;

    private void OnDrawGizmos()
    {
        if (interactionarea != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(interactionarea.position, detectionradius);
        }
    }

    private void Start()  
    {
        cauldronRef = FindFirstObjectByType<Cauldron>();
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

            // Si era un plato en el suelo, ya no está suelto
            Dish dish = bucket.GetComponentInChildren<Dish>();
            if (dish != null)
            {
                CauldronPersistenceManager.instance?.ClearDishState();
                cauldronRef?.NotificarPlatoRecogido();
            }
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
            pickedobject = cube;

            // Si era un plato en el suelo, ya no está suelto
            Dish dish = cube.GetComponentInChildren<Dish>();
            if (dish != null)
            {
                CauldronPersistenceManager.instance?.ClearDishState();
                cauldronRef?.NotificarPlatoRecogido();
            }

            Dish dishComp = pickedobject.GetComponentInParent<Dish>();
            if (dishComp != null)
            {
                Debug.Log("Plato recogido con saciedad: " + dishComp.GetSaciedad());
                Debug.Log(dishComp.GetIngredientesTexto());
            }

            cube.PickUp();
        
        }
        if (interactuable is Minipandas minipanda)
        {
            if (CanInteractWithMiniPanda())
            {
                Dish dishComp = pickedobject.GetComponentInParent<Dish>();
                //Debug.Log("ANTES de darlo:");
                //Debug.Log("player: Dish name: " + pickedobject.name);
                //Debug.Log("player: Dish instance ID: " + pickedobject.GetInstanceID()); 
                minipanda.InteractuarConPlato(dishComp, this);
                //Debug.Log("pikedu:" + dishComp);
                return;
            }
            else
            {
                minipanda.Interactuar(); //panda con ira
            }
            return;
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
        Dish dish = pickedobject.GetComponentInChildren<Dish>();
        Debug.Log($"[Player] Drop - dish encontrado: {dish}");
        if (dish != null && CauldronPersistenceManager.instance != null)
        {
            Debug.Log($"[Player] Guardando posición plato: {pickedobject.transform.position}");
            CauldronPersistenceManager.instance.SaveDishState(
                dish.GetReceta(),
                pickedobject.transform.position,
                inHand: false
            );
            cauldronRef?.NotificarPlatoSuelto(pickedobject.gameObject, dish.GetReceta());
            Debug.Log($"[Player] NotificarPlatoSuelto llamado en cauldronRef: {cauldronRef}");
        }

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