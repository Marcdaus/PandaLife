using System.Collections.Generic;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    [SerializeField] private List<PinUIElement> elementsList = new List<PinUIElement>();

    void Update()
    {
        EvaluateAllPins();
    }

    private void EvaluateAllPins()
    {
        foreach (PinUIElement element in elementsList)
        {
            if (element != null)
            {
                element.Evaluate();
            }
        }
    }
}