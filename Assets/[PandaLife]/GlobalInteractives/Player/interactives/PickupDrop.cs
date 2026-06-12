using UnityEngine;

public class PickupDrop : Interactuable
{
    [SerializeField] private Transform handpoint;
    private Rigidbody rb;
    private bool picked = false;
    public AudioClip waterSound; // Sonido alternativo para el cubo lleno


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override bool ShouldShakeHead(Player player)
    {
        // Si el jugador intenta recoger la cubeta pero tiene las manos ocupadas, dice que no
        return !player.IsHandEmpty();
    }

    // Implementamos la interacción real, recibiendo al Player
    public override void Interactuar(Player player)
    {
        // Si la mano no está vacía, no hacemos nada (la animación ya fue bloqueada por ShouldShakeHead)
        if (!player.IsHandEmpty()) return;

        // Le decimos al cubo dónde está la mano del jugador
        SetHandpoint(player.handpoint.transform);

        PickUp(); // Función de recoger

        BucketWater bucket = GetComponent<BucketWater>();

        // Si es un cubo Y tiene agua, suena el sonido alternativo
        if (bucket != null && bucket.hasWater)
        {
            if (interactData != null && waterSound != null)
            {
                ReproducirSonidoEnPunto(waterSound, transform.position);
            }
        }
        // Si no es un cubo, o es un cubo vacío, suena el sonido normal
        else
        {
            if (interactData != null && interactData.interactionSound != null)
            {
                ReproducirSonidoEnPunto(interactData.interactionSound, transform.position);
            }
        }

        // Le decimos al jugador que ahora sostiene este objeto
        player.SetPickedObject(this);

        // Lógica del plato
        Dish dish = GetComponentInChildren<Dish>();
        if (dish != null && CauldronPersistenceManager.instance != null)
        {
            CauldronPersistenceManager.instance.ClearDishState();
        }

        Dish dishComp = GetComponentInParent<Dish>();
        if (dishComp != null)
        {
            Debug.Log("Plato recogido con saciedad: " + dishComp.GetSaciedad());
        }
    }

    public void PickUp()
    {
        if (picked) return;
        if (handpoint == null)
        {
            Debug.LogError("Handpoint no asignado en " + gameObject.name);
            return;
        }

        rb.useGravity = false;
        rb.isKinematic = true;


        Transform pickPoint = transform.Find("PickPoint");
        if (pickPoint != null)
        {

            transform.SetParent(handpoint);

            transform.localRotation = Quaternion.Inverse(pickPoint.localRotation);
            transform.localPosition = -pickPoint.localPosition;

            transform.rotation = handpoint.rotation * Quaternion.Inverse(pickPoint.localRotation);
            transform.position = handpoint.position - transform.rotation * (pickPoint.localPosition);
        }
        else
        {
            transform.position = handpoint.position;
            transform.SetParent(handpoint);
        }


        picked = true;
        Mensaje($"{rb.name} recogido");
        // Si el tutorial está activo y esto es un saco, completamos el paso
        if (TutorialManager.instance != null && GetComponent<Bamboo_bag>() != null)
        {
            TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.CogerSaco);
        }
        // Si el tutorial está activo y esto es un cubo, completamos el paso
        if (TutorialManager.instance != null && GetComponent<BucketWater>() != null)
        {
            TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.CogerCubo);
        }
        
    }

    public void Drop()
    {
        if (!picked) return;

        rb.useGravity = true;
        rb.isKinematic = false;

        transform.SetParent(null);

        picked = false;
        if (interactData != null && interactData.dropSound != null)
        {
            ReproducirSonidoEnPunto(interactData.dropSound, transform.position);
        }
        Mensaje($"{rb.name} soltado");
    }


    public void SetHandpoint(Transform newHandpoint)
    {
        handpoint = newHandpoint;
    }
}