using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public abstract class PinUIElement : MonoBehaviour
{

    [SerializeField] private Transform worldPosition;
    [SerializeField] new private Camera camera;

    RectTransform rect;
    Animator animator;

    //=========================================================================================================================
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (camera == null) camera = Camera.main;

        animator = GetComponent<Animator>();

        gameObject.SetActive(false);

    }

    void LateUpdate()
    {
        if (worldPosition)
        {
            // Usamos Vector3 para poder leer la Z
            Vector3 pos = camera.WorldToViewportPoint(worldPosition.position);

            // Si la Z es menor que 0, significa que el objeto está a nuestra espalda
            if (pos.z < 0)
            {
                // Lo oculto
                rect.localScale = Vector3.zero;
            }
            else
            {
                // Si esta delante su tamaño
                rect.localScale = Vector3.one;

                // Actualizamos su posición en la pantalla
                rect.anchorMax = (Vector2)pos;
                rect.anchorMin = (Vector2)pos;
            }
        }
    }
    //=========================================================================================================================

    // Esta función se tiene que hacer override y poner la condición que sea necesaria
    public abstract bool CheckCondition();

    // Desde MessageController llamamos a ésta función
    public void Evaluate()
    {
        bool conditionMet = CheckCondition();

        if (conditionMet)
        {
            Show();
        }
        else if (!conditionMet)
        {
            Hide();
        }
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        animator.SetTrigger("hide");
        //gameObject.SetActive(false);
    }
    public void Desactivar()
    {
        gameObject.SetActive(false);
    }
}
