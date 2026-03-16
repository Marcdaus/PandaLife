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

        IInteractuable targetInteractable = null;
        IInteractuable otherInteractable = null;

        foreach (Collider col in detected)
        {
            IInteractuable interactuable = col.GetComponentInParent<IInteractuable>();
            if (interactuable == null) continue;

            // -----------------------------
            // Riego
            // -----------------------------
            if (interactuable is WaterCrop waterCrop)
            {
                PickupDrop bucket = GetBucket();
                if (bucket == null || !bucket.GetComponent<BucketWater>().hasWater)
                {
                    Debug.Log("Necesitas un cubo lleno para regar");
                    continue;
                }
                targetInteractable = waterCrop;
                break;
            }

            // -----------------------------
            // Cosecha
            // -----------------------------
            Harvest harvest = col.GetComponentInParent<Harvest>();
            if (harvest != null)
            {
                Crop crop = harvest.GetCrop();
                if (crop != null && crop.IsHarvestable())
                {
                    targetInteractable = harvest;
                    break;
                }
                else
                {
                    Debug.Log("Crop null o no está lista para cosechar");
                    continue;
                }
            }

            // -----------------------------
            // Plantar / otros
            // -----------------------------
            if (otherInteractable == null)
                otherInteractable = interactuable;
        }

        // Ejecutar interacción
        if (targetInteractable != null)
        {
            targetInteractable.Interactuar();
        }
        else if (otherInteractable != null)
        {
            PickupDrop cube = otherInteractable as PickupDrop;
            if (cube != null && pickedobject == null)
            {
                cube.PickUp();
                pickedobject = cube;
            }
            else
            {
                otherInteractable.Interactuar();
            }
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