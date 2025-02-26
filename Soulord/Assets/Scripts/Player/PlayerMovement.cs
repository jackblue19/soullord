using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal"); // Nhận input A/D (trái/phải)
        moveInput.y = Input.GetAxisRaw("Vertical");   // Nhận input W/S (lên/xuống)
        moveInput.Normalize(); // Đảm bảo di chuyển theo đường chéo không nhanh hơn
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
