using UnityEngine;

public class PinTutorialDoorSalir : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.SalirDeCasa;
    }
}
