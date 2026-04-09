using UnityEngine;

public class StatsMinipanda : MonoBehaviour
{
    [SerializeField]private HungerSystem hungerSystem;
    [SerializeField]private RageSystem rageSystem;

    [SerializeField]private PandaBarUIConection barraUI;

    void Start()
    {
    if (hungerSystem == null)
        hungerSystem = GetComponent<HungerSystem>();

    if (barraUI != null)
        barraUI.SetHunger(hungerSystem);
    else
        Debug.LogError("barraUI no asignada en " + gameObject.name);    }
}
