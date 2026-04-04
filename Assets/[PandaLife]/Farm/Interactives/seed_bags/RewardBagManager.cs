using System.Collections.Generic;
using UnityEngine;

public class RewardBagManager : MonoBehaviour
{

    public static void EvaluateAllBags(List<RewardBagElement> elementslist)
    {
        foreach (RewardBagElement element in elementslist)
        {
            if (element != null)
            {
                element.Evaluate();
            }
        }
    }
}