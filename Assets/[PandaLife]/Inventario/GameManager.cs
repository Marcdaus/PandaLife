using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header ("bambuVerde")]
    public int bambuverde = 0;
    public TextMeshProUGUI textoBambuVerde;

    void Start()
    {
       // CargarMonedas();
       bambuverde = 0;
        ActualizarInventarioUI();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
        //GuardarMonedas();
    }

    void ActualizarInventarioUI() //actualiza el textomonedas segun las monedas que tenemos
    {
        if(textoBambuVerde != null)
            textoBambuVerde.text = "x " + bambuverde.ToString();
    }

   /* void GuardarMonedas()
    {
        PlayerPrefs.SetInt("Monedas", monedas);
        PlayerPrefs.Save();
    }*/
   /* void CargarMonedas()
    {
        monedas = PlayerPrefs.GetInt("Monedas", 0);
    }*/


}
