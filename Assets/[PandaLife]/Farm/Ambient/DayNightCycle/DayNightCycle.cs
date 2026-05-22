using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class DayNightCycle : MonoBehaviour
{
    [Header("Configuración Escenas")]
    [SerializeField] private GameString homescene;
    [SerializeField] private GameString theend;
    [SerializeField] private bool isinto = false;
    [SerializeField] private bool ingameover = false ;

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
    [SerializeField] private Image fadeimage;
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

    [Header("Configuración de Color de la luces")]
    [SerializeField] private Light directionallight;
    private Light leftwindowlight;
    private Light rightwindowlight;
    private Light doorwindowlight;

    [Header("Momentos del día en formato de 24h (ej. 10.5 = 10:30)")]
    [SerializeField] private float timefindawn = 10f;
    [SerializeField] private float timeendsunset = 17f;
    [SerializeField] private float timeendnight = 20f;

    [Header("Colores de cada fase")]
    [SerializeField] private Color colordawn = new Color(1f, 0.4f, 0f); // Anaranjado amanecer
    [SerializeField] private Color colorday = new Color(1f, 0.957f, 0.839f);
    [SerializeField] private Color colorsunset = new Color(1f, 0.4f, 0f);  // Anaranjado anochecer
    [SerializeField] private Color colornight = new Color(0.1f, 0.1f, 0.2f);  // Azul oscuro/negro para la noche


    private RectTransform recttextday;

    private float durationsinseconds;
    private void Awake()
    {
        GameObject left = GameObject.Find("LeftWindowLight");
        if (left != null)
        {
            leftwindowlight = left.GetComponent<Light>();
        }

        GameObject right = GameObject.Find("RightWindowLight");
        if (right != null)
        {
            rightwindowlight = right.GetComponent<Light>();
        }

        GameObject door = GameObject.Find("DoorLight");
        if (door != null)
        {
            doorwindowlight = door.GetComponent<Light>();
        }
    }
    void Start()
    {
        durationsinseconds = durationinminuts * 60f;
        if(!ingameover)recttextday = daytext.GetComponent<RectTransform>();

            RewardManager.EvaluatelElement(note);  
            RewardManager.EvaluatelElement(tedy);
     
        RewardManager.EvaluateAllElements(elementlistbag);

        player = FindFirstObjectByType<Player>();
        pickupobject = FindFirstObjectByType<PickupDrop>();

    }

    void Update()
    {


        if (GameManager.instance.tiempoTranscurrido < durationsinseconds)
        {



            if (!ingameover) {
                if (!GameManager.instance.stopTime) GameManager.instance.tiempoTranscurrido += Time.deltaTime * GameManager.instance.multiplicadorvelocidaddia;
                GameManager.instance.porcentaje = GameManager.instance.tiempoTranscurrido / durationsinseconds;

                // 1. Control de Rotación
                float anguloActual = Mathf.Lerp(startangle, endangle, GameManager.instance.porcentaje);

                if (!isinto) transform.localEulerAngles = new Vector3(anguloActual, 0, 0);
                // 2. Control de Reloj
                ActualizarReloj(GameManager.instance.porcentaje);

                // 3. Control de obscurezers
                ActualizarFundido();

                // 4. Control del texto dia
                ActualizarTexto();

                // 5. Control del color de la cámara
            }
            else
            {
                // 1. Control de Rotación
                float anguloActual = Mathf.Lerp(startangle, endangle, GameManager.instance.porcentaje);
                transform.localEulerAngles = new Vector3(anguloActual, 0, 0);
            }

                ActualizarColorCamara();

        }
        else
        {
            
            GameManager.instance.tiempoTranscurrido = 0f;
            GameManager.instance.numday++;

            // Buscamos el PandaRequest que va a sobrevivir al cambio de escena
            PandaRequest persistentereq = GameManager.instance.GetComponent<PandaRequest>();

            if (persistentereq != null)
            {
                persistentereq.UnlockCropsForDay(GameManager.instance.numday);
                persistentereq.GenerateRandomRequests();
                Debug.Log("Nuevos pedidos generados para el día: " + GameManager.instance.numday);
            }
            else
            {
                Debug.LogError("El GameManager no tiene el script PandaRequest");
            }
            if(GameManager.instance.numday == 2) { 
                GameManager.instance.valuepercentage = 15;
                GameManager.instance.barmultiplicator = 1.15f;
            }
            if (GameManager.instance.numday == 3)
            {
                GameManager.instance.valuepercentage = 25;
                GameManager.instance.barmultiplicator = 1.25f;
            }

            
            // Guardar todos los platos sueltos antes de cambiar de día
            if (CauldronPersistenceManager.instance != null)
            {
                CauldronPersistenceManager.instance.ClearAllDishStates();
                Dish[] platos = FindObjectsByType<Dish>(FindObjectsSortMode.None);
                foreach (Dish dish in platos)
                {
                    GameObject raiz = dish.transform.root.gameObject;
                    CauldronPersistenceManager.instance.SaveDishState(
                        dish.GetReceta(),
                        raiz.transform.position,
                        inHand: false
                    );
                    Debug.Log($"[DayNightCycle] Guardando plato: {raiz.name} en {raiz.transform.position}");
                }
            }
            ResetBars();
            SceneManager.LoadScene(homescene.Value);
            
;
        }
        if(GameManager.instance.numday > 1)
        {
            percentage1.text = "+" + GameManager.instance.valuepercentage.ToString() + "%/s";
            percentage2.text = "+" + GameManager.instance.valuepercentage.ToString() + "%/s";
            percentage3.text = "+" + GameManager.instance.valuepercentage.ToString() + "%/s";
        }
        if (GameManager.instance.numday == 4)
        {
            GameManager.instance.Resetplay();
            SceneManager.LoadScene(theend.Value);
        }

    }
    


        void ActualizarReloj(float pct)
        {
            if (clocktext == null) return;

            // Calculamos el total de minutos entre las 8:00 y las 21:00
            float startminutes = starthour * 60;
            float endminutes = endhour * 60;

            // Calculamos cuántos minutos han pasado según el porcentaje del ciclo
            GameManager.instance.minutosActualesTotales = Mathf.Lerp(startminutes, endminutes, pct);

            // Convertimos esos minutos totales a formato HH:mm
            int hours = Mathf.FloorToInt(GameManager.instance.minutosActualesTotales / 60);
            int minutes = Mathf.FloorToInt(GameManager.instance.minutosActualesTotales % 60);

            // Actualizamos el texto con formato de dos dígitos (00:00)
            clocktext.text = string.Format("{0:00}:{1:00}", hours, minutes);
      
            
        }

        void ActualizarFundido()
        {
            if (fadeimage == null) return;

            // Convertimos la hora actual de minutos totales a formato 24h decimal
            float timeactualdecimal = GameManager.instance.minutosActualesTotales / 60f;


            if (timeactualdecimal > darkeningstarttime)
            {
                // Calculamos el progreso entre la hora de inicio (20:00) y el final (21:00)
                // Esto nos da un valor entre 0 y 1
                float progressdarkening = (timeactualdecimal - darkeningstarttime) / (endhour - darkeningstarttime);

                // Aplicamos ese progreso al Alpha de la imagen
                Color c = fadeimage.color;
                c.a = Mathf.Clamp01(progressdarkening);
                fadeimage.color = c;
            }
            else
            {

                // Si aún no son las 20:00, aseguramos que sea invisible
                Color c = fadeimage.color;
                c.a = 0f;
                fadeimage.color = c;
            }


        }
        void ActualizarTexto()
        {
            float timeactualdecimal = GameManager.instance.minutosActualesTotales / 60f;
            if (timeactualdecimal > efecthour && GameManager.instance.numday < 3)
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


            if (isinto) menucauldron.CloseCauldron();

            daytext.text = "Día " + (GameManager.instance.numday + 1);

                daytext.color = Color.white;
                // Centrar instantáneamente
                recttextday.anchorMin = new Vector2(0.5f, 0.5f);
                recttextday.anchorMax = new Vector2(0.5f, 0.5f);
                recttextday.pivot = new Vector2(0.5f, 0.5f);
                recttextday.anchoredPosition = Vector2.zero;

                // Tamaño grande y alineación
                daytext.fontSize = 80f;
                daytext.alignment = TextAlignmentOptions.Center;
            }
            else
            {
            daytext.text = "Día " + GameManager.instance.numday;
            }

        }

    void ActualizarColorCamara()
    {

        float currenttime = GameManager.instance.minutosActualesTotales / 60f;

        // Amanecer a Día
        if (currenttime >= starthour && currenttime < timefindawn)
        {
            float progress = (currenttime - starthour) / (timefindawn - starthour);
            // Transicion desde el naranja al amarillo
            directionallight.color = Color.Lerp(colordawn, colorday, progress);

            if (doorwindowlight != null)
                doorwindowlight.color = Color.Lerp(colordawn, colorday, progress);
            if (leftwindowlight != null)
                leftwindowlight.color = Color.Lerp(colordawn, colorday, progress);
            if (rightwindowlight != null)
                rightwindowlight.color = Color.Lerp(colordawn, colorday, progress);
        }
        // Pleno Día
        else if (currenttime >= timefindawn && currenttime < timeendsunset)
        {
            directionallight.color = colorday;
            if (doorwindowlight != null)
                doorwindowlight.color = colorday;
            if (leftwindowlight != null)
                leftwindowlight.color = colorday;
            if (rightwindowlight != null)
                rightwindowlight.color = colorday;
        }
        // Día a Atardecer
        else if (currenttime >= timeendsunset && currenttime < timeendnight)
        {
            float progress = (currenttime - timeendsunset) / (timeendnight - timeendsunset);
            directionallight.color = Color.Lerp(colorday, colorsunset, progress);
            if (doorwindowlight != null)
                doorwindowlight.color = Color.Lerp(colordawn, colorday, progress);
            if (leftwindowlight != null)
                leftwindowlight.color = Color.Lerp(colordawn, colorday, progress);
            if (rightwindowlight != null)
                rightwindowlight.color = Color.Lerp(colordawn, colorday, progress);
        }
        // Atardecer a Noche
        else if (currenttime >= timeendnight && currenttime <= endhour)
        {
            float progress = (currenttime - timeendnight) / (endhour - timeendnight);
            directionallight.color = Color.Lerp(colorsunset, colornight, progress);
            if (doorwindowlight != null)
                doorwindowlight.color = Color.Lerp(colordawn, colorday, progress);
            if (leftwindowlight != null)
                leftwindowlight.color = Color.Lerp(colordawn, colorday, progress);
            if (rightwindowlight != null)
                rightwindowlight.color = Color.Lerp(colordawn, colorday, progress);
        }
    }
    void ResetBars()
    {
        if (BarraManager.Instancia != null)
        {
            BarraManager.Instancia.hungerPaused = false;
            BarraManager.Instancia.isResetting = false;
        }

        // Buscamos los pandas actuales en la escena
        HungerSystem[] pandasonscene = FindObjectsByType<HungerSystem>(FindObjectsSortMode.None);

        foreach (var panda in pandasonscene)
        {

            panda.Restaurar(100f);


            panda.ResetSystem();


            panda.UpdateUI();
        }
        
    }
    

}
                
       