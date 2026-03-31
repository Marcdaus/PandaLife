using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PinUIElement : MonoBehaviour
{

    [SerializeField] private Transform worldPosition;

    RectTransform rect;

    [SerializeField] new private Camera camera;

    Animator animator;


    void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (camera == null) camera = Camera.main;
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
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
