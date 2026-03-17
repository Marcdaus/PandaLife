using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : Interactuable
{
    //Campos
    [SerializeField] int sceneNumber = 0; // Variable que contendrá el número de la escena a cargar;
    Player player;
    PickupDrop pickUpObject ;

    // Función donde se encuentran los objetos
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        pickUpObject = FindFirstObjectByType<PickupDrop>();
    }
    //Función interactuar que comprueba si tiene el cubo o un plato en la mano.
    public override void Interactuar()
    {
        if (player.IsHoldingBucket() || player.IsHoldingDish())
        {
            Debug.Log("Deja el cubo antes de entrar en casa");
            //Suelta el cubo
            pickUpObject.Drop();

            //Llama a la corutina
            StartCoroutine(EsperarParaCargar());                   
        }
        
            SceneManager.LoadScene(sceneNumber); 
                     
            
   

    }
    // Corutina que dará un pequeño tiempo para ver como cae el cubo. 
    IEnumerator EsperarParaCargar()
    {
        // Pausa  de 2 segundos
        yield return new WaitForSeconds(2f); 
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //Si es el player llama a interactuar
        if(other.CompareTag("Player"))
        {
            Interactuar();

        }
    }
}
