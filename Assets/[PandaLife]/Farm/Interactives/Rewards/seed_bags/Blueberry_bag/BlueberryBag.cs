using UnityEngine;

public class BlueberryBag : RewardElement
{
    public override bool CheckCondition()
    {
        if (GameManager.instance == null) return false;

        if (GameManager.instance.numeroDia >= 1)
        {
            return true;
        }
        return false;
    }
}