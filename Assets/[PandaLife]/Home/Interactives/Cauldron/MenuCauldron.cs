using UnityEngine;

public class MenuCauldron : MonoBehaviour
{
    [SerializeField] private GameObject panelcauldron;

    private void Start()
    {
        panelcauldron.SetActive(false);
    }

    private void Update()
    {
        if(panelcauldron.activeSelf && Input.GetButtonDown("SalirMenuCaldero"))
        {
            CloseCauldron();
        }
    }

    public void OpenCauldron()
    {
        panelcauldron.SetActive(true);
    }

    public void CloseCauldron()
    {
        panelcauldron.SetActive(false);
    }
}
