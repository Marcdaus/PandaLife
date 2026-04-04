using TMPro;
using UnityEngine;
using System.Collections;

public abstract class RewardBagElement : MonoBehaviour
{
    [SerializeField] protected int rewardDay;
    [SerializeField] protected GameObject bag;

    public abstract bool CheckCondition();

    public void Evaluate()
    {
        if (CheckCondition())
        {
            Show(); 
        }
    }

    public void Show()
    {
        if (bag != null)
        {
            bag.SetActive(true);
        }
    }
}