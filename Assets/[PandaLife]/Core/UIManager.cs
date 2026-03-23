using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoBambuVerde;
    [SerializeField] private TextMeshProUGUI textoBambuRojo;
    [SerializeField] private TextMeshProUGUI textoBayaArandanos;
    [SerializeField] private TextMeshProUGUI textoBayaUchuva;

    void Start()
    {
        GameManager.instance.SetTextoBambu(textoBambuVerde);
        GameManager.instance.SetTextoBambu(textoBambuVerde);
        
        GameManager.instance.ActualizarUIManual();
    }
}