using UnityEngine;
using UnityEngine.UI;

public class RageSystem : BarSystem
{
    [SerializeField] private string pandaID;

    private Color calmColor = new Color(1f, 0f, 0f);
    private Color rageColor = new Color(0.6f, 0f, 0f);

    [SerializeField] private Sprite calmFace;
    [SerializeField] private Sprite rageFace;

    [Header("Rage Model Faces")]
    [SerializeField] private GameObject calmModelFace;
    [SerializeField] private GameObject rageModelFace;

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

        UpdateUI();
    }

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

        Activate();
    }

    protected override void UpdateValue()
    {
        currentValue += changeRate * Time.deltaTime * GameManager.instance.barmultiplicator;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
    }

    protected override void UpdateColors()
    {
        float percentage = (currentValue / maxValue) * 100f;

        if (percentage < 50f)
        {
            if (fillImage != null)
                fillImage.color = calmColor;

            if (indicatorImage != null)
                indicatorImage.sprite = calmFace;

            SetModelFace(calmModelFace);
        }
        else
        {
            if (fillImage != null)
                fillImage.color = rageColor;

            if (indicatorImage != null)
                indicatorImage.sprite = rageFace;

            SetModelFace(rageModelFace);
        }
    }

    private void SetModelFace(GameObject activeFace)
    {
        // 🔥 SIEMPRE apagar todo primero
        if (calmModelFace != null)
            calmModelFace.SetActive(false);

        if (rageModelFace != null)
            rageModelFace.SetActive(false);

        // activar solo la correcta
        if (activeFace != null)
            activeFace.SetActive(true);
    }

    protected override void Update()
    {
        if (isActive)
            UpdateSystem();
    }

    void LateUpdate()
    {
        if (BarraManager.Instancia != null && pandaID != "")
            BarraManager.Instancia.rageValues[pandaID] = currentValue;
    }

    public void ReducirIraPorcentaje(float porcentaje)
    {
        float reduction = (porcentaje / 100f) * maxValue;
        currentValue -= reduction;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        UpdateUI();
    }

    public void ResetSystem()
    {
        currentValue = 0f;

        UpdateColors();

        if (bar != null)
            bar.value = 0f;
    }
}