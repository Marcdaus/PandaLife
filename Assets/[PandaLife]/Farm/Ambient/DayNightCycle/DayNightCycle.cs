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
    [Header("Configuración Escenas")]
    [SerializeField] private GameString homeScene;
    [SerializeField] private GameString theEnd;
    [SerializeField] private bool isInto = false;

    [Header("Configuración de Rotación")]
    [SerializeField] private float startangle = -10f;
    [SerializeField] private float endangle = 200f;



    [Header("Configuración de Tiempo")]
    [SerializeField] private int starthour = 8;
    [SerializeField] private int endhour = 21;
    [SerializeField] private float durationinminuts = 5f;
    [SerializeField] private TextMeshProUGUI clocktext;
    [SerializeField] private TextMeshProUGUI daytext;

    [Header("Efecto de Oscurecimiento y texto")]
    [SerializeField] private Image fadeImage;
    private static float darkeningstarttime = 20.0f;
    private float efecthour = 20.8333f;

    [Header("Recompensas")]
    [SerializeField] private  List<RewardElement> elementlistbag = new List<RewardElement>();
    [SerializeField] private RewardElement tedy ;
    [SerializeField] private RewardElement note;

    [Header("Menú Caldero")]
    [SerializeField] private MenuCauldron menucauldron;

    [Header("barras mini pandas")]
    [SerializeField] private TextMeshProUGUI percentage1;
    [SerializeField] private TextMeshProUGUI percentage2;
    [SerializeField] private TextMeshProUGUI percentage3;

    [Header("soltar cosas entre cambio de escena")]
    private Player player;
    private PickupDrop pickupobject;


    private RectTransform rectTextoDia;

    private float duracionEnSegundos;

    void Start()
    {
        duracionEnSegundos = durationinminuts * 60f;
        rectTextoDia = daytext.GetComponent<RectTransform>();

            RewardManager.EvaluatelElement(note);  
            RewardManager.EvaluatelElement(tedy);
     
        RewardManager.EvaluateAllElements(elementlistbag);

        player = FindFirstObjectByType<Player>();
        pickupobject = FindFirstObjectByType<PickupDrop>();
    }

    void Update()
    {


        if (GameManager.instance.tiempoTranscurrido < duracionEnSegundos)
        {
            if (!GameManager.instance.stopTime)GameManager.instance.tiempoTranscurrido += Time.deltaTime * GameManager.instance.multiplicadorvelocidaddia;
                float porcentaje = GameManager.instance.tiempoTranscurrido / duracionEnSegundos;

            // 1. Control de Rotación
            float anguloActual = Mathf.Lerp(startangle, endangle, porcentaje);
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
            GameManager.instance.numday++;

            // Buscamos el PandaRequest que va a sobrevivir al cambio de escena
            PandaRequest persistenteReq = GameManager.instance.GetComponent<PandaRequest>();

            if (persistenteReq != null)
            {
                persistenteReq.UnlockCropsForDay(GameManager.instance.numday);
                persistenteReq.GenerateRandomRequests();
                Debug.Log("Nuevos pedidos generados para el día: " + GameManager.instance.numday);
            }
            else
            {
                Debug.LogError("El GameManager no tiene el script PandaRequest");
            }
            GameManager.instance.valuepercentage += 5;
            GameManager.instance.barmultiplicator += 0.05f;


            ResetBars();
            Rewards("Bags");
            Rewards("Collectables");
            SceneManager.LoadScene(homeScene.Value);
        }
        if(GameManager.instance.numday > 1)
        {
            percentage1.text = "+" + GameManager.instance.valuepercentage.ToString() + "%";
            percentage2.text = "+" + GameManager.instance.valuepercentage.ToString() + "%";
            percentage3.text = "+" + GameManager.instance.valuepercentage.ToString() + "%";
        }
        if (GameManager.instance.numday == 4)
        {
            GameManager.instance.Resetplay();
            SceneManager.LoadScene(theEnd.Value);
        }

    }
    public void Rewards(string type)
    {
        if (type == "Bags")
        {
            
            if (GameManager.instance.numday == 2)
            {
                GameManager.instance.MostrarMensajeTemporal($"¡Dia {GameManager.instance.numday}! Saco de semillas Red Dragon desbloqueado \n ¡Nueva receta desbloqueada!", 5f, type);
            }
            else if (GameManager.instance.numday == 3)
            {
                GameManager.instance.MostrarMensajeTemporal($"¡Dia {GameManager.instance.numday}! Saco de semillas Uchuva desbloqueado \n ¡Nuevas recetas desbloqueadas! x2", 5f, type);
            }
        }
        else if (type == "Collectables")
        {
            
            if (GameManager.instance.numday == 2 && GameManager.instance.miniPandasHambrientos == 3)
            {
                GameManager.instance.MostrarMensajeTemporal($"¡Mini pandas 3/3! Coleccionable carta desbloqueado", 5f, type);
            }
            else if (GameManager.instance.numday == 3 && GameManager.instance.miniPandasHambrientos == 3)
            {
                GameManager.instance.MostrarMensajeTemporal($"¡Mini pandas 3/3! Coleccionable muñeco desbloqueado", 5f, type); 
            }
        }
    }


        void ActualizarReloj(float pct)
        {
            if (clocktext == null) return;

            // Calculamos el total de minutos entre las 8:00 y las 21:00
            float minutosInicio = starthour * 60;
            float minutosFin = endhour * 60;

            // Calculamos cuántos minutos han pasado según el porcentaje del ciclo
            GameManager.instance.minutosActualesTotales = Mathf.Lerp(minutosInicio, minutosFin, pct);

            // Convertimos esos minutos totales a formato HH:mm
            int horas = Mathf.FloorToInt(GameManager.instance.minutosActualesTotales / 60);
            int minutos = Mathf.FloorToInt(GameManager.instance.minutosActualesTotales % 60);

            // Actualizamos el texto con formato de dos dígitos (00:00)
            clocktext.text = string.Format("{0:00}:{1:00}", horas, minutos);
      
            
        }

        void ActualizarFundido()
        {
            if (fadeImage == null) return;

            // Convertimos la hora actual de minutos totales a formato 24h decimal
            float horaActualDecimal = GameManager.instance.minutosActualesTotales / 60f;


            if (horaActualDecimal > darkeningstarttime)
            {
                // Calculamos el progreso entre la hora de inicio (20:00) y el final (21:00)
                // Esto nos da un valor entre 0 y 1
                float progresoOscurecimiento = (horaActualDecimal - darkeningstarttime) / (endhour - darkeningstarttime);

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
            if (horaActualDecimal > efecthour && GameManager.instance.numday < 3)
            {

            if (player != null && player.pickedobject != null)
            {
                // Verificamos si lo que tiene es un plato (no tiene BucketWater)
                if (player.IsHoldingDish())
                {
                    Debug.Log("¡Es tarde! El jugador suelta el plato automáticamente.");
                    player.Drop(); // Usamos el método Drop del Player que ya limpia todo
                }
            }


            if (isInto) menucauldron.CloseCauldron();

            daytext.text = "Día " + (GameManager.instance.numday + 1);

                // Centrar instantáneamente
                rectTextoDia.anchorMin = new Vector2(0.5f, 0.5f);
                rectTextoDia.anchorMax = new Vector2(0.5f, 0.5f);
                rectTextoDia.pivot = new Vector2(0.5f, 0.5f);
                rectTextoDia.anchoredPosition = Vector2.zero;

                // Tamaño grande y alineación
                daytext.fontSize = 80f;
                daytext.alignment = TextAlignmentOptions.Center;
            }
            else
            {
            daytext.text = "Día " + GameManager.instance.numday;
            }

        }
    void ResetBars()
    {
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.isResetting = true;
        }

        HungerSystem[] todosLosPandas = FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);

        foreach (HungerSystem panda in todosLosPandas)
        {
            panda.ResetParaNuevoDia();
        }

    }

}
                
       