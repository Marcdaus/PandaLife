using UnityEngine;

public class Dish : MonoBehaviour
{
    [SerializeField]private RecipesData receta;

    private int saciedad;

    private int bambuverde;
    private int bamburojo;
    private int arandano;
    private int bayauchuva;
    private Color colormigajas;

    public RecipesData GetReceta() => receta;
    public void Initialize(RecipesData data)
    {
        receta = data;
        colormigajas = data.colormigajas;
        saciedad = data.saciedad;

        bambuverde = data.bambuverde;
        bamburojo = data.bamburojo;
        arandano = data.arandano;
        bayauchuva = data.bayauchuva;

        Debug.Log("Saciedad: " + saciedad);
        Debug.Log(GetIngredientesTexto());
    }

    public int GetSaciedad()
    {
        return saciedad;
    }

    public string GetIngredientesTexto()
    {
        string texto = "";

        if (bambuverde > 0) texto += "Bambu Verde x" + bambuverde + "\n";
        if (bamburojo > 0) texto += "Bambu Rojo x" + bamburojo + "\n";
        if (arandano > 0) texto += "Arandano x" + arandano + "\n";
        if (bayauchuva > 0) texto += "Baya Uchuva x" + bayauchuva + "\n";

        return texto;
    }

    public bool TieneIngrediente(string nombreIngrediente)
    {
        if (nombreIngrediente == "Bamboo" && bambuverde > 0) return true;
        if (nombreIngrediente == "RedDragon" && bamburojo > 0) return true;
        if (nombreIngrediente == "Blueberry" && arandano > 0) return true;
        if (nombreIngrediente == "Uchuva" && bayauchuva > 0) return true;

        return false;
    }

    public Color GetColor()
    {
        return colormigajas;
    }

}

