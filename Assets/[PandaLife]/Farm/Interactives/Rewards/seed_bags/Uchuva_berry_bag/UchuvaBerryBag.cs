using UnityEngine;

public class UchuvaBerryBag : RewardElement
{

    public override bool CheckCondition()
    {
        if (GameManager.instance == null) return false;

        if (GameManager.instance.numeroDia >= 3)
        {
            return true;
        }
        return false;
    }
}