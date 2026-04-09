using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{

    public static void EvaluateAllElements(List<RewardElement> elementslist)
    {
        foreach (RewardElement element in elementslist)
        {
            if (element != null) element.Evaluate();

        }
    }
    public static void EvaluatelElement(RewardElement element)
    {
        if (element != null) element.Evaluate();
    }
}