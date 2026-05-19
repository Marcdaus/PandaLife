using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : BarSystem
{
    private enum PandaState { None, Happy, Normal, Hungry, Rage }
    private PandaState currentState = PandaState.None;

    [SerializeField] private string pandaID;
    [SerializeField] private PandaBarUIConection barraUI;

    [SerializeField] private RageSystem rageSystem;
    private bool rageactivated = false;

    [SerializeField] private Sprite happyFace;
    [SerializeField] private Sprite normalFace;
    [SerializeField] private Sprite angryFace;

    private Color satisfiedbarcolor = Color.green;
    private Color normalbarcolor = Color.yellow;
    private Color hungrybarcolor = Color.red;

    [SerializeField] private GameObject happyModelFace;
    [SerializeField] private GameObject normalModelFace;
    [SerializeField] private GameObject angryModelFace;

    [SerializeField] private ParticleStateController particlecontroller;

    private bool ispaused = false;

    public bool IsRageActivated => rageactivated;

    void Start()
    {
        if (BarraManager.Instancia != null && pandaID != "")
        {
            var manager = BarraManager.Instancia;

            if (manager.hungerValues.ContainsKey(pandaID))
            {
                currentValue = manager.hungerValues[pandaID];
                rageactivated = manager.rageStates[pandaID];
            }
            else
            {
                currentValue = maxValue;
                rageactivated = false;

                manager.hungerValues[pandaID] = currentValue;
                manager.rageStates[pandaID] = rageactivated;
            }
        }

        if (rageactivated && rageSystem != null)
        {
            currentState = PandaState.Rage;
            ClearHungerFaces();
            Deactivate();

            rageSystem.ActivateRage(bar, fillImage);

            if (barraUI != null)
                barraUI.SetRage(rageSystem);
        }
        else
        {
            Activate();

            if (barraUI != null)
                barraUI.SetHunger(this);
        }

        UpdateUI();
        RefreshVisuals();
    }

    protected override void Update()
    {
        if (isActive)
            UpdateSystem();
    }

    protected override void UpdateValue()
    {
        bool globalPause = BarraManager.Instancia != null && BarraManager.Instancia.hungerPaused;

        if (!ispaused && !globalPause)
        {
            currentValue -= changeRate * Time.deltaTime * GameManager.instance.barmultiplicator;
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        }

        if (rageSystem != null && !rageactivated && currentValue <= 0)
        {
            rageactivated = true;
            currentState = PandaState.Rage;
            Deactivate();

            ClearHungerFaces();

            if (particlecontroller != null)
                particlecontroller.StopParticles();

            rageSystem.ActivateRage(bar, fillImage);

            if (barraUI != null)
                barraUI.SetRage(rageSystem);
        }
    }

    void LateUpdate()
    {
        if (BarraManager.Instancia != null && pandaID != "")
        {
            if (BarraManager.Instancia.isResetting) return;

            BarraManager.Instancia.hungerValues[pandaID] = currentValue;
            BarraManager.Instancia.rageStates[pandaID] = rageactivated;
        }
    }

    protected override void UpdateColors()
    {
        if (rageactivated) return;

        float percentage = (currentValue / maxValue) * 100f;

        if (percentage < 50f)
        {
            if (fillImage != null) fillImage.color = hungrybarcolor;
            if (indicatorImage != null) indicatorImage.sprite = angryFace;
            SetModelFace(angryModelFace);

            if (currentState != PandaState.Hungry)
            {
                currentState = PandaState.Hungry;
                if (particlecontroller != null) particlecontroller.Hungry();
            }
        }
        else if (percentage < 80f)
        {
            if (fillImage != null) fillImage.color = normalbarcolor;
            if (indicatorImage != null) indicatorImage.sprite = normalFace;
            SetModelFace(normalModelFace);

            if (currentState != PandaState.Normal)
            {
                currentState = PandaState.Normal;
                if (particlecontroller != null) particlecontroller.StopParticles();
            }
        }
        else
        {
            if (fillImage != null) fillImage.color = satisfiedbarcolor;
            if (indicatorImage != null) indicatorImage.sprite = happyFace;
            SetModelFace(happyModelFace);

            if (currentState != PandaState.Happy)
            {
                currentState = PandaState.Happy;
                if (particlecontroller != null) particlecontroller.Happy();
            }
        }
    }

    private void SetModelFace(GameObject activeFace)
    {
        if (happyModelFace != null) happyModelFace.SetActive(false);
        if (normalModelFace != null) normalModelFace.SetActive(false);
        if (angryModelFace != null) angryModelFace.SetActive(false);

        if (activeFace != null) activeFace.SetActive(true);
    }

    private void ClearHungerFaces()
    {
        if (happyModelFace != null) happyModelFace.SetActive(false);
        if (normalModelFace != null) normalModelFace.SetActive(false);
        if (angryModelFace != null) angryModelFace.SetActive(false);
    }

    public void Restaurar(float cantidad)
    {
        float amount = (cantidad / 100f) * maxValue;
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        RefreshVisuals();
    }

    public void PauseHunger(float seconds)
    {
        StartCoroutine(PauseRoutine(seconds));
    }

    private System.Collections.IEnumerator PauseRoutine(float seconds)
    {
        ispaused = true;
        yield return new WaitForSeconds(seconds);
        ispaused = false;
    }

    public void ResetSystem()
    {
        if (rageactivated)
        {
            currentValue = 0f;
            if (rageSystem != null) rageSystem.ResetSystem();
        }
        else
        {
            currentValue = maxValue;
            if (rageSystem != null) rageSystem.ResetSystem();
        }

        currentState = PandaState.None;
        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        UpdateColors();

        if (bar != null)
            bar.value = currentValue;
    }
}