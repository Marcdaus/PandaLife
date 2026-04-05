using UnityEngine;

public class Note : RewardElement
{

    public override bool CheckCondition()
    {
        if (GameManager.instance == null) return false;

        if (GameManager.instance.numeroDia == 2 && GameManager.instance.miniPandasHambrientos == 3)
        {
            GameManager.instance.notepersistente = true;
            return true;
        }
        if (GameManager.instance.notepersistente)
        {
            return true;
        }
        return false;
    }
}
