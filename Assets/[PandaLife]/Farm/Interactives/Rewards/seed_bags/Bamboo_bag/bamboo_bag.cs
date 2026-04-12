using UnityEngine;

public class Bamboo_bag : RewardElement
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