using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private GameString scenename;
    [SerializeField] private bool isInto = false;
    [Header("Configuración de Rotación")]
    [SerializeField] private float anguloInicial = -10f;
    [SerializeField] private float anguloFinal = 200f;



    [Header("Configuración de Tiempo")]
    [SerializeField] private int horaInicio = 8;
    [SerializeField] private int horaFin = 21;
    [SerializeField] private float duracionEnMinutos = 5f;
    [SerializeField] private TextMeshProUGUI textoReloj;
    [SerializeField] private TextMeshProUGUI textoDia;

    [Header("Efecto de Oscurecimiento y texto")]
    [SerializeField] private Image fadeImage;
     private float horaEmpiezaOscurecer = 20.0f;
    private float horaEfecto = 20.8333f;

    [Header("Recompensas")]
    [SerializeField] private List<RewardBagElement> elementslist = new List<RewardBagElement>();
 
    

    private RectTransform rectTextoDia;

    private float duracionEnSegundos;

    void Start()
    {
        duracionEnSegundos = duracionEnMinutos * 60f;
        rectTextoDia = textoDia.GetComponent<RectTransform>();
        RewardBagManager.EvaluateAllBags(elementslist);
    }

    void Update()
    {

        
        if (GameManager.instance.tiempoTranscurrido < duracionEnSegundos)
        {
            GameManager.instance.tiempoTranscurrido += Time.deltaTime * GameManager.instance.multiplicadorVelocidad ;
            float porcentaje = GameManager.instance.tiempoTranscurrido / duracionEnSegundos;

            // 1. Control de Rotación
            float anguloActual = Mathf.Lerp(anguloInicial, anguloFinal, porcentaje);
            if (!isInto) transform.localEulerAngles = new Vector3(anguloActual, 0, 0);  

            // 2. Control de Reloj
            ActualizarReloj(porcentaje);

            // 3. Control de obscurezers
            ActualizarFundido();

            // 4. Control del texto dia
            ActualizarTexto();
        }
        else
        {
            GameManager.instance.tiempoTranscurrido = 0f;
            GameManager.instance.numeroDia++;
            Rewards("Bags");
            Rewards("Collectables");
        }


        if (GameManager.instance.DiaActual != GameManager.instance.numeroDia)
        {
            SceneManager.LoadScene(scenename.Value);
            GameManager.instance.DiaActual++;
            RewardBagManager.EvaluateAllBags(elementslist);
        }
    }
    void Rewards(string type)
    {
        if (type == "Bags")
        {
            if (GameManager.instance.numeroDia == 2)
            {
                GameManager.instance.MostrarMensajeTemporal($"!Dia {GameManager.instance.numeroDia}¡ Saco de semillas Red Dragon desbloqueado");
            }
            else if (GameManager.instance.numeroDia == 3)
            {
                GameManager.instance.MostrarMensajeTemporal($"!Dia {GameManager.instance.numeroDia}¡ Saco de semillas Uchuva desbloqueado");
            }
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
    void ActualizarFundido()
    {
        if (fadeImage == null) return;

        // Convertimos la hora actual de minutos totales a formato 24h decimal
        float horaActualDecimal = GameManager.instance.minutosActualesTotales / 60f;
        
         
        if (horaActualDecimal > horaEmpiezaOscurecer)
        {
            // Calculamos el progreso entre la hora de inicio (20:00) y el final (21:00)
            // Esto nos da un valor entre 0 y 1
            float progresoOscurecimiento = (horaActualDecimal - horaEmpiezaOscurecer) / (horaFin - horaEmpiezaOscurecer);

            // Aplicamos ese progreso al Alpha de la imagen
            Color c = fadeImage.color;
            c.a = Mathf.Clamp01(progresoOscurecimiento);
            fadeImage.color = c;
        }
        else
        {
            
            // Si aún no son las 20:00, aseguramos que sea invisible
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
 

    }
    void ActualizarTexto()
    {
        float horaActualDecimal = GameManager.instance.minutosActualesTotales / 60f;
        if (horaActualDecimal > horaEfecto)
        {
            textoDia.text = "Día " + (GameManager.instance.numeroDia + 1);

            // Centrar instantáneamente
            rectTextoDia.anchorMin = new Vector2(0.5f, 0.5f);
            rectTextoDia.anchorMax = new Vector2(0.5f, 0.5f);
            rectTextoDia.pivot = new Vector2(0.5f, 0.5f);
            rectTextoDia.anchoredPosition = Vector2.zero;

            // Tamaño grande y alineación
            textoDia.fontSize = 80f; // Ajusta este número a tu gusto
            textoDia.alignment = TextAlignmentOptions.Center;
        }
        else
        {
            textoDia.text = "Día " + GameManager.instance.numeroDia;
        }
    }
}