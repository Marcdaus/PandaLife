using UnityEngine;
using System.Collections;

public class Cauldron : Interactuable
{
    [SerializeField] private MenuCauldron cauldronmenuUI;

    [SerializeField] private Transform handpoint;
    [SerializeField] private Player jugador;

    private GameObject platopendiente = null;
    private RecipesData recetaPendiente;

    public bool tieneplatopendiente => platopendiente != null;

    public override void Interactuar()
    {
        // Si hay un plato pendiente de recoger, dárselo al jugador
        if (platopendiente != null)
        {
            GiveDish();
            return;
        }

        if(jugador.pickedobject == null)
            cauldronmenuUI.OpenCauldron();
    }

    //public void SpawnDish(GameObject prefab, bool menuabierto)
    public void SpawnDish(GameObject prefab, RecipesData receta, bool menuabierto)
    {
        if (menuabierto)
        {
            // Instanciar directo en la mano
            GameObject plato = Instantiate(prefab, handpoint.position, Quaternion.identity);

            Dish dish = plato.GetComponentInChildren<Dish>();
            if (dish != null) dish.Initialize(receta);

            PickupDrop pickup = plato.GetComponentInChildren<PickupDrop>();
            if (pickup != null)
            {
                pickup.SetHandpoint(handpoint);
                pickup.PickUp();
                jugador.SetPickedObject(pickup); // decirle al jugador que tiene el plato
            }
            cauldronmenuUI.CloseCauldron();
            Debug.Log("saciedad de plato en caldero:" + dish.GetSaciedad());
        }
        else
        {
            GameObject plato = Instantiate(prefab);
            Dish dish = plato.GetComponentInChildren<Dish>();
            if(dish!=null)dish.Initialize(receta);
            platopendiente = plato;
        
            recetaPendiente = receta;
            Debug.Log("saciedad de plato:" + dish.GetSaciedad());

            mensajeInteraccion = "recoger plato";
        }
    }

    public void RestoreFromPersistence()
    {
        var mgr = CauldronPersistenceManager.instance;
        if (mgr == null || !mgr.hasPendingDish) return;

        Debug.Log($"[Cauldron] RestoreFromPersistence - dishWasInHand: {mgr.dishWasInHand} | pos: {mgr.pendingDishPosition}");

        RecipesData receta = mgr.pendingDishRecipe;
        if (receta?.prefabResultado == null) return;

        Vector3 spawnPos = mgr.dishWasInHand
            ? handpoint.position
            : mgr.pendingDishPosition;

        GameObject plato = Instantiate(receta.prefabResultado, spawnPos, Quaternion.identity);
        Dish dish = plato.GetComponentInChildren<Dish>();
        if (dish != null) dish.Initialize(receta);

        if (mgr.dishWasInHand)
        {
            platopendiente = plato;
            recetaPendiente = receta;
            mensajeInteraccion = "recoger plato";
        }
        else
        {
            // Dejar el rigidbody dormido un momento para que no caiga antes de que
            // el suelo cargue sus colliders
            Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                // Reactivar física tras un frame
                StartCoroutine(ActivarFisicaTrasFrame(rb));
            }
        }

        mgr.ClearDishState();

    }


    private IEnumerator ActivarFisicaTrasFrame(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f); // pequeña espera para que carguen colliders
        if (rb != null) rb.isKinematic = false;
    }

    private void GiveDish()
    {
        GameObject plato = platopendiente;       
        plato.transform.position = handpoint.position;
        plato.transform.rotation = Quaternion.identity;

        PickupDrop pickup = plato.GetComponentInChildren<PickupDrop>(); 
        if (pickup != null)
        {
            pickup.SetHandpoint(handpoint);
            pickup.PickUp();
            jugador.SetPickedObject(pickup);
        }

        platopendiente = null;
        mensajeInteraccion = "abrir menú";
       
      
      
    }

}
