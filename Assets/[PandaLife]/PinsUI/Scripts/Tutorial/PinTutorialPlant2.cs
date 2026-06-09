using UnityEngine;

public class PinTutorialPlant2 : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.Plantar2;
    }
}
