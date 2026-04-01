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
            Vector2 pos = camera.WorldToViewportPoint(worldPosition.position);
            rect.anchorMax = pos;
            rect.anchorMin = pos;
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
