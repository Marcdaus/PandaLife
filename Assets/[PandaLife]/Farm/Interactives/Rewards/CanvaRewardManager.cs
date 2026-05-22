using System.Collections;
using UnityEngine;

public class CanvaRewardManager : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [Header("Tiempos de Espera (Duración de la animación)")]
    [SerializeField] private float tiempoEsperaAnimacion = 3.0f;

    private void Start()
    {
        if (GameManager.instance == null) return;

        // Arrancamos la secuencia de recompensas
        StartCoroutine(SecuenciaRecompensasRoutine());
    }

    private IEnumerator SecuenciaRecompensasRoutine()
    {
        // Esperamos un frame para que la escena cargue por completo
        yield return null;

     
        //  DÍA 2 (Red Dragon y opcionalmente Nota)

        if (GameManager.instance.numday == 2)
        {
            //  Ejecutar Saco Red Dragon (Si no se ha mostrado ya)
            if (!GameManager.instance.animacionRedDragonMostrada)
            {
                GameManager.instance.animacionRedDragonMostrada = true;

                if (anim != null)
                {
                    anim.SetTrigger("Reddragon"); 
                    Debug.Log("[Recompensas] Mostrando Saco Red Dragon");
                }

                // Esperamos a que la animación del saco termine en pantalla
                yield return new WaitForSeconds(tiempoEsperaAnimacion);

                
                ResetearAEstadoReposo();
            }

            //  3 pandas con hambre, ejecutamos la Nota inmediatamente después
            if (GameManager.instance.miniPandasHambrientos == 3 && !GameManager.instance.animacionNoteMostrada)
            {
                GameManager.instance.animacionNoteMostrada = true;

                if (anim != null)
                {
                    anim.SetTrigger("Note"); 
                    Debug.Log("[Recompensas] Condición cumplida: Mostrando Nota");
                }

                yield return new WaitForSeconds(tiempoEsperaAnimacion);
                ResetearAEstadoReposo();
            }
        }

     
        //  DÍA 3 (Uchuva y opcionalmente Teddy)
    
        else if (GameManager.instance.numday == 3)
        {
            //  Ejecutar Saco Uchuva (Si no se ha mostrado ya)
            if (!GameManager.instance.animacionUchuva)
            {
                GameManager.instance.animacionUchuva = true;

                if (anim != null)
                {
                    anim.SetTrigger("Uchuva"); 
                    Debug.Log("[Recompensas] Mostrando Saco Uchuva");
                }

                yield return new WaitForSeconds(tiempoEsperaAnimacion);
                ResetearAEstadoReposo();
            }

            //   3 pandas con hambre, ejecutamos el Teddy inmediatamente después
            if (GameManager.instance.miniPandasHambrientos == 3 && !GameManager.instance.animacionTeddyMostrada)
            {
                GameManager.instance.animacionTeddyMostrada = true;

                if (anim != null)
                {
                    anim.SetTrigger("Teddy"); 
                    Debug.Log("[Recompensas] Condición cumplida: Mostrando Teddy");
                }

                yield return new WaitForSeconds(tiempoEsperaAnimacion);
                ResetearAEstadoReposo();
            }
        }
    }

    private void ResetearAEstadoReposo()
    {
        if (anim != null)
        {
            anim.Play("nothing", 0, 0f);
        }
    }
}