using UnityEngine;

public class Minipandas : Interactuable
{

    private HungerSystem hungerSystem;
    void Awake()
    {
        hungerSystem = GetComponent<HungerSystem>();
    }

    public void InteractuarConPlato(Dish dish, Player player)
    {
        Debug.Log("Mini panda se come el plato ");
        
        if (dish != null)
        {
            int saciedad = dish.GetSaciedad();
            Debug.Log("saciedad en minipandas al alimentar: " + saciedad);

            hungerSystem.Restaurar(saciedad);
            hungerSystem.PauseHunger(5f);
            //Debug.Log("minipandas: Dish name: " + dish.name);
            //Debug.Log("minipandas: Dish instance ID: " + dish.GetInstanceID());
        }
  
       Destroy(player.pickedobject.gameObject);
        player.SetPickedObject(null);
    }

    public override void Interactuar()
    {
        //seguramente aqui diferencie entre los estados, si no en otro script
        //y hago otra funcion que sea pandaEnfadado
        Debug.Log("el panda: si no me muevo no me ve");
    }
}
