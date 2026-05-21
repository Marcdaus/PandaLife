using System.Collections.Generic;
using UnityEngine;

public class Minipandas : Interactuable
{

    private HungerSystem hungerSystem;

    [SerializeField] private int indicePanda; // 0, 1 o 2
    [SerializeField] private string pedidoDeseado; // Aquí guardaremos qué es lo que quiere ("Bamboo", "Uchuva", etc.)
    [SerializeField] private float porcentajecalmado;
    [SerializeField] Transform spawnpoint; 
    [SerializeField] private ParticleSystem eatingParticles;
    private ParticleSystem eatingParticlesInstance;
    private Animator animator;

    void Awake()
    {
        hungerSystem = GetComponent<HungerSystem>();
    }

    private void Start()
    {
        // Al arrancar, el panda busca el GameManager y sus pedidos
        if (GameManager.instance != null)
        {
            PandaRequest pandaReq = GameManager.instance.GetComponent<PandaRequest>();
            animator = GetComponent<Animator>();

            if (pandaReq != null)
            {
                // Obtenemos la memoria de los pedidos actuales
                List<string> pedidosActuales = pandaReq.GetCurrentRequests();

                // Comprobamos que la lista tenga suficientes elementos para evitar errores
                if (pedidosActuales.Count > indicePanda)
                {
                    pedidoDeseado = pedidosActuales[indicePanda];
                    Debug.Log($"Mini panda {indicePanda} configurado. Quiere comer: {pedidoDeseado}");
                }
                else
                {
                    Debug.LogWarning($"El panda {indicePanda} no encontró un pedido en la lista.");
                }
            }
        }
    }
    

    public void InteractuarConPlato(Dish dish, Player player)
    {
        Debug.Log("Mini panda se come el plato ");
        
        if (dish != null)
        {
            if (hungerSystem.IsRageActivated)
            {
                Debug.Log("Esta enfadado y no quiere comer");
                return;
            }
            int saciedad = dish.GetSaciedad();
            animator.SetTrigger("eating");
            SpawEatingParticles();

            // Comprobamos si el plato tiene lo que el panda quiere
            if (!string.IsNullOrEmpty(pedidoDeseado) && !dish.TieneIngrediente(pedidoDeseado))
            {
                // Si NO lo tiene, la saciedad se reduce a la mitad
                saciedad = saciedad / 2;
                Debug.Log("El plato no tiene " + pedidoDeseado + ". Penalización aplicada. Saciedad final: " + saciedad);
            }
            else
            {
                Debug.Log("El plato le gusta. Saciedad completa: " + saciedad);
            }
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
        if (hungerSystem.IsRageActivated)
        {
            RageSystem rage = GetComponent<RageSystem>();

            if (rage != null)
            {
                rage.ReducirIraPorcentaje(porcentajecalmado); 
                Debug.Log("Has calmado al panda ");
            }
        }
        else
        {
            Debug.Log("El panda está tranquilo");
            Debug.Log("el panda: si no me muevo no me ve");
        }
    }

    public void ActualizarPedidoDebug()
    {
        PandaRequest pReq = GameManager.instance.GetComponent<PandaRequest>();
        List<string> pedidos = pReq.GetCurrentRequests();
        if (pedidos.Count > indicePanda)
        {
            pedidoDeseado = pedidos[indicePanda];
        }
    }

    private void SpawEatingParticles()
    {
        eatingParticlesInstance = Instantiate(eatingParticles, spawnpoint.position, Quaternion.identity);
    }
}
