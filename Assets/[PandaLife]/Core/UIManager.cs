using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoBambuVerde;
    [SerializeField] private TextMeshProUGUI textoBambuRedDragon;
    [SerializeField] private TextMeshProUGUI textoBayaBlueberry;
    [SerializeField] private TextMeshProUGUI textoBayaUchuva;

    void Start()
    {
        GameManager.instance.SetTextoBambu(textoBambuVerde);
        GameManager.instance.SetTextoBambuRojo(textoBambuRedDragon);
        GameManager.instance.SetTextoBayaArandanos(textoBayaBlueberry);
        GameManager.instance.SetTextoBayaUchuva(textoBayaUchuva);

        
        GameManager.instance.ActualizarUIManual();
    }
}