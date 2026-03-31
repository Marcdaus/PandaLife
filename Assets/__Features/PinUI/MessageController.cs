using System.Collections.Generic;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    [SerializeField] private List<PinUIElement> elementsList = new List<PinUIElement>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleElements();
        }
    }

    private void ToggleElements()
    {
        // Recorremos cada elemento de la lista
        foreach (PinUIElement element in elementsList)
        {
            
                if (element.gameObject.activeSelf)
                {
                    element.Hide();
                }
                else
                {
                    element.Show();
                }
            
        }
    }
}