using System.Collections.Generic;
using UnityEngine;

public class RewardBagManager : MonoBehaviour
{
    [SerializeField] private List<RewardBagElement> elementslist = new List<RewardBagElement>();

    void Update()
    {
        EvaluateAllBags();
    }

    private void EvaluateAllBags()
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