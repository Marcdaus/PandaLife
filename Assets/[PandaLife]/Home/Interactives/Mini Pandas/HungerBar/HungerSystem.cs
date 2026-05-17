using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : BarSystem
{
    [SerializeField] private string pandaID;
    [SerializeField] private PandaBarUIConection barraUI;

    [SerializeField] private RageSystem rageSystem;
    private bool rageActivated = false;

    [SerializeField] private Sprite happyFace;
    [SerializeField] private Sprite normalFace;
    [SerializeField] private Sprite angryFace;

    private Color satisfiedBarColor = Color.green;
    private Color normalBarColor = Color.yellow;
    private Color hungryBarColor = Color.red;

    [Header("Model Faces")]
    [SerializeField] private GameObject happyModelFace;
    [SerializeField] private GameObject normalModelFace;
    [SerializeField] private GameObject angryModelFace;

    private bool isPaused = false;

    public bool IsRageActivated => rageActivated;

    void Start()
    {
        if (BarraManager.Instancia != null && pandaID != "")
        {
            var manager = BarraManager.Instancia;

            if (manager.hungerValues.ContainsKey(pandaID))
            {
                currentValue = manager.hungerValues[pandaID];
                rageActivated = manager.rageStates[pandaID];
            }
            else
            {
                currentValue = maxValue;
                rageActivated = false;

                manager.hungerValues[pandaID] = currentValue;
                manager.rageStates[pandaID] = rageActivated;
            }
        }

        if (rageActivated && rageSystem != null)
        {
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
    }

    protected override void Update()
    {
        if (isActive)
            UpdateSystem();
    }

    protected override void UpdateValue()
    {
        bool globalPause = BarraManager.Instancia != null && BarraManager.Instancia.hungerPaused;

        if (!isPaused && !globalPause)
        {
            currentValue -= changeRate * Time.deltaTime * GameManager.instance.barmultiplicator;
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        }

        if (rageSystem != null && !rageActivated && currentValue <= 0)
        {
            rageActivated = true;
            Deactivate();

            // 🔥 APAGAR ÚLTIMA CARA ANTES DEL CAMBIO
            ClearHungerFaces();

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
            BarraManager.Instancia.rageStates[pandaID] = rageActivated;
        }
    }

    protected override void UpdateColors()
    {
        if (rageActivated) return;

        float percentage = (currentValue / maxValue) * 100f;

        if (percentage < 50f)
        {
            if (fillImage != null)
                fillImage.color = hungryBarColor;

            if (indicatorImage != null)
                indicatorImage.sprite = angryFace;

            SetModelFace(angryModelFace);
        }
        else if (percentage < 80f)
        {
            if (fillImage != null)
                fillImage.color = normalBarColor;

            if (indicatorImage != null)
                indicatorImage.sprite = normalFace;

            SetModelFace(normalModelFace);
        }
        else
        {
            if (fillImage != null)
                fillImage.color = satisfiedBarColor;

            if (indicatorImage != null)
                indicatorImage.sprite = happyFace;

            SetModelFace(happyModelFace);
        }
    }

    private void SetModelFace(GameObject activeFace)
    {
        if (happyModelFace != null)
            happyModelFace.SetActive(false);

        if (normalModelFace != null)
            normalModelFace.SetActive(false);

        if (angryModelFace != null)
            angryModelFace.SetActive(false);

        if (activeFace != null)
            activeFace.SetActive(true);
    }

    private void ClearHungerFaces()
    {
        if (happyModelFace != null)
            happyModelFace.SetActive(false);

        if (normalModelFace != null)
            normalModelFace.SetActive(false);

        if (angryModelFace != null)
            angryModelFace.SetActive(false);
    }

    public void Restaurar(float cantidad)
    {
        float amount = (cantidad / 100f) * maxValue;
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        UpdateUI();
    }

    public void PauseHunger(float seconds)
    {
        StartCoroutine(PauseRoutine(seconds));
    }

    private System.Collections.IEnumerator PauseRoutine(float seconds)
    {
        isPaused = true;
        yield return new WaitForSeconds(seconds);
        isPaused = false;
    }

    public void ResetSystem()
    {
        if (rageActivated)
        {
            currentValue = 0f;

            if (rageSystem != null)
                rageSystem.ResetSystem();
        }
        else
        {
            currentValue = maxValue;

            if (rageSystem != null)
                rageSystem.ResetSystem();
        }

        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        UpdateColors();

        if (bar != null)
            bar.value = currentValue;
    }
}