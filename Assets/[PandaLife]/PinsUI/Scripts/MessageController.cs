using System.Collections.Generic;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    [SerializeField] private List<PinUIElement> elementslist = new List<PinUIElement>();

    void Update()
    {
        EvaluateAllPins();
    }

    private void EvaluateAllPins()
    {
        foreach (PinUIElement element in elementslist)
        {
            if (element != null)
            {
                element.Evaluate();
            }
        }
    }
}