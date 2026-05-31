using UnityEngine;

public class PinTutorialBagTake : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.CogerSaco;
    }
}
