using UnityEngine;

public class PinTutorialCauldron : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.Caldero;
    }
}
