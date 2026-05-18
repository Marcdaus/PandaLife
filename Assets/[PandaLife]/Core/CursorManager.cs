using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public bool cursorblock = false;
    void Awake()
    {
        OcultarCursor();
    }

    void Update()
    {
        if (!cursorblock)
        {
            // mostrar cursor si se selecciona la tecla escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MostrarCursor();
            }

            // clic en la pantalla, volver a ocultarlo
            if (Input.GetMouseButtonDown(0) && !Cursor.visible)
            {
                OcultarCursor();
            }
        }
    }

    public void OcultarCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla
    }

    public void MostrarCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Libera el cursor para que se mueva normalmente
    }
}
