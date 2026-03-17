using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : Interactuable
{
    // Campos
    [SerializeField] private int scenenumber = 1; // Variable que contendrá el número de la escena a cargar
    private Player player;
    private PickupDrop pickupobject;

    // Función donde se encuentran los objetos
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        pickupobject = FindFirstObjectByType<PickupDrop>();
    }

    // Función interactuar que comprueba si tiene el cubo o un plato en la mano.
    public override void Interactuar()
    {
        if (player.IsHoldingBucket() || player.IsHoldingDish())
        {
            Debug.Log("Deja el cubo antes de entrar en casa");
            // Suelta el cubo
            pickupobject.Drop();
            // Llama a la corutina
            StartCoroutine(EsperarParaCargar());
        }
        else 
        {
            SceneManager.LoadScene(scenenumber);
        }


    }

    // Corutina que dará un pequeño tiempo para ver cómo cae el cubo. 
    IEnumerator EsperarParaCargar()
    {
        // Pausa de 2 segundos
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scenenumber);
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