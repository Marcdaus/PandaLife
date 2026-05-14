using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform interactionarea;
    [SerializeField] private float detectionradius = 1f;
    public LayerMask interactlayer;
    [SerializeField] private Animator anim;
    private bool collectWater=false;

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

        if (collectWater) return; // Solo bloqueamos la interacción/drop

        // Interactuar con E
        if (Input.GetButtonDown("Interactuar") && currentTarget != null)
        {
            // Decidimos que animacion toca
            TriggerInteractionAnimation();
        }

        // Soltar objetos con Q
        if (Input.GetButtonDown("Soltar")) anim.SetTrigger("Drop");
    }

    // Elegir animacion dependiendo del current target
    void TriggerInteractionAnimation()
    {
        // Recuerden poner el evento al final de cada animacion
        // para que se pueda mover el jugador.
        DisableMovement();

        if (currentTarget is River && IsHoldingBucket())
        {
            collectWater = true;

            anim.SetTrigger("CollectWater"); // Llenar cubo
        }
        else if (currentTarget is Harvest)
        {
            anim.SetTrigger("PickUp"); // Cosechar 
        }
        else if (currentTarget is WaterCrop)
        {
            anim.SetTrigger("Water"); // Regar
        }
        else if (currentTarget is PickupDrop)
        {
            anim.SetTrigger("PickUp"); // Recoger objetos
        }
        else if (currentTarget is Minipandas panda)
        {
            HungerSystem hunger = panda.GetComponent<HungerSystem>();
            if (hunger != null && hunger.IsRageActivated) anim.SetTrigger("Pet");  // Acariciar
            else if (CanInteractWithMiniPanda()) anim.SetTrigger("PickUp"); // Alimentar
            else anim.SetTrigger("PickUp"); // Animación genérica
        }
        else
        {
            // Demás objetos interactuables
            anim.SetTrigger("PickUp");
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
        if (IsHandEmpty())
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
            if (bucketTarget == null && harvesttarget == null && IsHoldingBucket())
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
            if (othertarget == null && IsHandEmpty())
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

            // Minipandas
            if (currentTarget is Minipandas panda)
            {
                // Obtenemos el HungerSystem para comprobar el estado de ira
                HungerSystem hunger = (panda as MonoBehaviour).GetComponent<HungerSystem>();

                if (hunger != null && hunger.IsRageActivated)
                {
                    currentActionText = "Acariciar";
                }
                else if (CanInteractWithMiniPanda())
                {
                    currentActionText = "alimentar";
                }
                else
                {
                    currentActionText = "interactuar";
                }
            }
            // Los demás
            else
            {
                Interactuable interactuableRef = (currentTarget as MonoBehaviour)?.GetComponent<Interactuable>();
                if (interactuableRef != null)
                {
                    currentActionText = interactuableRef.mensajeInteraccion;
                }
                else
                {
                    currentActionText = "interactuar";
                }
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

    // Asegúrate de que esta función sea public si la llamas desde un Animation Event
    public void Interact()
    {
        // Si estamos recogiendo agua, la animación ya se disparó y el movimiento está bloqueado.
        // Aquí no hacemos nada, porque la lógica real de llenado sucederá en FinishWaterCollection().
        if (currentTarget is River && IsHoldingBucket())
        {
            return;
        }

        if (currentTarget is PickupDrop bucket)
        {
            if (!IsHandEmpty())
            {
                Debug.Log("Ya tienes algo en la mano, no puedes coger esto.");
                return;
            }
            bucket.SetHandpoint(handpoint.transform);
            bucket.PickUp();
            pickedobject = bucket;

            // Si era un plato en el suelo, ya no está suelto
            Dish dish = bucket.GetComponentInChildren<Dish>();
            if (dish != null)
                CauldronPersistenceManager.instance?.ClearDishState();
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

    // Esta función se mantiene para ser llamada al FINAL de la animación de recoger agua
    public void FinishWaterCollection()
    {
        // 1. Rellenar el cubo físicamente/visualmente
        if (pickedobject != null)
        {
            BucketWater cubo = pickedobject.GetComponent<BucketWater>();
            if (cubo != null)
            {
                cubo.Fill();
            }
        }

        collectWater = false;
    }


    public bool IsHoldingBucket()
    {
        return pickedobject != null && pickedobject.GetComponent<BucketWater>() != null;
    }

    public bool IsHoldingDish()
    {
        return pickedobject != null && pickedobject.GetComponentInChildren<Dish>() != null;
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
        if (interactuable is PickupDrop cube && IsHandEmpty())
        {
            if (!IsHandEmpty())
            {
                Debug.Log("La mano está ocupada.");
                return;
            }
            cube.SetHandpoint(handpoint.transform);
            pickedobject = cube;

            // Si era un plato en el suelo, ya no está suelto
            Dish dish = cube.GetComponentInChildren<Dish>();
            if (dish != null)
                CauldronPersistenceManager.instance?.ClearDishState();

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

public void Drop()
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
        if (IsHandEmpty())
            return false;

        // Es un plato 
        if (pickedobject.GetComponent<Dish>() == null)
            return false;

        return true;
    }
    public bool IsHandEmpty()
    {
        // Devuelve true SOLO si la variable es nula Y el handpoint no tiene ningún objeto emparentado
        return pickedobject == null && handpoint.transform.childCount == 0;
    }

    // Función para detener al jugador
    public void DisableMovement()
    {
        if (GetComponent<movement>() != null)
        {
            GetComponent<movement>().enabled = false;
        }
    }

    // Función para que vuelva a moverse (Mi gente llamad esto en un evento al final de cada animación)
    public void EnableMovement()
    {
        if (GetComponent<movement>() != null)
        {
            GetComponent<movement>().enabled = true;
        }
    }
}