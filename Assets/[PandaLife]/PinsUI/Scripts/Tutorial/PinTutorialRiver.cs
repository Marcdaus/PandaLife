using UnityEngine;

public class PinTutorialRiver : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.SalirDeCasa;
    }
}
