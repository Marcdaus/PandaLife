using UnityEngine;

public class PinTutorialFlechaIzquierda : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.Flecha2;
    }
}
