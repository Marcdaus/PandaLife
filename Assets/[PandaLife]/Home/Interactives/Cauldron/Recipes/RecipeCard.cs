using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombrereceta;
    [SerializeField] private TextMeshProUGUI tiempo;
    [SerializeField] private TextMeshProUGUI saciedad;
    [SerializeField] private TextMeshProUGUI ingredientes;

    [SerializeField] private RecipesData receta;
    [SerializeField] private MenuCauldron menucauldron;

    private Button cookingbutton;
    private Color colorOriginal;

    [SerializeField] private GameObject iconocandado;

    private void Awake()
    {
        cookingbutton = GetComponent<Button>();
        colorOriginal = GetComponent<Button>().image.color;
        if (iconocandado != null) iconocandado.SetActive(false);
        ShowRecipe();
        cookingbutton.onClick.AddListener(OnClick);

    }

    private void OnClick()
    {
        menucauldron.StartCooking(receta);
    }

    private void ShowRecipe()
    {
        nombrereceta.text = receta.nombrereceta;
        tiempo.text = receta.tiempopreparacion + "s";
        saciedad.text = receta.saciedad + "%";
        ingredientes.text = ShowIngredients();
    }

    private string ShowIngredients() {

        var ingredientes = new List<(string nombre, int cantidad)>
       {
           ("Bambú Verde", receta.bambuverde),
           ("Bambú Rojo", receta.bamburojo),
           ("Arándano", receta.arandano),
           ("Baya Uchuva", receta.bayauchuva)
       };

        string texto = "";
        foreach(var (nombre, cantidad) in ingredientes)
        {
            if (cantidad > 0)
            {
                texto+= nombre + " x" + cantidad + "\n";
            }
        }
        return texto;
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
        cookingbutton.image.color = Color.black;
        if (iconocandado != null) iconocandado.SetActive(true);
        // aquí ponemos luego el candado
    }

    public void Block()
    {
        cookingbutton.interactable = false;
        cookingbutton.image.color = Color.gray;
    }

    public void UnBlock()
    {
        cookingbutton.interactable = true;
        cookingbutton.image.color = colorOriginal;
        if (iconocandado != null) iconocandado.SetActive(false);
    }
}
