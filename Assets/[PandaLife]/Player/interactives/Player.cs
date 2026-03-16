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

        // Si tienes algo en la mano, solo puedes recoger/soltar objetos, no plantar ni cosechar
        if (pickedobject != null)
        {
            Debug.Log("Tienes un objeto en la mano, no puedes plantar ni cosechar.");
            return;
        }

        IInteractuable cropToHarvest = null;
        IInteractuable otherInteractable = null;

        // Buscamos primero cultivos, si no hay, otros interactuables
        foreach (Collider col in detected)
        {
            IInteractuable interactuable = col.GetComponentInParent<IInteractuable>();
            if (interactuable == null) continue;

            // Prioridad: cultivo
            if (col.GetComponentInParent<Crop>() != null)
            {
                cropToHarvest = interactuable;
                break; // ya tenemos cultivo, no necesitamos buscar más
            }

            // Si no es cultivo, lo guardamos como alternativa
            if (otherInteractable == null)
                otherInteractable = interactuable;
        }

        // Interactuar con cultivo si existe, si no con otro interactuable
        if (cropToHarvest != null)
        {
            cropToHarvest.Interactuar();
        }
        else if (otherInteractable != null)
        {
            // Si es cubo y no tenemos ninguno en mano
            PickupDrop cube = otherInteractable as PickupDrop;
            if (cube != null && pickedobject == null)
            {
                cube.PickUp();   // Recogemos cubo
                pickedobject = cube;
            }
            else
            {
                // Otros interactuables (parcelas)
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
}