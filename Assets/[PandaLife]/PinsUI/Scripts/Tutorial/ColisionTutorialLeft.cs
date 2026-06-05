using UnityEngine;

public class ColisionTutorialLeft : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // La flecha que no le gusta a Minerva x2
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.CompleteStep(TutorialManager.TutorialStep.Flecha2);
        }
    }
}
