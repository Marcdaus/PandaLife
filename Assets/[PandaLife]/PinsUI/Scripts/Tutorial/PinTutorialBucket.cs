using UnityEngine;

public class PinTutorialBucket : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.SalirDeCasa;
    }
}
