using UnityEngine;

public class PinCloseExclamation : PinUIElement
{
    public override bool CheckCondition()
    {
        return  TutorialManager.instance.currentStepRecipe == TutorialManager.TutorialRecipeBook.CloseRedDragon ||
                TutorialManager.instance.currentStepRecipe == TutorialManager.TutorialRecipeBook.CloseUchuva;
    }
}
