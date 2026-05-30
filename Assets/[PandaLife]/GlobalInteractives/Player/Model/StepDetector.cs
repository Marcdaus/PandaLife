using UnityEngine;

public class StepDetector : MonoBehaviour
{
    [SerializeField] private StepsPlayer scriptDelPadre;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (scriptDelPadre != null)
        {
            scriptDelPadre.ActualizarSueloDesdeHijo(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (scriptDelPadre != null)
        {
            scriptDelPadre.ActualizarSueloDesdeHijo(other);
        }
    }
}