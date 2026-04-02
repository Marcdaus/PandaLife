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
        Debug.Log("CHEAT: +10 Bambú");
    }

}