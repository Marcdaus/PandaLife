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
        //Debug.Log("UI usando HUNGER");
        slider.maxValue = hunger.MaxValue;
        slider.value = hunger.CurrentValue;
    }
    if (rage != null)
    {
        //Debug.Log("UI usando RAGE");
        slider.maxValue = rage.MaxValue;
        slider.value = rage.CurrentValue;
    }
}
}
