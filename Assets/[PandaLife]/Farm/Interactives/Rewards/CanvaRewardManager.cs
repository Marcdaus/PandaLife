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

        // =================================================================
        // LOGICÁ DEL DÍA 2 (Red Dragon y opcionalmente Nota)
        // =================================================================
        if (GameManager.instance.numday == 2)
        {
            // 1. Ejecutar Saco Red Dragon (Si no se ha mostrado ya)
            if (!GameManager.instance.animacionRedDragonMostrada)
            {
                GameManager.instance.animacionRedDragonMostrada = true;

                if (anim != null)
                {
                    anim.SetTrigger("Reddragon"); // Trigger exacto de tu imagen
                    Debug.Log("[Recompensas] Mostrando Saco Red Dragon");
                }

                // Esperamos a que la animación del saco termine en pantalla
                yield return new WaitForSeconds(tiempoEsperaAnimacion);

                // RESET MANUAL: Forzamos al Animator a volver a 'nothing' para limpiar la cola
                ResetearAEstadoReposo();
            }

            // 2. Si tiene los 3 pandas con hambre, ejecutamos la Nota inmediatamente después
            if (GameManager.instance.miniPandasHambrientos == 3 && !GameManager.instance.animacionNoteMostrada)
            {
                GameManager.instance.animacionNoteMostrada = true;

                if (anim != null)
                {
                    anim.SetTrigger("Note"); // Trigger exacto de tu imagen
                    Debug.Log("[Recompensas] Condición cumplida: Mostrando Nota");
                }

                yield return new WaitForSeconds(tiempoEsperaAnimacion);
                ResetearAEstadoReposo();
            }
        }

        // =================================================================
        // LOGICÁ DEL DÍA 3 (Uchuva y opcionalmente Teddy)
        // =================================================================
        else if (GameManager.instance.numday == 3)
        {
            // 1. Ejecutar Saco Uchuva (Si no se ha mostrado ya)
            if (!GameManager.instance.animacionUchuva)
            {
                GameManager.instance.animacionUchuva = true;

                if (anim != null)
                {
                    anim.SetTrigger("Uchuva"); // Trigger exacto de tu imagen
                    Debug.Log("[Recompensas] Mostrando Saco Uchuva");
                }

                yield return new WaitForSeconds(tiempoEsperaAnimacion);
                ResetearAEstadoReposo();
            }

            // 2. Si tiene los 3 pandas con hambre, ejecutamos el Teddy inmediatamente después
            if (GameManager.instance.miniPandasHambrientos == 3 && !GameManager.instance.animacionTeddyMostrada)
            {
                GameManager.instance.animacionTeddyMostrada = true;

                if (anim != null)
                {
                    anim.SetTrigger("Teddy"); // Trigger exacto de tu imagen
                    Debug.Log("[Recompensas] Condición cumplida: Mostrando Teddy");
                }

                yield return new WaitForSeconds(tiempoEsperaAnimacion);
                ResetearAEstadoReposo();
            }
        }
    }

    /// <summary>
    /// Fuerza al Animator a regresar instantáneamente al estado base para poder recibir otro Trigger.
    /// </summary>
    private void ResetearAEstadoReposo()
    {
        if (anim != null)
        {
            // "nothing" es el nombre exacto de tu estado naranja en el Animator
            anim.Play("nothing", 0, 0f);
        }
    }
}