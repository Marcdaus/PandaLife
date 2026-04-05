using TMPro;
using UnityEngine;
using System.Collections;

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

    [Header("Variables de Tutorial")]
    public bool tutorialCuboCompletado = false;
    public bool tutorialRioCompletado = false;

    [Header("Variables de Sistema de dia")]
    public float tiempoTranscurrido = 0f;
    public float minutosActualesTotales;
    public int numeroDia = 1;
    public float multiplicadorVelocidad = 1f;

    [Header("Recompensas")]
    public TextMeshProUGUI messageRewardSacks;
    public TextMeshProUGUI messageRewardCollectable;

    [Header("Mini pandas")]
    public int miniPandasHambrientos = 3;

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
            bamburojo = 0;
            bayaarandanos = 0;
            bayauchuva = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void sumarBambu(int cantidad, int tipo)
    {
        
        if (tipo == 1)
        {
            bambuverde += cantidad;
            ActualizarInventarioUI();
        }
        else if (tipo == 2)
        {
            bamburojo += cantidad;
            ActualizarInventarioUI();
        }
        else if(tipo == 3)
        {
            bayaarandanos += cantidad;
            ActualizarInventarioUI();
        }
        else if(tipo == 4)
        {
            bayauchuva += cantidad;
            ActualizarInventarioUI();
        }
        else
        {
            return;
        }
        
    }
    public void quitarBambu()
    {
       bambuverde = 0;
       bamburojo = 0;
       bayaarandanos = 0;
       bayauchuva = 0;
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
        if (textoBambuVerde != null)
            textoBambuVerde.text = "x " + bambuverde.ToString();
        if (textoBambuRojo != null)
            textoBambuRojo.text = "x " + bamburojo.ToString();
        if (textoBayaArandanos != null)
            textoBayaArandanos.text = "x " + bayaarandanos.ToString();
        if (textoBayaUchuva != null)
            textoBayaUchuva.text = "x " + bayauchuva.ToString();
    }
    public void MostrarMensajeTemporal(string mensaje, float duracion, string type)
    {
        StartCoroutine(MensajeRoutine(mensaje, duracion, type));
    }
    //Recompensas textos temporales
    private IEnumerator MensajeRoutine(string mensaje, float duracion, string type)
    {
        if (type == "Bags")
        {            messageRewardSacks.text = mensaje;
            messageRewardSacks.gameObject.SetActive(true);
            yield return new WaitForSeconds(duracion);
            messageRewardSacks.text = "";
            messageRewardSacks.gameObject.SetActive(false);
        }
        else if (type == "Collectables")
        {
            messageRewardCollectable.text = mensaje;
            messageRewardCollectable.gameObject.SetActive(true);
            yield return new WaitForSeconds(duracion);
            messageRewardCollectable.text = "";
            messageRewardCollectable.gameObject.SetActive(false);
        }
    }

}
