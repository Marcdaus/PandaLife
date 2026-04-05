using UnityEngine;

public class Note : RewardElement
{

    public override bool CheckCondition()
    {
        if (GameManager.instance == null) return false;

        if (GameManager.instance.numeroDia >= 2 && GameManager.instance.miniPandasHambrientos == 3)
        {
            Debug.Log("Note desbloqueada");
            return true;
        }
        return false;
    }
}
