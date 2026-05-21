using UnityEngine;

public class ParticleStateController : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem particlesystemref;

    [SerializeField] private Material happymaterial;
    [SerializeField] private Material hungrymaterial;
    [SerializeField] private Material angrymaterial;
    [SerializeField] private Material sadmaterial;

    [SerializeField] private float happysize = 0.5f;
    [SerializeField] private float hungrysize = 0.3f;
    [SerializeField] private float angrysize = 0.8f;
    [SerializeField] private float sadsize = 0.4f;

    [Header("Faces")]
    [SerializeField] private GameObject happymodelface;
    [SerializeField] private GameObject normalmodelface;
    [SerializeField] private GameObject hungrymodelface;
    [SerializeField] private GameObject calmmodelface;
    [SerializeField] private GameObject ragemodelface;

    private ParticleSystemRenderer particlerenderer;

    private void Awake()
    {
        if (particlesystemref == null)
            particlesystemref = GetComponent<ParticleSystem>();

        if (particlesystemref != null)
            particlerenderer = particlesystemref.GetComponent<ParticleSystemRenderer>();

        ResetVisuals(); // 🔥 CLAVE REAL
    }

    // =========================
    // STATES
    // =========================

    public void Happy()
    {
        DisableAllFaces();
        if (happymodelface) happymodelface.SetActive(true);
        Apply(happymaterial, happysize);
    }

    public void Normal()
    {
        DisableAllFaces();
        if (normalmodelface) normalmodelface.SetActive(true);
        StopParticles();
    }

    public void Hungry()
    {
        DisableAllFaces();
        if (hungrymodelface) hungrymodelface.SetActive(true);
        Apply(hungrymaterial, hungrysize);
    }

    public void Sad()
    {
        DisableAllFaces();
        if (calmmodelface) calmmodelface.SetActive(true);
        Apply(sadmaterial, sadsize);
    }

    public void Angry()
    {
        DisableAllFaces();
        if (ragemodelface) ragemodelface.SetActive(true);
        Apply(angrymaterial, angrysize);
    }

    // =========================
    // CORE
    // =========================

    private void Apply(Material mat, float size)
    {
        if (!particlesystemref || !particlerenderer) return;

        particlesystemref.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        particlerenderer.material = mat;

        var main = particlesystemref.main;
        main.startSize = size;

        particlesystemref.Play();
    }

    private void DisableAllFaces()
    {
        if (happymodelface) happymodelface.SetActive(false);
        if (normalmodelface) normalmodelface.SetActive(false);
        if (hungrymodelface) hungrymodelface.SetActive(false);
        if (calmmodelface) calmmodelface.SetActive(false);
        if (ragemodelface) ragemodelface.SetActive(false);
    }

    public void StopParticles()
    {
        if (particlesystemref == null) return;

        particlesystemref.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void ClearAll()
    {
        DisableAllFaces();
        StopParticles();
        particlesystemref.Clear(true);
    }

    public void ResetVisuals()
    {
        if (!particlesystemref) return;

        particlesystemref.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particlesystemref.Clear(true);

        var main = particlesystemref.main;
        main.startSize = 0.3f;

        if (particlerenderer)
        {
            particlerenderer.material = null;
            particlerenderer.trailMaterial = null;
        }

        DisableAllFaces();
    }
}