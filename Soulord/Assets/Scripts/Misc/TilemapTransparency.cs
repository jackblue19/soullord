using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTransparency : MonoBehaviour
{
    [Range(0f, 1f)] public float transparency = 0.5f;
    private TilemapRenderer tilemapRenderer;
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();

        if (tilemap != null)
        {
            SetTransparency(transparency);
        }
    }

    public void SetTransparency(float alpha)
    {
        if (tilemap == null) return;

        Color color = tilemap.color;
        color.a = Mathf.Clamp01(alpha);
        tilemap.color = color;
    }
}
