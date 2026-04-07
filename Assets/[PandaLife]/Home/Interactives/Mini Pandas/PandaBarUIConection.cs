using UnityEngine;
using UnityEngine.UI;

public class PandaBarUIConection : MonoBehaviour
{
    public Slider slider;
    private HungerSystem hunger;

    public void SetTarget(HungerSystem target)
    {
        hunger = target;
    }

    void Update()
    {
        if (hunger != null)
        {
            slider.maxValue = hunger.MaxValue;
            slider.value = hunger.CurrentValue;
        }
    }
}
