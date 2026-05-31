using UnityEngine;

public class PinTutorialPlant : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.Plantar;
    }
}
