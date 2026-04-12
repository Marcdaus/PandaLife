using UnityEngine;

public class BlueberryBag : RewardElement
{
    public override bool CheckCondition()
    {
        if (GameManager.instance == null) return false;

        if (GameManager.instance.numday >= 1)
        {
            return true;
        }
        return false;
    }
}