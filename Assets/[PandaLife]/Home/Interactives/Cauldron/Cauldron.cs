using UnityEngine;

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
        // Si hay un plato pendiente de recoger, d�rselo al jugador
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
            if (dish != null)
            {
                dish.Initialize(receta);
            }

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
            dish.Initialize(receta);
            platopendiente = plato;
        
            recetaPendiente = receta;
            Debug.Log("saciedad de plato:" + dish.GetSaciedad());

            mensajeInteraccion = "recoger plato";
        }
    }

    private void GiveDish()
    {
        GameObject plato = platopendiente;
        Dish dish = plato.GetComponentInChildren<Dish>();
       
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
