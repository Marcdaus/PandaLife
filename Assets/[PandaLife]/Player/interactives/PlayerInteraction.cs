using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform interactionArea; 
    public float detectionRadius = 1f; // Area de la esfera de interacción
   
    //==================cultivar=====================

    public LayerMask cropLayer; // coger la capa de cultivos

    //==================recoger y soltar cubo=====================

    public GameObject Hand_point; //punto donde va estar la mano
    private GameObject Picke_bucket; //sirve para comprobar que ha pillado el cubo
    public LayerMask pickeLayer; // coger la capa de picke

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
            PickBucket();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropBucket();
        }
    }
    //==================cultivar=====================
    void TryHarvest()
    {
        // 1. Crea una esfera invisible y guarda en un array todos los objetos que toca
        Collider[] detectedObjects = Physics.OverlapSphere(interactionArea.position, detectionRadius, cropLayer);

        // 2. Recorre cada objeto que ha tocado la esfera
        foreach (Collider col in detectedObjects)
        {
            // Buscamos si el objeto del collider tiene el script Crop
            Crop foundCrop = col.GetComponent<Crop>();

            if (foundCrop != null) // Si el objeto tiene el script Crop
            {
                // Estas dos funciones pertenecen a la clase Crop
                if (foundCrop.IsHarvestable()) // Comprueba si su estado de crecimiento es 3
                {
                    foundCrop.Harvest(); // Recolecta el cultivo y destruye el objeto
                    break;
                }
                else
                {
                    Debug.Log("Aún no ha crecido: " + foundCrop.growthStage);
                }
            }
        }
    }
    void PickBucket()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(interactionArea.position, detectionRadius, pickeLayer);
        foreach (Collider col in detectedObjects)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Desactiva la física
                rb.useGravity = false;
                rb.isKinematic = true;

                // Mueve el objeto al punto de la mano
                col.transform.position = Hand_point.transform.position;

                // Lo hace hijo de la mano
                col.transform.SetParent(Hand_point.transform);

                // Guardamos referencia del objeto recogido
                Picke_bucket = col.gameObject;

                break;
            }

        }
    }
    void DropBucket()
    {
        if (Picke_bucket != null)
        {
            Rigidbody rb = Picke_bucket.GetComponent<Rigidbody>();

            // Activar la física otra vez
            rb.useGravity = true;
            rb.isKinematic = false;

            // Quitar el objeto de la mano
            Picke_bucket.transform.SetParent(null);

            // Limpiar la referencia
            Picke_bucket = null;
        }
    }

}