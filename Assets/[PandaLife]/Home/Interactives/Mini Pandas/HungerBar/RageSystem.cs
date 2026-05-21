using UnityEngine;
using UnityEngine.UI;

public class RageSystem : BarSystem
{
    private enum RageState { None, Calm, Angry }
    private RageState currentState = RageState.None;

    [SerializeField] private string pandaID;

    private Color calmColor = new Color(1f, 0f, 0f);
    private Color rageColor = new Color(0.6f, 0f, 0f);

    [SerializeField] private Sprite calmFace;
    [SerializeField] private Sprite rageFace;

    [SerializeField] private ParticleStateController particlecontroller;

    // =========================
    // START (FIX CLAVE)
    // =========================

    void Start()
    {
        if (BarraManager.Instancia != null && pandaID != "")
        {
            var manager = BarraManager.Instancia;

            if (manager.rageValues.ContainsKey(pandaID))
            {
                currentValue = manager.rageValues[pandaID];
                maxValue = manager.rageMaxValue;
                changeRate = manager.rageChangeRate;
            }
        }

        currentState = RageState.None;

        if (isActive)
        {
            UpdateUI();
            UpdateColors();
        }
    }

    // =========================
    // ACTIVATE
    // =========================

    public void ActivateRage(Slider sharedBar, Image sharedFill)
    {
      
        bar = sharedBar;
        fillImage = sharedFill;

        GameManager.instance.miniPandasHambrientos--;

        if (BarraManager.Instancia != null && pandaID != "")
        {
            var manager = BarraManager.Instancia;

            if (manager.rageValues.ContainsKey(pandaID))
                currentValue = manager.rageValues[pandaID];

            maxValue = manager.rageMaxValue;
            changeRate = manager.rageChangeRate;
        }

        currentState = RageState.None;

        Activate();

        UpdateUI();
        UpdateColors();
    }

    // =========================
    // UPDATE VALUE
    // =========================

    protected override void UpdateValue()
    {
        currentValue +=
            changeRate *
            Time.deltaTime *
            GameManager.instance.barmultiplicator;

        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
    }

    // =========================
    // UPDATE
    // =========================

    protected override void Update()
    {
        if (isActive)
            UpdateSystem();
    }

    // =========================
    // VISUAL LOGIC
    // =========================

    protected override void UpdateColors()
    {
        float percentage = (currentValue / maxValue) * 100f;

        // CALM
        if (percentage < 50f)
        {
            if (fillImage != null)
                fillImage.color = calmColor;

            if (indicatorImage != null)
                indicatorImage.sprite = calmFace;

            if (currentState != RageState.Calm)
            {
                currentState = RageState.Calm;

                if (particlecontroller != null)
                    particlecontroller.Sad();
            }
        }
        // ANGRY
        else
        {
            if (fillImage != null)
                fillImage.color = rageColor;

            if (indicatorImage != null)
                indicatorImage.sprite = rageFace;

            if (currentState != RageState.Angry)
            {
                currentState = RageState.Angry;

                if (particlecontroller != null)
                    particlecontroller.Angry();
            }
        }
    }

    // =========================
    // SAVE
    // =========================

    void LateUpdate()
    {
        if (BarraManager.Instancia != null && pandaID != "")
        {
            BarraManager.Instancia.rageValues[pandaID] = currentValue;
        }
    }

    // =========================
    // ACTIONS
    // =========================

    public void ReducirIraPorcentaje(float porcentaje)
    {
        float reduction = (porcentaje / 100f) * maxValue;

        currentValue -= reduction;

        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        UpdateUI();
        UpdateColors();
    }

    public void ResetSystem()
    {
        currentValue = 0f;
        currentState = RageState.None;

        UpdateColors();

        if (bar != null)
            bar.value = 0f;

        if (particlecontroller != null)
            particlecontroller.ResetVisuals(); // 🔥 IMPORTANTE
    }
}