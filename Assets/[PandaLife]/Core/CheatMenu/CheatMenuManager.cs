using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatMenuManager : MonoBehaviour
{
    private GameObject cheatPanel;
    [SerializeField] private MenuCauldron menucauldron;
    [SerializeField] private DayNightCycle daynightcycle;

    private float originalHungerChangeRate = 0f;

    void Start()
    {
        // Buscamos el Canvas persistente usando su Singleton, y obtenemos el primer hijo (el Panel)
        if (CheatMenuPersistent.instance != null)
        {
            cheatPanel = CheatMenuPersistent.instance.transform.GetChild(0).gameObject;
        }

        if (BarraManager.Instancia != null)
        {
            originalHungerChangeRate = BarraManager.Instancia.HungerChangeRate;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("CheatMenu")) // He puesto en el input manager la tecla K
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (cheatPanel != null)
        {
            bool isMenuOpen = !cheatPanel.activeSelf;
            cheatPanel.SetActive(isMenuOpen);
        }
    }

    public void Cheat_DarBambu()
    {
        GameManager.instance.sumarBambu(10, 1);
        GameManager.instance.sumarBambu(10, 2);
        GameManager.instance.sumarBambu(10, 3);
        GameManager.instance.sumarBambu(10, 4);
        Debug.Log("CHEAT: +10 Bambú");
    }
    public void Cheat_QuitarBambu()
    {
        GameManager.instance.quitarBambu();

        Debug.Log("CHEAT: 0 Bambú");
    }

    public void Cheat_acelerardia()
    {
        if(GameManager.instance.multiplicadorVelocidad == 1f)
        {
            GameManager.instance.multiplicadorVelocidad = 20f;
            Debug.Log("CHEAT: multiplicador de velocidad activado");
        }
        else
        {
            GameManager.instance.multiplicadorVelocidad = 1f;
            Debug.Log("CHEAT: multiplicador de velocidad desactivado");
        }

    }
    
    public void Cheat_adelantardia()
    {
        GameManager.instance.tiempoTranscurrido = 280.0f;
    }
    public void Cheat_Volver_dia1()
    {
        GameManager.instance.tedypersistente = false;
        GameManager.instance.notepersistente = false;
        GameManager.instance.numeroDia = 1;
        GameManager.instance.quitarBambu();
        SceneManager.LoadScene("Home");
    }

    /*public void Cheat_cambiarDia(int dia)
    {
        Debug.Log("Has seleccionado el día: " + dia);

        switch (dia)
        {
            case 1:
                GameManager.instance.numeroDia = 1;
                break;
            case 2:
                GameManager.instance.numeroDia = 2;
                break;
            case 3:
                GameManager.instance.numeroDia = 3;
                break;
        }
        daynightcycle.Rewards("Bags");
        daynightcycle.Rewards("Collectables");

        MenuCauldron menuCauldron = FindAnyObjectByType<MenuCauldron>();
        if (menuCauldron != null)
            menuCauldron.RefreshCards();
    }*/
    // 🔹 Cheat: Poner a los 3 pandas en estado de ira
    public void Cheat_PandasIraMaxima()
    {
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.RageActivated = true;
            BarraManager.Instancia.RageCurrentValue = BarraManager.Instancia.RageMaxValue;
        }

        Debug.Log("CHEAT: Todos los pandas en ira máxima");
    }

    // 🔹 Cheat: Pausar o reanudar la disminución de hambre
    public void Cheat_PausarOReanudarHambre()
    {
        if (BarraManager.Instancia == null) return;

        if (BarraManager.Instancia.HungerChangeRate != 0f)
        {
            // Pausar hambre
            BarraManager.Instancia.HungerChangeRate = 0f;
            Debug.Log("CHEAT: Disminución de hambre pausada");
        }
        else
        {
            // Reanudar hambre
            BarraManager.Instancia.HungerChangeRate = originalHungerChangeRate;
            Debug.Log("CHEAT: Disminución de hambre reanudada");
        }
    }

}