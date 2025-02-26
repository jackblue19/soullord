using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject , lifeTime); // Xóa đạn sau thời gian tồn tại
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.CompareTag("Enemy") )
        {
            collision.GetComponent<EnemyBase>()?.TakeDamage(damage);
            Destroy(gameObject); // Hủy đạn khi va chạm quái
        }
        else if ( collision.CompareTag("Wall") )
        {
            Destroy(gameObject); // Đạn biến mất khi chạm tường
        }
    }
}
