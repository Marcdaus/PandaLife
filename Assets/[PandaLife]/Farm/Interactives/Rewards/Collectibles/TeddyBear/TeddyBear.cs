using UnityEngine;

public class TeddyBear : RewardElement
{

    public override bool CheckCondition()
    {
        if (GameManager.instance == null) return false;

        if (GameManager.instance.numday == 3 && GameManager.instance.miniPandasHambrientos == 3)
        {
            GameManager.instance.tedypersistente = true;
            return true;
        }
        if (GameManager.instance.tedypersistente)
        {
            return true;
        }
        return false;
    }
}
