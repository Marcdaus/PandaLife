using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cauldron : Interactuable
{
    [SerializeField] private MenuCauldron cauldronmenuUI;
    [SerializeField] private Transform displayPoint;

    private GameObject platopendiente = null;
    private RecipesData recetaPendiente;

    [Header("Recursos insuficientes")]
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private string message;
    [SerializeField] private float tiempoMensaje = 3.0f;
    private Coroutine mensajeCoroutine;
    [SerializeField] private Animator anim;
    [Header("Sonidos")]
    [SerializeField] private AudioSource finishdish;
    public GameObject PlatoPendienteGameObject => platopendiente;
    public bool tieneplatopendiente => platopendiente != null;

    // Textos dinámicos
    public override string GetActionText(Player player)
    {
        if (tieneplatopendiente) return "Recoger plato";
        return interactData != null ? interactData.actionText : "Abrir menú";
    }

    // Animaciones dinámicas
    public override string GetAnimationTrigger(Player player)
    {
        if (tieneplatopendiente) return "PickUp"; // Animación de recoger el plato
        return "Interactuar"; // Animación de pulsar/abrir
    }

    // Condición negar con la cabeza
    public override bool ShouldShakeHead(Player player)
    {
        // El jugador no puede usar el caldero para cocinar ni recoger platos si tiene las manos ocupadas
        return !player.IsHandEmpty();
    }

    // Interacción unificada
    public override void Interactuar(Player player)
    {
        
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.Caldero);
        }
        if (!player.IsHandEmpty()) return;

        if (!Checkresources(message, Text)) return;

        // Si hay un plato pendiente de recoger, dárselo al jugador
        if (platopendiente != null)
        {
            GiveDish(player);
            return;
        }
     
        cauldronmenuUI.OpenCauldron();

    }

    public void SpawnDish(GameObject prefab, RecipesData receta, bool menuabierto)
    {
        finishdish.Play();
        if (menuabierto)
        {
            Player player = FindFirstObjectByType<Player>(); // Buscamos al jugador localmente

            GameObject plato = Instantiate(prefab, player.handpoint.transform.position, Quaternion.identity);

            Dish dish = plato.GetComponentInChildren<Dish>();
            if (dish != null) dish.Initialize(receta);

            PickupDrop pickup = plato.GetComponentInChildren<PickupDrop>();
            if (pickup != null)
            {
                pickup.SetHandpoint(player.handpoint.transform);
                pickup.PickUp();
                player.SetPickedObject(pickup); // decirle al jugador que tiene el plato
            }
            cauldronmenuUI.CloseCauldron();
            Debug.Log("saciedad de plato en caldero:" + dish.GetSaciedad());
        }
        else
        {
            GameObject plato = Instantiate(prefab, displayPoint.position, displayPoint.rotation);

            Animator animPlato = plato.GetComponentInChildren<Animator>();
            if (animPlato != null)
            {
                animPlato.Play("Rodar");
            }
            Dish dish = plato.GetComponentInChildren<Dish>();
            if (dish != null) dish.Initialize(receta);

            platopendiente = plato;
            recetaPendiente = receta;

            Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
            Debug.Log("saciedad de plato:" + dish.GetSaciedad());
        }
    }

    public void RestoreFromPersistence()
    {
        var mgr = CauldronPersistenceManager.instance;
        if (mgr == null || !mgr.hasPendingDish) return;

        var dishes = new List<CauldronPersistenceManager.DishState>(mgr.PendingDishes);
        mgr.ClearAllDishStates();

        foreach (var state in dishes)
        {
            RecipesData receta = state.recipe;
            if (receta?.prefabResultado == null) continue;

            if (state.wasInHand)
            {
                if (platopendiente == null)
                {
                    GameObject plato = Instantiate(receta.prefabResultado, displayPoint.position, displayPoint.rotation);

                    Animator animPlato = plato.GetComponentInChildren<Animator>();
                    if (animPlato != null) animPlato.Play("Rodar");

                    Dish dish = plato.GetComponentInChildren<Dish>();
                    if (dish != null) dish.Initialize(receta);
                    Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
                    if (rb != null) rb.isKinematic = true;

                    platopendiente = plato;
                    recetaPendiente = receta;
                    Debug.Log($"[Cauldron] Restaurado plato caldero: {receta.nombrereceta}");
                }
            }
            else
            {
                GameObject plato = Instantiate(receta.prefabResultado, state.position, Quaternion.identity);
                Dish dish = plato.GetComponentInChildren<Dish>();
                if (dish != null) dish.Initialize(receta);
                Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    StartCoroutine(ActivarFisicaTrasFrame(rb));
                }
                Debug.Log($"[Cauldron] Restaurado plato suelto: {receta.nombrereceta} en {state.position}");
            }
        }
    }

    private IEnumerator ActivarFisicaTrasFrame(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);
        if (rb != null) rb.isKinematic = false;
    }

    private void GiveDish(Player player)
    {
        GameObject plato = platopendiente;
        plato.transform.position = player.handpoint.transform.position;
        plato.transform.rotation = Quaternion.identity;

        Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        PickupDrop pickup = plato.GetComponentInChildren<PickupDrop>();
        if (pickup != null)
        {
            pickup.SetHandpoint(player.handpoint.transform);
            pickup.PickUp();
            player.SetPickedObject(pickup);
        }

        platopendiente = null;
    }

    public bool Checkresources(string message, TextMeshProUGUI text)
    {
        if (GameManager.instance.bambuverde <= 0 && GameManager.instance.bamburojo <= 0 && GameManager.instance.bayaarandanos <= 0 && GameManager.instance.bayauchuva <= 0)
        {
            text.text = message;
            anim.SetTrigger("ShakeHead");

            if (mensajeCoroutine != null) StopCoroutine(mensajeCoroutine);

            mensajeCoroutine = StartCoroutine(OcultarTextoTrasTiempo(text, tiempoMensaje));
            return false;
        }
        return true;
    }

    private IEnumerator OcultarTextoTrasTiempo(TextMeshProUGUI textComponent, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (textComponent != null) textComponent.text = string.Empty;
    }
}