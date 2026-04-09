using UnityEngine;

public class PinTutorialRiver : PinUIElement
{
    public override bool CheckCondition()
    {
        return !GameManager.instance.tutorialRioCompletado;
    }
}
