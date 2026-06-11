using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorPorDefecto;
    public Texture2D cursorAlClicar;

    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        Cursor.SetCursor(cursorPorDefecto, hotSpot, cursorMode);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorAlClicar, hotSpot, cursorMode);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorPorDefecto, hotSpot, cursorMode);
        }
    }
}
