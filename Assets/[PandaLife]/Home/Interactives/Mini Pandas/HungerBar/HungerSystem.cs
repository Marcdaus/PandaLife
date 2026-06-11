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

    [SerializeField] private Sprite happyBarSprite;
    [SerializeField] private Sprite normalBarSprite;
    [SerializeField] private Sprite hungryBarSprite;
    [SerializeField] private Image fondoBarraImage;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite normalbackground;
    [SerializeField] private Sprite brokenbackground;

    [SerializeField] private ParticleStateController particlecontroller;

    private bool ispaused = false;

    public float limiteVisualSlider = 1f;

    public bool IsRageActivated => rageactivated;

    void Start()
    {
        currentState = PandaState.None;

        if (BarraManager.Instancia != null && pandaID != "")
        {
            var manager = BarraManager.Instancia;

            if (manager.hungerValues.ContainsKey(pandaID))
            {
                currentValue = manager.hungerValues[pandaID];
                rageactivated = manager.rageStates[pandaID];
               
                if (manager.backgroundStates.ContainsKey(pandaID))
                {
                    bool isBroken = manager.backgroundStates[pandaID];
                    backgroundImage.sprite = isBroken ? brokenbackground : normalbackground;
                }
            }
            else
            {
                currentValue = maxValue;
                rageactivated = false;

                manager.hungerValues[pandaID] = currentValue;
                manager.rageStates[pandaID] = rageactivated;

                manager.backgroundStates[pandaID] = false; 
                backgroundImage.sprite = normalbackground;
            }
        }

        if (rageactivated && rageSystem != null)
        {
            currentState = PandaState.Rage;

            particlecontroller?.ClearAll();

            Deactivate();

            rageSystem.ActivateRage(fullBarImage);

            if (barraUI != null)
                barraUI.SetRage(rageSystem);
        }
        else
        {
            Activate();

            if (barraUI != null)
                barraUI.SetHunger(this);

            RefreshVisuals();
        }

        UpdateUI();
    }

    protected override void Update()
    {
        if (isActive)
            UpdateSystem();
    }

    protected override void UpdateValue()
    {
        bool globalPause =
            BarraManager.Instancia != null &&
            BarraManager.Instancia.hungerPaused;

        if (!ispaused && !globalPause)
        {
            currentValue -=
                changeRate *
                Time.deltaTime *
                GameManager.instance.barmultiplicator;

            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        }

        // ENTRAR EN RAGE
        if (rageSystem != null && !rageactivated && currentValue <= 0)
        {
            rageactivated = true;
            backgroundImage.sprite = brokenbackground;

            currentState = PandaState.Rage;

            Deactivate();

            if (particlecontroller != null)
                particlecontroller.ClearAll();

            rageSystem.ActivateRage(fullBarImage);

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

            BarraManager.Instancia.backgroundStates[pandaID] = (backgroundImage.sprite == brokenbackground);
        }
    }

    protected override void UpdateColors()
    {
        // RAGE CONTROLA TODO
        if (rageactivated) return;

        float percentage = (currentValue / maxValue) * 100f;

        // HAMBRIENTO
        if (percentage < 50f)
        {
            if (fullBarImage != null)
                fullBarImage.sprite = hungryBarSprite;

            if (indicatorImage != null)
                indicatorImage.sprite = angryFace;

            if (currentState != PandaState.Hungry)
            {
                currentState = PandaState.Hungry;

                if (particlecontroller != null)
                    particlecontroller.Hungry();
            }
        }
        // NORMAL
        else if (percentage < 80f)
        {
            if (fullBarImage!= null)
                fullBarImage.sprite = normalBarSprite;

            if (indicatorImage != null)
                indicatorImage.sprite = normalFace;

            if (currentState != PandaState.Normal)
            {
                currentState = PandaState.Normal;

                if (particlecontroller != null)
                    particlecontroller.Normal();
            }
        }
        // FELIZ
        else
        {
            if (fullBarImage != null)
                fullBarImage.sprite = happyBarSprite;

            if (indicatorImage != null)
                indicatorImage.sprite = happyFace;

            if (currentState != PandaState.Happy)
            {
                currentState = PandaState.Happy;

                if (particlecontroller != null)
                    particlecontroller.Happy();
            }
        }
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

            if (rageSystem != null)
                rageSystem.ResetSystem();
        }
        else
        {
            currentValue = maxValue;
            backgroundImage.sprite = normalbackground;

            if (BarraManager.Instancia != null && pandaID != "")
            {
                BarraManager.Instancia.backgroundStates[pandaID] = false;
            }

            if (rageSystem != null)
                rageSystem.ResetSystem();
        }

        currentState = PandaState.None;

        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        UpdateUI();

    }
    public override void UpdateUI()
    {
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }

        if (fullBarImage != null)
        {
            fullBarImage.fillAmount = (currentValue / maxValue) * limiteVisualSlider;
        }

        
        if (fondoBarraImage != null)
        {
            fondoBarraImage.fillAmount = limiteVisualSlider;
        }

        if (valueText != null)
        {
            valueText.text = Mathf.RoundToInt(currentValue).ToString();
        }

        UpdateColors();
    }
}