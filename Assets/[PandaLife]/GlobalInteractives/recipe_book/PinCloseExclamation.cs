using UnityEngine;

public class PinCloseExclamation : PinUIElement
{
    public override bool CheckCondition()
    {
        return  TutorialManager.instance.currentStepRecipe == TutorialManager.TutorialRecipeBook.CloseRedDragon     ||
                TutorialManager.instance.currentStepRecipe == TutorialManager.TutorialRecipeBook.CloseRecipeBook    ||
                TutorialManager.instance.currentStepRecipe == TutorialManager.TutorialRecipeBook.CloseUchuva;
    }
    override public void Hide()
    {
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(false);

    }
}
