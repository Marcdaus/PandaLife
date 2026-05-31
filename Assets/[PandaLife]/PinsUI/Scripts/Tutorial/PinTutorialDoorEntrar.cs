using UnityEngine;

public class PinTutorialDoorEntrar : PinUIElement
{
    public override bool CheckCondition()
    {
        return TutorialManager.instance.currentStep == TutorialManager.TutorialStep.EntrarEnCasa;
    }
}
