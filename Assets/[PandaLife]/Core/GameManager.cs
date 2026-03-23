using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header ("bambuVerde")]
    public int bambuverde = 0;
    public int bamburojo = 0;
    public int bayauchuva = 0;
    public int bayaarandanos = 0;
    public TextMeshProUGUI textoBambuVerde;
    public TextMeshProUGUI textoBambuRojo;
    public TextMeshProUGUI textoBayaArandanos;
    public TextMeshProUGUI textoBayaUchuva;

    void Start()
    {
       //bambuverde = 0;
        ActualizarInventarioUI();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            bambuverde = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void sumarBambuVerde(int cantidad)
    {
        bambuverde += cantidad;
        ActualizarInventarioUI();
    }
    public void ActualizarUIManual()
    {
        ActualizarInventarioUI();
    }
    public void SetTextoBambu(TextMeshProUGUI texto)
    {
        textoBambuVerde = texto;
    }
    public void SetTextoBambuRojo(TextMeshProUGUI texto)
    {
        textoBambuRojo = texto;
    }
    public void SetTextoBayaUchuva(TextMeshProUGUI texto)
    {
        textoBayaUchuva = texto;
    }
    public void SetTextoBayaArandanos(TextMeshProUGUI texto)
    {
        textoBayaArandanos = texto;
    }

    void ActualizarInventarioUI() //actualiza el texto segun los recursos que tenemos
    {
        if(textoBambuVerde != null)
            textoBambuVerde.text = "x " + bambuverde.ToString();
    }

}
