using UnityEngine;

public interface IBarSystem
{
    void Activate();      // activar sistema
    void Deactivate();    // desactivar sistema
    void UpdateSystem();  
    void UpdateUI();      // actualizar UI
}
