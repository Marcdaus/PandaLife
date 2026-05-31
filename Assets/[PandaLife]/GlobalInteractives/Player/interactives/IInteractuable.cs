using UnityEngine;

public interface IInteractuable
{
    // Le pasamos el Player para ver el objeto que lleva
    void Interactuar(Player player);

    // Si puede interactuar
    bool CanInteract(Player player);

    // Para activar animación de negación
    bool ShouldShakeHead(Player player);

    // Para sacar el texto, sonido y animación desde el ScriptableObject
    InteractableObject GetInteractData();
}