using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minipandas : Interactuable
{
    private HungerSystem hungerSystem;
    private Animator animator;

    [SerializeField] private int indicePanda;
    [SerializeField] private string pedidoDeseado;
    [SerializeField] private float porcentajecalmado;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private ParticleSystem eatingParticles;

    [SerializeField] private PinUIElement myRequestPin;
    [SerializeField] private float transitionTime = 0.5f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlaySFX eatingaudiosource;
    [SerializeField] private AudioClip soundCorrectDish;
    [SerializeField] private AudioClip soundWrongDish;
    [SerializeField] private AudioClip soundNewRequest;
    [SerializeField] private AudioClip soundCloseRequest;


    void Awake()
    {
        hungerSystem = GetComponent<HungerSystem>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (GameManager.instance != null)
        {
            PandaRequest pandaReq = GameManager.instance.GetComponent<PandaRequest>();
            animator = GetComponent<Animator>();

            if (pandaReq != null)
            {
                List<string> pedidosActuales = pandaReq.GetCurrentRequests();
                if (pedidosActuales.Count > indicePanda)
                {
                    pedidoDeseado = pedidosActuales[indicePanda];
                }
            }
        }
    }

    // Textos dinámicos
    public override string GetActionText(Player player)
    {
        if (hungerSystem != null && hungerSystem.IsRageActivated) return "Acariciar";
        if (!player.IsHandEmpty() && player.pickedobject.GetComponentInParent<Dish>() != null) return "Alimentar";
        return "Interactuar";
    }

    // Animaciones dinámicas
    public override string GetAnimationTrigger(Player player)
    {
        if (hungerSystem != null && hungerSystem.IsRageActivated) return "Pet";
        return "PickUp"; // Animación de alimentar
    }

    // Condición negar con la cabeza
    public override bool ShouldShakeHead(Player player)
    {
        if (hungerSystem == null) return false;

        if (hungerSystem.IsRageActivated)
        {
            // Para acariciar necesita las manos vacías
            return !player.IsHandEmpty();
        }
        else
        {
            // Para alimentar necesita tener un plato en la mano
            if (player.IsHandEmpty()) return true;
            Dish dish = player.pickedobject.GetComponentInParent<Dish>();
            if (dish == null) return true; // Tiene algo, pero no es un plato
        }

        return false;
    }

    public override void Interactuar(Player player)
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
            if (player.IsHandEmpty()) return;

            Dish dish = player.pickedobject.GetComponentInParent<Dish>();
            if (dish != null)
            {
                StartCoroutine(SequenceChangeRequest(dish, player));
            }
        }
    }

    private IEnumerator SequenceChangeRequest(Dish dish, Player player)
    {
        // Ocultar el pin actual
        if (myRequestPin != null)
        {
            myRequestPin.SetTransitionState(true);
            myRequestPin.Hide();
        }

        yield return new WaitForSeconds(transitionTime);

        // Comer (animación, partículas, sonidos, saciedad)
        int saciedad = dish.GetSaciedad();
        animator.SetTrigger("eating");
        SpawEatingParticles(dish.GetColor());

        string nombreDelPlatoEvaluado = dish.GetNombre();
        if (!string.IsNullOrEmpty(pedidoDeseado) && nombreDelPlatoEvaluado != pedidoDeseado)
        {
            saciedad /= 2;
            if (audioSource != null && soundWrongDish != null)
            {
                audioSource.PlayOneShot(soundWrongDish);
                audioSource.PlayOneShot(soundCloseRequest);

                eatingaudiosource.Eating();
            }
        }
        else
        {
            if (audioSource != null && soundCorrectDish != null)
            {
                audioSource.PlayOneShot(soundCorrectDish);
                audioSource.PlayOneShot(soundCloseRequest);
                eatingaudiosource.Eating();
            }
        }

        hungerSystem.Restaurar(saciedad);
        hungerSystem.PauseHunger(5f);

        // Destruir el plato para liberar al jugador
        Destroy(player.pickedobject.gameObject);
        player.SetPickedObject(null);

        // Esperar 3 segundos mientras el panda mastica y el jugador ya es libre
        yield return new WaitForSeconds(3f);

        // Generar el nuevo pedido
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

        // Volver a mostrar el pin con el nuevo pedido y su sonido
        if (myRequestPin != null)
        {
            myRequestPin.SetTransitionState(false);
            myRequestPin.Show();
            if (audioSource != null && soundNewRequest != null) audioSource.PlayOneShot(soundNewRequest);
        }
    }

    public void ActualizarPedidoDebug()
    {
        PandaRequest pReq = GameManager.instance.GetComponent<PandaRequest>();
        List<string> pedidos = pReq.GetCurrentRequests();
        if (pedidos.Count > indicePanda) pedidoDeseado = pedidos[indicePanda];
    }

    private void SpawEatingParticles(Color color)
    {
        foreach (Transform point in spawnPoints)
        {
            ParticleSystem instance = Instantiate(eatingParticles, point.position, Quaternion.identity, point);
            var main = instance.main;
            main.startColor = color;
        }
    }
}