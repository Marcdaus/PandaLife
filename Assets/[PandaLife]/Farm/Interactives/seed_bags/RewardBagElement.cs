using UnityEngine;

public abstract class RewardBagElement : MonoBehaviour
{
    [SerializeField] protected int rewardDay;
    [SerializeField] protected GameObject bag;

    public abstract void MostrarMensaje();
    public abstract bool CheckCondition();

    public void Evaluate()
    {
        if (CheckCondition())
        {
            MostrarMensaje();
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