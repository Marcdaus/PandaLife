using UnityEngine;

public abstract class Interactuable : MonoBehaviour , IInteractuable
{
    // Cada acción implementa su interacción
    public abstract void Interactuar();
    protected void Mensaje(string texto)
    {
        Debug.Log(texto);
    }
}
