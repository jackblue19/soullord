using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTransparency : MonoBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        SetTransparency(0.5f);
    }

    public void SetTransparency(float alpha)
    {
        if (tilemap != null)
        {
            Color color = tilemap.color;
            color.a = Mathf.Clamp01(alpha);
            tilemap.color = color;
        }
    }
}
