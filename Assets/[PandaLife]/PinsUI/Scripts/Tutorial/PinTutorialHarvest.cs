using UnityEngine;

public class PinTutorialHarvest : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.Cosechar;
    }
}
