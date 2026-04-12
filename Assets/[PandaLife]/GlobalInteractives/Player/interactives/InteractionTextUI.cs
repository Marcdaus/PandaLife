using UnityEngine;
using TMPro;

public class InteractionTextUI : MonoBehaviour
{
    public static InteractionTextUI instance;

    [Header("Referencias de UI")]
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private TextMeshProUGUI textoInteraccion;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        OcultarMensaje();
    }

    public void MostrarMensaje(string accion)
    {
        // Cambiamos el texto
        if (textoInteraccion != null)
        {
            textoInteraccion.text = "E " + accion;
        }

        // Encendemos el panel completo
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(true);
        }
    }

    public void OcultarMensaje()
    {
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(false);
        }
    }
}