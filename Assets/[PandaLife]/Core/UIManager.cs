using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoBambuVerde;

    void Start()
    {
        GameManager.instance.SetTextoBambu(textoBambuVerde);
        GameManager.instance.ActualizarUIManual();
    }
}