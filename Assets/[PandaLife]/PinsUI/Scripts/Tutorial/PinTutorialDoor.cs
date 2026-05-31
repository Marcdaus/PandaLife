using UnityEngine;

public class PinTutorialDoor : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.SalirDeCasa;
    }
}
