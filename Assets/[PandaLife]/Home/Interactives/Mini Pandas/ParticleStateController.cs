using UnityEngine;

public class ParticleStateController : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem particleSystemRef;

    [Header("Audio")]
    [SerializeField] private PlaySFX playSfx;

    [SerializeField] private Material happyMaterial;
    [SerializeField] private Material hungryMaterial;
    [SerializeField] private Material angryMaterial;
    [SerializeField] private Material sadMaterial;

    [SerializeField] private float happySize = 0.5f;
    [SerializeField] private float hungrySize = 0.3f;
    [SerializeField] private float angrySize = 0.8f;
    [SerializeField] private float sadSize = 0.4f;

    [Header("Faces")]
    [SerializeField] private GameObject happyModelFace;
    [SerializeField] private GameObject normalModelFace;
    [SerializeField] private GameObject hungryModelFace;
    [SerializeField] private GameObject calmModelFace;
    [SerializeField] private GameObject rageModelFace;

    private ParticleSystemRenderer particleRenderer;

    private void Awake()
    {
        if (particleSystemRef == null)
            particleSystemRef = GetComponent<ParticleSystem>();

        if (particleSystemRef != null)
            particleRenderer = particleSystemRef.GetComponent<ParticleSystemRenderer>();

        ResetVisuals();
    }

    // =========================
    // STATES
    // =========================

    public void Happy()
    {
        StopAllAudio();

        DisableAllFaces();
        if (happyModelFace) happyModelFace.SetActive(true);

        Apply(happyMaterial, happySize);

        if (playSfx != null)
            playSfx.PlayHappy();
    }

    public void Normal()
    {
        StopAllAudio();

        DisableAllFaces();
        if (normalModelFace) normalModelFace.SetActive(true);

        StopParticles();
    }

    public void Hungry()
    {
        StopAllAudio();

        DisableAllFaces();
        if (hungryModelFace) hungryModelFace.SetActive(true);

        Apply(hungryMaterial, hungrySize);

        if (playSfx != null)
            playSfx.PlayHungry();
    }

    public void Sad()
    {
        StopAllAudio();

        DisableAllFaces();
        if (calmModelFace) calmModelFace.SetActive(true);

        Apply(sadMaterial, sadSize);
    }

    public void Angry()
    {
        StopAllAudio();

        DisableAllFaces();
        if (rageModelFace) rageModelFace.SetActive(true);

        Apply(angryMaterial, angrySize);

        if (playSfx != null)
            playSfx.PlayAngry();
    }

    // =========================
    // AUDIO
    // =========================

    private void StopAllAudio()
    {
        if (playSfx != null)
            playSfx.StopAll();
    }

    // =========================
    // VISUAL CORE
    // =========================

    private void Apply(Material material, float size)
    {
        if (!particleSystemRef || particleRenderer == null) return;

        particleSystemRef.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        particleRenderer.material = material;

        var main = particleSystemRef.main;
        main.startSize = size;

        particleSystemRef.Play();
    }

    private void DisableAllFaces()
    {
        if (happyModelFace) happyModelFace.SetActive(false);
        if (normalModelFace) normalModelFace.SetActive(false);
        if (hungryModelFace) hungryModelFace.SetActive(false);
        if (calmModelFace) calmModelFace.SetActive(false);
        if (rageModelFace) rageModelFace.SetActive(false);
    }

    public void StopParticles()
    {
        if (particleSystemRef == null) return;

        particleSystemRef.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void ClearAll()
    {
        DisableAllFaces();
        StopParticles();

        if (playSfx != null)
            playSfx.StopAll();
    }

    public void ResetVisuals()
    {
        if (!particleSystemRef) return;

        particleSystemRef.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleSystemRef.Clear(true);

        var main = particleSystemRef.main;
        main.startSize = 0.3f;

        if (particleRenderer)
        {
            particleRenderer.material = null;
            particleRenderer.trailMaterial = null;
        }

        DisableAllFaces();
    }
}