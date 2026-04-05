using UnityEngine;

public class TeddyBear : RewardElement
{

    public override bool CheckCondition()
    {
        if (GameManager.instance == null) return false;

        if (GameManager.instance.numeroDia >= 3 && GameManager.instance.miniPandasHambrientos == 3)
        {
            Debug.Log("TeddyBear desbloqueado");
            return true;
        }
        return false;
    }
}
