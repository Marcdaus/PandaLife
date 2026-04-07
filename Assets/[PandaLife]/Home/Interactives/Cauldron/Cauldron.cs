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

            Dish dish = plato.GetComponent<Dish>();
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

        }
        else
        {
            platopendiente = prefab;
            recetaPendiente = receta;
            mensajeInteraccion = "recoger plato";
        }
    }

    private void GiveDish()
    {
        GameObject plato = Instantiate(platopendiente, handpoint.position, Quaternion.identity);

        Dish dish = plato.GetComponent<Dish>();
        if (dish != null)
        {
            dish.Initialize(recetaPendiente);
        }
        
        PickupDrop pickup = plato.GetComponentInChildren<PickupDrop>(); 
        if (pickup != null)
        {
            pickup.SetHandpoint(handpoint);
            pickup.PickUp();
            jugador.SetPickedObject(pickup);
        }

        platopendiente = null;
        mensajeInteraccion = "abrir men�";
    }
}
