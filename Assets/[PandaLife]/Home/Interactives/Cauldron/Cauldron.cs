using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Interactuable
{
    [SerializeField] private MenuCauldron cauldronmenuUI;

    [SerializeField] private Transform handpoint;
    [SerializeField] private Player jugador;
    [SerializeField] private Transform displayPoint;
    private GameObject platopendiente = null;
    private RecipesData recetaPendiente;
    public GameObject PlatoPendienteGameObject => platopendiente;

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
            GameObject plato = Instantiate(prefab, displayPoint.position, displayPoint.rotation);
            Dish dish = plato.GetComponentInChildren<Dish>();
            if(dish!=null)dish.Initialize(receta);
            platopendiente = plato;
        
            recetaPendiente = receta;
            Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
            Debug.Log("saciedad de plato:" + dish.GetSaciedad());

            mensajeInteraccion = "recoger plato";
        }
    }

public void RestoreFromPersistence()
{
    var mgr = CauldronPersistenceManager.instance;
    if (mgr == null || !mgr.hasPendingDish) return;

    // Copiar la lista antes de limpiarla
    var dishes = new List<CauldronPersistenceManager.DishState>(mgr.PendingDishes);
    mgr.ClearAllDishStates();

    foreach (var state in dishes)
    {
        RecipesData receta = state.recipe;
        if (receta?.prefabResultado == null) continue;

            Vector3 spawnPos = state.wasInHand ? displayPoint.position : state.position;
            Quaternion spawnRot = state.wasInHand ? displayPoint.rotation : Quaternion.identity;
            GameObject plato = Instantiate(receta.prefabResultado, spawnPos, Quaternion.identity);
        Dish dish = plato.GetComponentInChildren<Dish>();
        if (dish != null) dish.Initialize(receta);

        if (state.wasInHand)
        {
            // Solo el primero "en mano" se convierte en plato pendiente del caldero
            if (platopendiente == null)
            {
                platopendiente = plato;
                recetaPendiente = receta;
                mensajeInteraccion = "recoger plato";
                Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
            }
        }
        else
        {
            Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                StartCoroutine(ActivarFisicaTrasFrame(rb));
            }
        }

        Debug.Log($"[Cauldron] Restaurado plato: {receta.nombrereceta} en {spawnPos}");
    }
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

        Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

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
