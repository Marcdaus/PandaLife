using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombrereceta;
    [SerializeField] private TextMeshProUGUI tiempo;
    [SerializeField] private TextMeshProUGUI saciedad;
    [SerializeField] private TextMeshProUGUI ingredientes;

    [SerializeField] private RecipesData receta;

    private void Start()
    {
        ShowRecipe();
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
}
