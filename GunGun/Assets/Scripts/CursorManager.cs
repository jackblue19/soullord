using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorShoot;
    [SerializeField] private Texture2D cursorReLoad;
    private Vector2 hotspot = new Vector2(16, 48);
    private bool isReloading = false;

    void Start()
    {
        Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (!isReloading)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.SetCursor(cursorShoot, hotspot, CursorMode.Auto);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Cursor.SetCursor(cursorReLoad, hotspot, CursorMode.Auto);
            isReloading = true;
            Invoke("ResetCursor", 2f); // Quay lại con trỏ bình thường sau 2 giây
        }
    }

    private void ResetCursor()
    {
        Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
        isReloading = false;
    }
}
