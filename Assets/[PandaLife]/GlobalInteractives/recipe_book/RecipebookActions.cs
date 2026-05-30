using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipebookActions : MonoBehaviour
{
    [System.Serializable]
    public class Recipe
    {
        [Tooltip("El nombre exacto del plato tal cual viene en los pedidos ('bambu cocido', etc.)")]
        public string recipename;

        [Tooltip("Arrastra aquí las imágenes de las caras de los pandas asignadas a ESTA receta en el inspector")]
        public List<Image> pandaFaces;
    }

    [SerializeField] private List<Recipe> recipes;
    [SerializeField] private Animator anim;

    public void Open_notebook()
    {
        anim.SetTrigger("Open");
        Updatefaces();
    }

    public void Close_notebook()
    {
        anim.SetTrigger("Close");
    }

    public void Updatefaces()
    {
        if (GameManager.instance == null) return;

        PandaRequest pandaReq = GameManager.instance.GetComponent<PandaRequest>();
        if (pandaReq == null) return;

        // 1. Obtenemos la lista de los pedidos que tienen los pandas hoy
        List<string> pedidosActuales = pandaReq.GetCurrentRequests();

        // 2. Primero, apagamos TODAS las caritas de todas las recetas para resetear la UI
        foreach (var r in recipes)
        {
            foreach (var faceImage in r.pandaFaces)
            {
                if (faceImage != null) faceImage.gameObject.SetActive(false);
            }
        }

        // 3. Recorremos los pedidos de los pandas (máximo 3 pandas basándonos en tu RequestManager)
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