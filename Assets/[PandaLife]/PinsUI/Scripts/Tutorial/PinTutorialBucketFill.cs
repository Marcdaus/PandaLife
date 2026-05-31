using UnityEngine;

public class PinTutorialBucketFill : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.LlenarCubo;
    }
}
