using UnityEngine;

public interface IBarSystem
{
    void Activate();      // activar sistema
    void Deactivate();    // desactivar sistema
    void UpdateSystem();  // lógica principal
    void UpdateUI();      // actualizar UI
}
