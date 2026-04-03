using UnityEngine;
using TMPro; // Necesario si usas TextMeshPro

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] bool isInto = false;
    [Header("Configuración de Rotación")]
    [SerializeField] public float anguloInicial = -10f;
    [SerializeField] public float anguloFinal = 200f;
    [SerializeField] public float duracionEnMinutos = 5f;

    [Header("Configuración de Tiempo")]
    [SerializeField] public int horaInicio = 8;
    [SerializeField] public int horaFin = 21;
    [SerializeField] public TextMeshProUGUI textoReloj; // Arrastra tu objeto de texto aquí

    
    private float duracionEnSegundos;

    void Start()
    {
        duracionEnSegundos = duracionEnMinutos * 60f;
    }

    void Update()
    {
        
        if (GameManager.instance.tiempoTranscurrido < duracionEnSegundos)
        {
            GameManager.instance.tiempoTranscurrido += Time.deltaTime;
            float porcentaje = GameManager.instance.tiempoTranscurrido / duracionEnSegundos;

            // 1. Control de Rotación
            float anguloActual = Mathf.Lerp(anguloInicial, anguloFinal, porcentaje);
            if (!isInto) transform.localEulerAngles = new Vector3(anguloActual, 0, 0);  

            // 2. Control de Reloj
            ActualizarReloj(porcentaje);
        }
        else
        {
            GameManager.instance.tiempoTranscurrido = 0f;
        }
    }

    void ActualizarReloj(float pct)
    {
        if (textoReloj == null) return;

        // Calculamos el total de minutos entre las 8:00 y las 21:00
        float minutosInicio = horaInicio * 60;
        float minutosFin = horaFin * 60;

        // Calculamos cuántos minutos han pasado según el porcentaje del ciclo
        GameManager.instance.minutosActualesTotales = Mathf.Lerp(minutosInicio, minutosFin, pct);

        // Convertimos esos minutos totales a formato HH:mm
        int horas = Mathf.FloorToInt(GameManager.instance.minutosActualesTotales / 60);
        int minutos = Mathf.FloorToInt(GameManager.instance.minutosActualesTotales % 60);

        // Actualizamos el texto con formato de dos dígitos (00:00)
        textoReloj.text = string.Format("{0:00}:{1:00}", horas, minutos);
    }
}