using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurMaterialActivation : MonoBehaviour
{
    [Header("Asigna tus materiales aquí")]
    public Material[] materialesAOscurecer;

    [Header("Configuración del Tiempo")]
    public float duracionTransicion = 3.0f;

    private List<Material> copiasMateriales = new List<Material>();
    private List<Color> coloresOriginales = new List<Color>();
    private string propiedadColor;

    // OnEnable se ejecuta AUTOMÁTICAMENTE en el instante exacto 
    // en que la Activation Track de Timeline enciende este objeto.
    void OnEnable()
    {
        if (materialesAOscurecer == null || materialesAOscurecer.Length == 0) return;

        // Limpiar listas por si acaso el Timeline se repite
        copiasMateriales.Clear();
        coloresOriginales.Clear();

        // Detectarel tipo de Shader
        ConfigurarPropiedadColor(materialesAOscurecer[0]);

        // Clonamos los materiales y los aplicamos a la escena de inmediato
        for (int i = 0; i < materialesAOscurecer.Length; i++)
        {
            if (materialesAOscurecer[i] != null)
            {
                Material copia = new Material(materialesAOscurecer[i]);
                copiasMateriales.Add(copia);
                coloresOriginales.Add(copia.GetColor(propiedadColor));

                ReemplazarMaterialEnEscena(materialesAOscurecer[i], copia);
            }
        }

        // 3. Iniciar el oscurecimiento
        if (copiasMateriales.Count > 0)
        {
            StartCoroutine(RutinaOscurecerTodo());
        }
    }

    private void ConfigurarPropiedadColor(Material mat)
    {
        if (mat.HasProperty("_BaseColor")) propiedadColor = "_BaseColor"; // URP
        else if (mat.HasProperty("_Color")) propiedadColor = "_Color"; // Estándar
    }

    private void ReemplazarMaterialEnEscena(Material original, Material copia)
    {
        Renderer[] todosLosRenderers = Object.FindFirstObjectByType<Renderer>() != null ?
            Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None) : new Renderer[0];

        foreach (Renderer rend in todosLosRenderers)
        {
            Material[] mats = rend.sharedMaterials;
            bool huboCambio = false;

            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i] == original)
                {
                    mats[i] = copia;
                    huboCambio = true;
                }
            }

            if (huboCambio) rend.sharedMaterials = mats;
        }
    }

    private IEnumerator RutinaOscurecerTodo()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionTransicion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float porcentaje = tiempoTranscurrido / duracionTransicion;

            for (int i = 0; i < copiasMateriales.Count; i++)
            {
                Color colorActual = Color.Lerp(coloresOriginales[i], Color.black, porcentaje);
                copiasMateriales[i].SetColor(propiedadColor, colorActual);
            }

            yield return null;
        }

        for (int i = 0; i < copiasMateriales.Count; i++)
        {
            copiasMateriales[i].SetColor(propiedadColor, Color.black);
        }
    }
}