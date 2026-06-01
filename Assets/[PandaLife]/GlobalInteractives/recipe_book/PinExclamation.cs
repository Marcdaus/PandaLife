using UnityEngine;

public class PinExclamation : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.CogerSaco;
    }
}
