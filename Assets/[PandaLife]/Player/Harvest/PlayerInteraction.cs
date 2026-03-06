using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform interactionArea; 
    public float detectionRadius = 1f; // Area de la esfera de interacción
    public LayerMask cropLayer; // coger la capa de cultivos

    // Dibujar la esfera para interactuar
    private void OnDrawGizmos()
    {
        if (interactionArea != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(interactionArea.position, detectionRadius);
        }
    }

    void Update()
    {
        // Al pulsar la E intenta cosechar
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryHarvest();
        }
    }

    void TryHarvest()
    {
        // 1. Create the invisible sphere and store everything it touches in an array
        Collider[] detectedObjects = Physics.OverlapSphere(interactionArea.position, detectionRadius, cropLayer);

        // 2. Check each object that touched the sphere
        foreach (Collider col in detectedObjects)
        {
            // Buscamos de los objetos del collider cual es un Crop
            Crop foundCrop = col.GetComponent<Crop>();

            if (foundCrop != null) // Si tiene el script Crop
            {
                // Estas dos funciones son de la clase Crop
                if (foundCrop.IsHarvestable()) //chequea si tiene un 3 en su estado
                {
                    foundCrop.Harvest(); // destruye el objeto
                    break;
                }
                else
                {
                    Debug.Log("Aún no ha crecido: " + foundCrop.growthStage);
                }
            }
        }
    }

    
}