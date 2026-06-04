using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public abstract class PinUIElement : MonoBehaviour
{

    [SerializeField] private Transform worldPosition;
    [SerializeField] private Camera ccamera;

    RectTransform rect;
    Animator animator;

    

    //=========================================================================================================================
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (ccamera == null) ccamera = Camera.main;

        animator = GetComponent<Animator>();

        gameObject.SetActive(false);

    }

    void LateUpdate()
    {
        if (worldPosition)
        {
            Vector3 viewportPos = ccamera.WorldToViewportPoint(worldPosition.position);

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
    public bool isTransitioning = false;

    // Desde MessageController llamamos a ésta función
    public void Evaluate()
    {
        if (isTransitioning) return;

        bool conditionMet = CheckCondition();


        if (conditionMet) Show();
        
        else Hide();
        
    }


    public void Show()
    {
        if (gameObject.activeSelf && animator.GetCurrentAnimatorStateInfo(0).IsName("Show")) return;

        gameObject.SetActive(true);
        animator.SetTrigger("show");
    }

    public virtual void Hide()
    {
        if (!gameObject.activeSelf) return;
        animator.SetTrigger("hide");
    }
    public void SetTransitionState(bool state)
    {
        isTransitioning = state;
    }
    public void Desactivar()
    {
        gameObject.SetActive(false);
    }
}
