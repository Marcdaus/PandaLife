using UnityEngine;

public class Minipandas : Interactuable
{

    private HungerSystem hungerSystem;
    void Awake()
    {
        hungerSystem = GetComponent<HungerSystem>();
    }

    public void InteractuarConPlato(PickupDrop plato, Player player)
    {
        Debug.Log("Mini panda se come el plato ");

        Dish dish = plato.GetComponent<Dish>();
        if (dish != null)
        {
            int saciedad = dish.GetSaciedad();
            Debug.Log("alimentaste con el plato");
            hungerSystem.Restaurar(20);
            hungerSystem.PauseHunger(5f);
        }

        plato.Drop();
        Destroy(plato.gameObject);
    }

    public override void Interactuar()
    {
        //seguramente aqui diferencie entre los estados, si no en otro script
        //y hago otra funcion que sea pandaEnfadado
        Debug.Log("el panda: si no me muevo no me ve");
    }
}
