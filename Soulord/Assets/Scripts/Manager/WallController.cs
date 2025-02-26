using UnityEngine;

public class WallController : MonoBehaviour
{
    private Collider2D wallCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        wallCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void LockWall()
    {
        wallCollider.enabled = true;
        spriteRenderer.enabled = true;
    }

    public void UnlockWall()
    {
        wallCollider.enabled = false;
        spriteRenderer.enabled = false;
    }
}
