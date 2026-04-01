using UnityEngine;

public class PinTutorialBucket : PinUIElement
{
    public override bool CheckCondition()
    {
        return !GameManager.instance.tutorialCuboCompletado;
    }
}
