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
            Vector3 viewportPos = camera.WorldToViewportPoint(worldPosition.position);

            if (viewportPos.z < 0)
            {
                rect.anchorMax = new Vector2(-2f, -2f);
                rect.anchorMin = new Vector2(-2f, -2f);
            }
            else
            {
                rect.anchorMax = (Vector2)viewportPos;
                rect.anchorMin = (Vector2)viewportPos;
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
