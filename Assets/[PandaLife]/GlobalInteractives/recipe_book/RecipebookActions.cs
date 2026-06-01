using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipebookActions : MonoBehaviour
{
    [System.Serializable]
    public class Recipe
    {
        public string recipename;

        public List<Image> pandaFaces;
    }

    [SerializeField] private List<Recipe> recipes;
    [SerializeField] private Animator anim;

    [SerializeField] private Image bamboosalad;
    [SerializeField] private Image bobatea;
    [SerializeField] private Image berrysoup;

    [SerializeField] private Sprite sbamboosalad;
    [SerializeField] private Sprite sbobatea;
    [SerializeField] private Sprite sberrysoup;

    public void Open_notebook()
    {
        anim.SetTrigger("Open");
        Updatefaces();
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.CompleteRecipeStep(TutorialManager.TutorialRecipeBook.OpenUchuva);
            TutorialManager.instance.CompleteRecipeStep(TutorialManager.TutorialRecipeBook.OpenRedDragon);
        }
    }

    public void Close_notebook()
    {
        anim.SetTrigger("Close");
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.CompleteRecipeStep(TutorialManager.TutorialRecipeBook.CloseUchuva);
            TutorialManager.instance.CompleteRecipeStep(TutorialManager.TutorialRecipeBook.CloseRedDragon);
        }
    }

    public void Updatefaces()
    {
        if(GameManager.instance.numday == 2)
        {
            bamboosalad.sprite = sbamboosalad;
        }
        else if(GameManager.instance.numday == 3)
        {
            bobatea.sprite = sbobatea;
            berrysoup.sprite = sberrysoup;
            bamboosalad.sprite = sbamboosalad;

        }
        if (GameManager.instance == null) return;

        PandaRequest pandaReq = GameManager.instance.GetComponent<PandaRequest>();
        if (pandaReq == null) return;

        //Obtenemos la lista de los pedidos que tienen los pandas hoy
        List<string> pedidosActuales = pandaReq.GetCurrentRequests();

        //Primero, apagamos TODAS las caritas de todas las recetas para resetear la UI
        foreach (var r in recipes)
        {
            foreach (var faceImage in r.pandaFaces)
            {
                if (faceImage != null) faceImage.gameObject.SetActive(false);
            }
        }

        //Recorremos los pedidos de los pandas (máximo 3 pandas basándonos en tu RequestManager)
        for (int iPanda = 0; iPanda < pedidosActuales.Count; iPanda++)
        {
            string pedidoDelPanda = pedidosActuales[iPanda];

            // Buscamos cuál receta en nuestro libro coincide con lo que quiere este panda
            foreach (var r in recipes)
            {
                if (r.recipename == pedidoDelPanda)
                {
                    // Comprobamos que esa receta tenga un slot de imagen configurado para este panda
                    if (iPanda < r.pandaFaces.Count && r.pandaFaces[iPanda] != null)
                    {
                        // Activamos la carita del panda correspondiente en esa receta
                        r.pandaFaces[iPanda].gameObject.SetActive(true);
                    }
                    break; // Saltamos a evaluar el siguiente panda
                }
            }
        }
    }
}