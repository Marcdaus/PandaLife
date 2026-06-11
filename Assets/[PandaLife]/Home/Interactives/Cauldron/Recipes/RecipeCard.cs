using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class RecipeCard : MonoBehaviour
{


    [SerializeField] private RecipesData receta;
    [SerializeField] private MenuCauldron menucauldron;

    private Button cookingbutton;
    private Color colorOriginal;

    [SerializeField] private GameObject iconocandado;

    [Header("imagenes por defecto")]
    [SerializeField] private Sprite recipe;

    [Header("imagenes sin recursos")]
    [SerializeField] private Sprite norecipe;

    [Header("imagenes bloqueadas")]

    [SerializeField] private Sprite lockrecipe;


    public Button button;


    private void Awake()
    {
        cookingbutton = GetComponent<Button>();
        colorOriginal = GetComponent<Button>().image.color;
        cookingbutton.onClick.AddListener(OnClick);

    }

    private void Start()
    {
        // Si está cocinando, bloquear en gris directamente
        if (CauldronPersistenceManager.instance != null && CauldronPersistenceManager.instance.isCooking)
        {
            if (GameManager.instance.numday < receta.diadesbloqueado)
                BlockedByDay();
            else
                Block();
            return;
        }
        CheckUnblock();
    }

    private void OnClick()
    {
        menucauldron.StartCooking(receta);
    }

    public void CheckIngredients()
    {
        if (!menucauldron.HasIngredients(receta)) Block();
        else UnBlock();
    }

    public void CheckUnblock()
    {
        Debug.Log($"{receta.nombrereceta} - DiaDesbloqueo: {receta.diadesbloqueado} - DiaActual: {GameManager.instance.numday}");

        if (GameManager.instance.numday < receta.diadesbloqueado)
        {
            BlockedByDay();
            return;
        }
        CheckIngredients();
    }

    public void BlockedByDay()
    {
        cookingbutton.interactable = false;
        cookingbutton.image.sprite = lockrecipe;
        if (iconocandado != null) iconocandado.SetActive(true);
        GetComponent<EventTrigger>().enabled = false;
        // aquí ponemos luego el candado
    }

    public void Block()
    {
        //cookingbutton.interactable = false;
        cookingbutton.image.sprite = norecipe;
        GetComponent<EventTrigger>().enabled = false;
        button.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
        button.onClick.SetPersistentListenerState(1, UnityEventCallState.RuntimeOnly);
    }

    public void UnBlock()
    {
        //cookingbutton.interactable = true;
        cookingbutton.image.sprite = recipe;
        if (iconocandado != null) iconocandado.SetActive(false);
        GetComponent<EventTrigger>().enabled = true;
        button.onClick.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
        button.onClick.SetPersistentListenerState(1, UnityEventCallState.Off);
    }
}
