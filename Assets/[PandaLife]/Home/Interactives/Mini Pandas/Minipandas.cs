using UnityEngine;

public class Minipandas : Interactuable
{
    public void InteractuarConPlato(PickupDrop plato, Player player)
    {
        Debug.Log("Mini panda se come el plato ");

        Dish dish = plato.GetComponent<Dish>();
        if (dish != null)
        {
            Debug.Log("Saciedad: " + dish.GetSaciedad());
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
