using System.Collections;
using UnityEngine;

public class CanvaRewardManager : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Start()
    {
        if (GameManager.instance == null) return;

        // --- COMPROBACIÓN DÍA 2 (Red Dragon) ---
        if (GameManager.instance.numday == 2 && !GameManager.instance.animacionRedDragonMostrada)
        {
            // Le pasamos el nombre del trigger "Reddragon" y el tipo de recompensa 2
            StartCoroutine(PlayRewardAnimationRoutine("Reddragon", 2));
        }

        // --- COMPROBACIÓN DÍA 3 (Uchuva) ---
        else if (GameManager.instance.numday == 3 && !GameManager.instance.animacionUchuva)
        {
            // OJO: Asegúrate de que el nombre del Trigger en las condiciones de tu Animator 
            // sea exactamente igual al texto que pongas aquí (por ejemplo, "Uchuva")
            StartCoroutine(PlayRewardAnimationRoutine("Uchuva", 3));
        }
    }

    private IEnumerator PlayRewardAnimationRoutine(string triggerName, int day)
    {
        // 1. Marcamos la animación correspondiente como mostrada en el GameManager
        if (day == 2)
        {
            GameManager.instance.animacionRedDragonMostrada = true;
        }
        else if (day == 3)
        {
            GameManager.instance.animacionUchuva = true;
        }

        // 2. Esperamos un frame para que todo se asiente en la nueva escena
        yield return null;

        // 3. Lanzamos el trigger dinámico
        if (anim != null)
        {
            anim.SetTrigger(triggerName);
            Debug.Log($"[CanvaRewardManager] ¡Animación {triggerName} ejecutada por ÚNICA vez al comenzar el Día {day}!");
        }
        else
        {
            Debug.LogError("[CanvaRewardManager] Falta asignar el Animator en el CanvaRewardManager.");
        }
    }
}