using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cauldron : Interactuable
{
    [SerializeField] private MenuCauldron cauldronmenuUI;

    [SerializeField] private Transform handpoint;
    [SerializeField] private Player jugador;
    [SerializeField] private Transform displayPoint;

    private GameObject platopendiente = null;
    private RecipesData recetaPendiente;

    [Header("Recursos insuficientes")]
    [SerializeField]private TextMeshProUGUI Text;
    [SerializeField] private string message;
    [SerializeField] private float tiempoMensaje = 3.0f;
    private Coroutine mensajeCoroutine;
    [SerializeField] private Animator anim;

    public GameObject PlatoPendienteGameObject => platopendiente;

    public bool tieneplatopendiente => platopendiente != null;

    public override void Interactuar()
    {
        if (!Checkresources(message, Text)) return;
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

            Animator animPlato = plato.GetComponentInChildren<Animator>();
            if (animPlato != null)
            {
                animPlato.Play("Rodar");
            }
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
                    if (animPlato != null)
                    {
                        animPlato.Play("Rodar"); // Que siga rodando al volver a entrar en la escena si sigue encima del caldero
                    }

                    Dish dish = plato.GetComponentInChildren<Dish>();
                    if (dish != null) dish.Initialize(receta);
                    Rigidbody rb = plato.GetComponentInChildren<Rigidbody>();
                    if (rb != null) rb.isKinematic = true;
                    platopendiente = plato;
                    recetaPendiente = receta;
                    mensajeInteraccion = "recoger plato";
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
    public bool Checkresources(string message, TextMeshProUGUI text)
    {
        if (GameManager.instance.bambuverde <= 0 && GameManager.instance.bamburojo <= 0 && GameManager.instance.bayaarandanos <= 0 && GameManager.instance.bayauchuva <= 0)
        {
            text.text = message;
            anim.SetTrigger("ShakeHead");

            //reiniciar corrutina
            if (mensajeCoroutine != null)
            {
                StopCoroutine(mensajeCoroutine);
            }
            //iniciar corrutina 
            mensajeCoroutine = StartCoroutine(OcultarTextoTrasTiempo(text, tiempoMensaje));
            return false;
        }
        return true;
    }
    private IEnumerator OcultarTextoTrasTiempo(TextMeshProUGUI textComponent, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (textComponent != null)
        {
            textComponent.text = string.Empty; // Borra el texto de la pantalla
        }
    }


}
