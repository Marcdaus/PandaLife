using UnityEngine;

public class PinRequest : PinUIElement
{

    [SerializeField] private HungerSystem panda;

    public override bool CheckCondition()
    {
        if (panda == null)
        {
            return true;
        }

        return !panda.IsRageActivated;
    }
}
