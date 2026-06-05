using UnityEngine;

public class PinTutorialFlechaDerecha : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.Flecha1;
    }
}
