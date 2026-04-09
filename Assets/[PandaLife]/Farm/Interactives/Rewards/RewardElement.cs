using TMPro;
using UnityEngine;
using System.Collections;

public abstract class RewardElement : MonoBehaviour
{

    [SerializeField] protected GameObject element;

    public abstract bool CheckCondition();

    public void Evaluate()
    {
        if (CheckCondition())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        if (element != null)
        {
            element.SetActive(true);
        }
    }
    public void Hide()
    {
        if (element != null)
        {
            element.SetActive(false);
        }
    }
}