using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public bool cursorblock = false;
    public static CursorManager Instancia { get; private set; }

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameOver" || SceneManager.GetActiveScene().name == "Theend")
        {
            MostrarCursor();
            cursorblock = true;
        }
        if (!cursorblock)
        {
            // mostrar cursor si se selecciona la tecla escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MostrarCursor();
            }

            // clic en la pantalla, volver a ocultarlo
            if (Input.GetMouseButtonDown(0) && Cursor.visible)
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
