using UnityEngine;

public class StepDetector : MonoBehaviour
{
    [SerializeField] private StepsPlayer scriptDelPadre;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (scriptDelPadre != null)
        {
            Debug.Log("ENTER -> " + other.name);
            scriptDelPadre.ActualizarSueloDesdeHijo(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (scriptDelPadre != null)
        {
            Debug.Log("STAY -> " + other.name);
            scriptDelPadre.ActualizarSueloDesdeHijo(other);
        }
    }
}