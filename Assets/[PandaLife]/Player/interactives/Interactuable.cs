using UnityEngine;

public abstract class Interactuable : MonoBehaviour , IInteractuable
{
    protected string m_name;
    public abstract void Interactuar();
}
