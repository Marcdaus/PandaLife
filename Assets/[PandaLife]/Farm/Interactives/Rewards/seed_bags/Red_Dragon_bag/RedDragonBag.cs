using UnityEngine;

public class RedDragonBag : RewardElement
{

    public override bool CheckCondition()
    {
        if (GameManager.instance == null) return false;

        if (GameManager.instance.numeroDia >= 2)
        {
            return true;
        }
        return false;
    }
}