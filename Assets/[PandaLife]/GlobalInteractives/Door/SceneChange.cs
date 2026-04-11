using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : Interactuable
{
    // Campos
    [SerializeField] private GameString scenename; // Variable que contendrá el nombre de la escena a cargar
    private Player player;
    private PickupDrop pickupobject;

    // Función donde se encuentran los objetos
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        //pickupobject = FindFirstObjectByType<PickupDrop>();
    }

    // Función interactuar que comprueba si tiene el cubo o un plato en la mano.
    public override void Interactuar()
    {
        if (player.IsHoldingBucket() || player.IsHoldingDish())
        {
            PickupDrop pickupobject = player.pickedobject;

            if (pickupobject != null)
            {
                Dish dish = pickupobject.GetComponentInChildren<Dish>();
                if (dish != null && CauldronPersistenceManager.instance != null)
                {
                    CauldronPersistenceManager.instance.SaveDishState(
                        dish.GetReceta(),
                        pickupobject.transform.position,
                        inHand: false
                    );
                }
                pickupobject.Drop();
            }

            StartCoroutine(EsperarParaCargar());
        }
        else
        {
            // Aunque no lleves nada en mano, buscar si hay un plato suelto en la escena
            GuardarPlatoSueltoSiExiste();
            SceneManager.LoadScene(scenename.Value);
        }
    }

    IEnumerator EsperarParaCargar()
    {
        // Guardar plato suelto antes de cambiar
        GuardarPlatoSueltoSiExiste();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scenename.Value);
    }

    private void GuardarPlatoSueltoSiExiste()
    {
        if (CauldronPersistenceManager.instance == null) return;

        Dish[] platos = FindObjectsByType<Dish>(FindObjectsSortMode.None);
        Debug.Log($"[SceneChange] Buscando platos sueltos: {platos.Length} encontrados");

        foreach (Dish dish in platos)
        {
            GameObject raiz = dish.transform.root.gameObject;

            // Ignorar el que lleva el jugador en mano
            if (player.pickedobject != null &&
                raiz == player.pickedobject.transform.root.gameObject) continue;

            Debug.Log($"[SceneChange] Guardando plato suelto: {raiz.name} en {raiz.transform.position}");
            CauldronPersistenceManager.instance.SaveDishState(
                dish.GetReceta(),
                raiz.transform.position,
                inHand: false
            );
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si es el player llama a interactuar
        if (other.CompareTag("Player"))
        {
            Interactuar();
        }
    }
}