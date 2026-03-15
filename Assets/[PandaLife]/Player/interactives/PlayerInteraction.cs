using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform interactionArea; 
    public float detectionRadius = 1f; // Area de la esfera de interacción
   
    //==================cosechar=====================

    public LayerMask cropLayer; // coger la capa de cultivos
    public LayerMask farmingLayer; // capa con un trozo de parcela

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
            PickBucket();
            if(Picke_bucket == null) //si no tiene la cubeta, que intente cosechar
                if (!TryHarvest())   // prueba hacer cosecha y si le da false
            { sow(); }       // intenta plantar

          
            
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropBucket();
        }
    }
    //==================cosechar=====================
    bool TryHarvest()
    {
        //esto es para que si tiene la cubeta, que no coseche------------------------
         if (Picke_bucket != null)
        {
            Debug.Log("harvest: tienes una cubeta y necesitas las dos manos campeon");
            return false;
        }
        //-----------------------------------------------------------------------------

        // 1. Crea una esfera invisible y guarda en un array todos los objetos que toca
        Collider[] detectedObjects = Physics.OverlapSphere(interactionArea.position, detectionRadius, cropLayer); //cropLayer debe tener farmingArea y crops en unity

        // 2. Recorre cada objeto que ha tocado la esfera
        foreach (Collider col in detectedObjects)
        {
            // Buscamos si el objeto del collider tiene el script Crop
            Crop foundCrop = col.GetComponent<Crop>();
            FarmingArea area = col.GetComponentInParent<FarmingArea>();  //y los colliders con farmingArea lo metemos en otra variable
            
            if (foundCrop != null) // Si el objeto tiene el script Crop
            {
                area.ThereIsSomething = true; // dice que el farmingArea no esta vacia

                // Estas dos funciones pertenecen a la clase Crop
                if (foundCrop.IsHarvestable()) // Comprueba si su estado de crecimiento es 3
                {
                    foundCrop.Harvest(); // Recolecta el cultivo y destruye el objeto
                    

                    //esta funcion es de farmingArea
                    if (area != null)
                        area.ThereIsSomething = false; //dice que el terreno de cultivo (farmingArea) esta vacio

                    Debug.Log("recolectado");
                    return true; //devuelve true si cosecho
                }
                else
                {
                    Debug.Log("Aún no ha crecido: " + foundCrop.growthStage);
                }
            }
        }
        return false; //false si no cosecho
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

//====================================plantar=========================================    
void sow()
{
    //para ver que no tenga la cubeta -------------------------------------------
    if (Picke_bucket != null)
    {
        Debug.Log("sow: tienes una cubeta y necesitas las dos manos campeon");
        return;
    }
    //----------------------------------------------------------------------------
    
    Collider[] terrain = Physics.OverlapSphere(interactionArea.position, detectionRadius, farmingLayer); //tiene las cosas de la capa de farming en unity

    foreach (Collider col in terrain)
    {
        //agarras todos los colliders que tengan FarmingArea que choquen con el gizmo del player
        FarmingArea area = col.GetComponentInParent<FarmingArea>(); 
        if (area != null)
        {
            Debug.Log("el terreno tiene crops? " + area.ThereIsSomething);

            if (!area.ThereIsSomething) //si el terreno NO tiene un crop
            {
                area.sowing(); //plantas 
                break;
            }
        }
    }
}

}