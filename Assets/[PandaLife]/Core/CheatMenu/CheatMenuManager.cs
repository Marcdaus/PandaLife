using System.Security.Cryptography;
using UnityEngine;

public class CheatMenuManager : MonoBehaviour
{
    private GameObject cheatPanel;
    [SerializeField] private MenuCauldron menucauldron;

    void Start()
    {
        // Buscamos el Canvas persistente usando su Singleton, y obtenemos el primer hijo (el Panel)
        if (CheatMenuPersistent.instance != null)
        {
            cheatPanel = CheatMenuPersistent.instance.transform.GetChild(0).gameObject;
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
    public void Cheat_cambiarDia(int dia)
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
        DayNightCycle.Rewards("Bags");
        DayNightCycle.Rewards("Collectables");

        MenuCauldron menuCauldron = FindAnyObjectByType<MenuCauldron>();
        if (menuCauldron != null)
            menuCauldron.RefreshCards();
    }


}