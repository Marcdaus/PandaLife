using UnityEngine;

public class CheatMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject cheatPanel;

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


}