using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : Interactuable
{
    // Campos
    [SerializeField] private GameString scenename; // Variable que contendrá el nombre de la escena a cargar
    private Player player;
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
            GuardarPlatoSueltoSiExiste(limpiarAntes: true);
            SceneManager.LoadScene(scenename.Value);
        }
    }

    IEnumerator EsperarParaCargar()
    {
        // Guardar sueltos SIN limpiar, porque el plato en mano ya está en la lista
        GuardarPlatoSueltoSiExiste(limpiarAntes: false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scenename.Value);
    }

    private void GuardarPlatoSueltoSiExiste(bool limpiarAntes)
    {
        if (CauldronPersistenceManager.instance == null) return;

        Dish[] platos = FindObjectsByType<Dish>(FindObjectsSortMode.None);

        List<Dish> platosSueltos = new List<Dish>();
        foreach (Dish dish in platos)
        {
            GameObject raiz = dish.transform.root.gameObject;
            if (player.pickedobject != null &&
                raiz == player.pickedobject.transform.root.gameObject) continue;
            platosSueltos.Add(dish);
        }

        if (platosSueltos.Count == 0) return;

        if (limpiarAntes)
            CauldronPersistenceManager.instance.ClearAllDishStates();

        foreach (Dish dish in platosSueltos)
        {
            GameObject raiz = dish.transform.root.gameObject;
            Debug.Log($"[SceneChange] Guardando plato suelto: {raiz.name} en {raiz.transform.position}");
            CauldronPersistenceManager.instance.SaveDishState(
                dish.GetReceta(),
                raiz.transform.position,
                inHand: false
            );
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