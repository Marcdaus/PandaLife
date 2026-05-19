using UnityEngine;

public class ParticleStateController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlesystemref;

    [SerializeField] private Material happymaterial;
    [SerializeField] private Material hungrymaterial;
    [SerializeField] private Material angrymaterial;
    [SerializeField] private Material sadmaterial;

    [SerializeField] private float happysize = 0.5f;
    [SerializeField] private float hungrysize = 0.3f;
    [SerializeField] private float angrysize = 0.8f;
    [SerializeField] private float sadsize = 0.4f;

    private ParticleSystemRenderer particlerenderer;

    private void Awake()
    {
        if (particlesystemref == null)
        {
            particlesystemref = GetComponent<ParticleSystem>();
        }

        if (particlesystemref != null)
        {
            particlerenderer = particlesystemref.GetComponent<ParticleSystemRenderer>();
        }
    }

    public void Happy()
    {
        ApplyState(happymaterial, happysize);
    }

    public void Hungry()
    {
        ApplyState(hungrymaterial, hungrysize);
    }

    public void Angry()
    {
        ApplyState(angrymaterial, angrysize);
    }

    public void Sad()
    {
        ApplyState(sadmaterial, sadsize);
    }

    private void ApplyState(Material material, float size)
    {
        if (particlesystemref == null || particlerenderer == null)
        {
            Debug.LogWarning("No hay ParticleSystem asignado.");
            return;
        }

        // Detener de forma limpia antes de cambiar propiedades visuales
        particlesystemref.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Cambiar material
        particlerenderer.material = material;

        // Cambiar tamaño de partículas
        var main = particlesystemref.main;
        main.startSize = size;

        // Volver a reproducir el nuevo estado
        particlesystemref.Play();
    }

    public void PauseParticles()
    {
        if (particlesystemref == null) return;
        particlesystemref.Pause();
    }

    public void ResumeParticles()
    {
        if (particlesystemref == null) return;
        particlesystemref.Play();
    }

    public void StopParticles()
    {
        if (particlesystemref == null) return;
        particlesystemref.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}