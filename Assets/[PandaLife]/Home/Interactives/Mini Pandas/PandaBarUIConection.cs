using UnityEngine;
using UnityEngine.UI;

public class PandaBarUIConection : MonoBehaviour
{
    public Slider slider;
    private HungerSystem hunger;
    private RageSystem rage;

    public void SetHunger(HungerSystem target)
{
    hunger = target;
    rage = null;
}

public void SetRage(RageSystem target)
{
    rage = target;
    hunger = null;
}
void Update()
{
    if (hunger != null)
    {
        slider.maxValue = hunger.MaxValue;
        slider.value = hunger.CurrentValue;
    }
    else if (rage != null)
    {
        slider.maxValue = rage.MaxValue;
        slider.value = rage.CurrentValue;
    }
}
}
