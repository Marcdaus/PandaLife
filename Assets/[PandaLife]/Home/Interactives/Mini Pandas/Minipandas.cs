using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minipandas : Interactuable
{

    private HungerSystem hungerSystem;

    [SerializeField] private int indicePanda; // 0, 1 o 2
    [SerializeField] private string pedidoDeseado; // Aquí guardaremos qué es lo que quiere ("bambu cocido", "sopa de bayas", etc.)
    [SerializeField] private float porcentajecalmado;
   [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private ParticleSystem eatingParticles;
    private ParticleSystem eatingParticlesInstance;
     private Animator animator;
    [SerializeField] private PinUIElement myRequestPin;
    [SerializeField] private float transitionTime = 0.5f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundCorrectDish; // Sonido de Ding
    [SerializeField] private AudioClip soundWrongDish;   // Sonido de error
    [SerializeField] private AudioClip soundNewRequest;  // Sonido al aparecer un nuevo pedido Pop

    void Awake()
    {
        hungerSystem = GetComponent<HungerSystem>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
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
        if (dish != null)
        {
            if (hungerSystem.IsRageActivated) return;

            
            StartCoroutine(SequenceChangeRequest(dish, player));
        }
    }

    private IEnumerator SequenceChangeRequest(Dish dish, Player player)
    {
        // Bloquear evaluación y Ocultar
        if (myRequestPin != null)
        {
            myRequestPin.SetTransitionState(true);
            myRequestPin.Hide();
        }

        // Esperar a que la animación de "Hide" termine
        yield return new WaitForSeconds(transitionTime);

        // Lógica de comer
        int saciedad = dish.GetSaciedad();
        animator.SetTrigger("eating");
        SpawEatingParticles(dish.GetColor());

        string nombreDelPlatoEvaluado = dish.GetNombre();
        if (!string.IsNullOrEmpty(pedidoDeseado) && nombreDelPlatoEvaluado != pedidoDeseado)
        {
            saciedad /= 2;
            // Reproducir sonido de Error
            if (audioSource != null && soundWrongDish != null)
            {
                audioSource.PlayOneShot(soundWrongDish);
            }
        }
        else
        {
            // Reproducir sonido Ding
            if (audioSource != null && soundCorrectDish != null)
            {
                audioSource.PlayOneShot(soundCorrectDish);
            }
        }

        hungerSystem.Restaurar(saciedad);
        hungerSystem.PauseHunger(5f);

        // Cambiar el pedido en el Data
        if (GameManager.instance != null)
        {
            PandaRequest pandaReq = GameManager.instance.GetComponent<PandaRequest>();
            if (pandaReq != null)
            {
                pandaReq.ReplaceRequestAtIndex(indicePanda);
                ActualizarPedidoDebug();

                RequestManager requestManager = FindAnyObjectByType<RequestManager>();
                if (requestManager != null) requestManager.ActualizarTextosManual();
            }
        }

        // Destruir objetos del jugador
        Destroy(player.pickedobject.gameObject);
        player.SetPickedObject(null);

        // Volver a mostrar con el nuevo pedido
        if (myRequestPin != null)
        {
            myRequestPin.SetTransitionState(false); // Desbloqueamos
            myRequestPin.Show(); // Forzamos la aparición
            if (audioSource != null && soundNewRequest != null)
            {
                audioSource.PlayOneShot(soundNewRequest);
            }
        }
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

    private void SpawEatingParticles(Color color)
    {
    foreach (Transform point in spawnPoints)
    {
        ParticleSystem instance =
            Instantiate(eatingParticles, point.position, Quaternion.identity, point);

        var main = instance.main;
        main.startColor = color;
    }
    }
}
