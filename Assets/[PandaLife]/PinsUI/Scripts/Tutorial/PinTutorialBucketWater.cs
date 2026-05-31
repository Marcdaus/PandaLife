using UnityEngine;

public class PinTutorialBucketWater : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.RegarPlanta;
    }
}
