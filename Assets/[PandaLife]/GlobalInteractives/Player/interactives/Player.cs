using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform interactionarea;
    [SerializeField] private Vector3 detectionBoxSize = new Vector3(1f, 2f, 1.5f);
    public LayerMask interactlayer;
    [SerializeField] private Animator anim;
    private bool collectWater = false;

    [SerializeField] private RecipesData receta;

    public PickupDrop pickedobject = null; // referencia al objeto que tienes en la mano

    

    [SerializeField] public GameObject handpoint;
    [SerializeField] private GameObject bucket;
    [SerializeField] private bool isinto = false;


    [SerializeField] private ParticleSystem waterParticles;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip splashClip;
    [SerializeField] private AudioClip fillBucketClip;
    [SerializeField] private AudioClip feedingClip;
    private Interactuable currentTarget = null;
    [SerializeField] private AudioSource PettingaudioSource;

    private ActionIconType currentActionIcon = ActionIconType.None;

    private void OnDrawGizmos()
    {
        if (interactionarea != null)
        {
            Gizmos.matrix = Matrix4x4.TRS(interactionarea.position, interactionarea.rotation, Vector3.one);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, detectionBoxSize);
        }

    }

    void Update()
    {
        ScanInteractables();
        if (collectWater) return; // Solo bloqueamos la interacción/drop

        if (!IsHandEmpty())
        {
            InteractionTextUI.instance.MostrarIconoSoltar();
        }
        else
        {
            InteractionTextUI.instance.OcultarIconoSoltar();
        }
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
        DisableMovement();

        if (currentTarget != null)
        {
            if (currentTarget.ShouldShakeHead(this))
            {
                ShakeHead();

                InteractableObject data = currentTarget.GetInteractData();
                if (data != null && data.errorSound != null)
                {
                    // Asignamos el randomizador
                    audioSource.resource = data.errorSound;

                    audioSource.Play();
                }

                return;
            }

            // Le pedimos al objeto su trigger. 
            // Si es normal, nos dará el del ScriptableObject. Si es el Panda, nos dará uno dinámico.
            anim.SetTrigger(currentTarget.GetAnimationTrigger(this));
        }
    }

    // Buscar objetos
    void ScanInteractables()
    {
        // Lanzamos la esfera de detección
        Collider[] detected = Physics.OverlapBox(interactionarea.position, detectionBoxSize / 2f, interactionarea.rotation, interactlayer);

        // Variables temporales para ver qué encontramos
        PickupDrop bucketTarget = null;
        Harvest harvesttarget = null;
        WaterCrop watertarget = null;
        Interactuable othertarget = null;

        // Prioridad máxima: coger cubo si hay uno y no est�s sosteniendo nada
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

        // Buscar cosecha solo si no hay cubo
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

        // Buscar riego solo si no hay cubo ni cosecha
        if (isinto == false)
        {
            if (bucketTarget == null && harvesttarget == null) // quitamos el IsHoldingBucket()
            {
                foreach (Collider col in detected)
                {
                    WaterCrop watercrop = col.GetComponentInParent<WaterCrop>();
                    if (watercrop != null) // quitamos el CanWater()
                    {
                        watertarget = watercrop;
                        break;
                    }
                }
            }
        }

        // Otros objetos si no hay cubo, cosecha ni riego
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
            if (othertarget == null)
            {
                foreach (Collider col in detected)
                {
                    PickupDrop pickup = col.GetComponentInParent<PickupDrop>();
                    if (pickup != null && pickup.GetComponent<BucketWater>() == null && pickup != pickedobject)
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
                    Interactuable interactuable = col.GetComponentInParent<Interactuable>();
                    if (interactuable != null)
                    {
                        Component interactuableComp = interactuable as Component;
                        if (interactuableComp != null && pickedobject != null && interactuableComp.gameObject == pickedobject.gameObject)
                            continue;
                        if (IsHoldingBucket() && !(interactuable is River))
                            continue;
                        if (interactuable is River && !IsHoldingBucket())
                        {
                            { }
                            if (interactuable is PickupDrop)
                            {
                                othertarget = interactuable;
                                break;
                            }
                            continue;
                        }
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
        currentActionIcon = ActionIconType.None;

        // Determinamos el objetivo final y le asignamos su texto al mismo tiempo
        if (bucketTarget != null)
        {
            currentTarget = bucketTarget;
            currentActionIcon = ActionIconType.PickUpBucket;
        }
        else if (harvesttarget != null)
        {
            currentTarget = harvesttarget;
            currentActionIcon = ActionIconType.Harvest;
        }
        else if (watertarget != null)
        {
            currentTarget = watertarget;
            // Mostrar icono de error o de regar según el estado
            if (!IsHoldingBucket())
                currentActionIcon = ActionIconType.NeedBucket;
            else if (!pickedobject.GetComponent<BucketWater>().hasWater)
                currentActionIcon = ActionIconType.EmptyBucket;
            else
                currentActionIcon = ActionIconType.Water;
        }
        else if (othertarget != null)
        {
            currentTarget = othertarget;

            // Minipandas
            if (currentTarget is Minipandas panda)
            {
                HungerSystem hunger = (panda as MonoBehaviour).GetComponent<HungerSystem>();

                if (hunger != null && hunger.IsRageActivated)
                    currentActionIcon = ActionIconType.Pet;
                else if (CanInteractWithMiniPanda())
                    currentActionIcon = ActionIconType.Feed;
                else
                    currentActionIcon = ActionIconType.Interact;
            }
            // Caldero
            else if (currentTarget is Cauldron)
            {
                currentActionIcon = ActionIconType.Cauldron;
            }
            // Radio
            else if (currentTarget.GetComponent<Radio>() != null)
            {
                currentActionIcon = ActionIconType.Radio;
            }
            // Plantar en FarmingArea
            else if (currentTarget is Plant)
            {
                currentActionIcon = ActionIconType.Plant;
            }
            // Coger saco de semillas u otro PickupDrop
            else if (currentTarget is PickupDrop pickup)
            {
                if (pickup.GetComponent<Bamboo_bag>() != null || pickup.GetComponent<BlueberryBag>() != null || pickup.GetComponent<RedDragonBag>() != null || pickup.GetComponent<UchuvaBerryBag>() != null)
                {
                    currentActionIcon = ActionIconType.PickUpSeedBag;
                }
                else
                {
                    currentActionIcon = ActionIconType.Interact; // Otros pickup
                }
            }
            else
            {
                // Para cualquier otro interactuable general
                currentActionIcon = ActionIconType.Interact;
            }
        }

        // Finalmente, avisamos a la UI con el tipo de icono que debe mostrar
        if (currentTarget != null)
        {
            InteractionTextUI.instance.MostrarIcono(currentActionIcon);
        }
        else
        {
            InteractionTextUI.instance.OcultarIcono();
        }
    }

    public void Interact()
    {
        // Si no hay objetivo, salimos
        if (currentTarget == null) return;

        // Por ahora mantenemos los if antiguos de los objetos que AÚN no hemos actualizado
        
        
        // Todo lo que hereda de Interactuable pasa por aquí
        else
        {
            currentTarget.Interactuar(this);
        }
    }

    // Esta función se mantiene para ser llamada al FINAL de la animación de recoger agua
    public void FinishWaterCollection()
    {
        // Rellenar el cubo físicamente/visualmente
        if (pickedobject != null)
        {
            BucketWater cubo = pickedobject.GetComponent<BucketWater>();
            if (cubo != null)
            {

                // Completamos el paso del tutorial si estamos en el río
                if (TutorialManager.instance != null)
                {
                    TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.LlenarCubo);
                }


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
        if (bucket == null || !bucket.GetComponent<BucketWater>().hasWater)
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

        Debug.Log("EnableMovement llamado");
        if (GetComponent<movement>() != null)
        {
            GetComponent<movement>().enabled = true;
        }
    }
    public void PlayWaterParticles()
    {
        if (waterParticles != null)
        {
            waterParticles.Play();
            PlaySplashSound();
        }
        else
        {
            Debug.LogWarning("No has asignado el ParticleSystem en el Inspector de Player.");
        }
    }

    public void StopWaterParticles()
    {
        if (waterParticles != null)
        {
            waterParticles.Stop();
            PlaySplashSound();
        }
    }

    public void PlaySplashSound()
    {
        audioSource.clip = splashClip;
        audioSource.Play();
    }

    public void StopSplashSound()
    {
        audioSource.Stop();
    }

    public void PlayFillBucketSound()
    {
        audioSource.PlayOneShot(fillBucketClip);
    }

    public void GivingFood()
    {
        audioSource.clip = feedingClip;
        audioSource.Play();
    }

    //-----------------------------
    public void ShakeHead()
    {
        Debug.Log("ShakeHead llamado");
        DisableMovement();
        anim.SetTrigger("ShakeHead");
    }
    public void PetPanda()
    {
       PettingaudioSource.Play();
    }

}
   