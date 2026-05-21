using UnityEngine;

[CreateAssetMenu(fileName = "RecipesData", menuName = "Recipes/RecipesData")]
public class RecipesData : ScriptableObject
{
    public string nombrereceta;
    public int diadesbloqueado;
    [SerializeField] public Color colorAgua;

    [Header("Ingredientes")]
    public int bambuverde;
    public int bamburojo;
    public int arandano;
    public int bayauchuva;

    [Header("Resultados")]
    public int saciedad;
    public int tiempopreparacion;
    public GameObject prefabResultado;
}
