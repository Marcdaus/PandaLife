using UnityEngine;

public abstract class Interactuable : MonoBehaviour , IInteractuable
{
    // Mensaje que se muestra en el UI
    public string mensajeInteraccion = "interactuar";
    // Cada acción implementa su interacción
    public abstract void Interactuar();
    protected void Mensaje(string texto)
    {
        Debug.Log(texto);
    }
}
